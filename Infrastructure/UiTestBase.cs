using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace MauiUkraine.UITests.Infrastructure;

public abstract class UiTestBase
{
    private UIA3Automation? _automation;
    private Application? _application;

    protected Window MainWindow { get; private set; } = default!;

    [SetUp]
    public void SetUp()
    {
        EnsureAppExists();
        CloseLeftoverAppProcesses();
        LaunchApplication();
        PrepareMainWindow();
        EnsureEnglishLanguage();
    }

    [TearDown]
    public void TearDown()
    {
        _application?.Close();
        _application?.Dispose();

        _automation?.Dispose();

        _application = null;
        _automation = null;

        CloseLeftoverAppProcesses();
    }

    private void LaunchApplication()
    {
        _automation = new UIA3Automation();
        _application = Application.Launch(AppConfig.AppPath);

        MainWindow = WaitForMainWindow(_application, _automation);
    }

    private void PrepareMainWindow()
    {
        MainWindow.Focus();

        try
        {
            MainWindow.SetForeground();
        }
        catch (COMException)
        {
            // Windows may deny bringing the window to foreground.
        }

        UiWait.WaitForAutomationId(MainWindow, AutomationIds.Home.HeroTitle);
        UiWait.WaitForAutomationId(MainWindow, AutomationIds.Home.MenuButtons);
    }
    
    private void EnsureEnglishLanguage()
    {
        var hero = UiWait.WaitForAutomationId(
            MainWindow,
            AutomationIds.Home.HeroTitle);

        if (hero.Name?.Contains(
                EnglishTexts.Home.HeroTitle,
                StringComparison.Ordinal) == true)
        {
            return;
        }

        UiWait.ClickByAutomationId(MainWindow,AutomationIds.Home.LanguageEn);

        UiWait.WaitForNameContains(MainWindow,AutomationIds.Home.HeroTitle,EnglishTexts.Home.HeroTitle,
            TimeSpan.FromSeconds(10));

        UiWait.WaitForAutomationId(MainWindow,AutomationIds.Home.MenuButtons);
    }

    private static void EnsureAppExists()
    {
        if (File.Exists(AppConfig.AppPath))
            return;

        Assert.Fail(
            $"Application was not found at '{AppConfig.AppPath}'. " +
            "Build the MAUI application or update AppPath in .runsettings.");
    }

    private static Window WaitForMainWindow(Application application,UIA3Automation automation)
    {
        var timeout = AppConfig.DefaultTimeout;
        var deadline = DateTime.UtcNow + timeout;

        Exception? lastError = null;

        while (DateTime.UtcNow < deadline)
        {
            try
            {
                application.WaitWhileMainHandleIsMissing(
                    TimeSpan.FromMilliseconds(300));

                var window = application.GetMainWindow(
                    automation,
                    TimeSpan.FromSeconds(1));

                if (window != null)
                {
                    window.Focus();
                    return window;
                }
            }
            catch (COMException ex)
            {
                lastError = ex;
            }
            catch (TimeoutException ex)
            {
                lastError = ex;
            }

            Thread.Sleep(AppConfig.PollInterval);
        }

        throw new TimeoutException(
            $"Main window did not appear within {timeout.TotalSeconds:0} seconds." +
            (lastError != null ? $" Last error: {lastError.Message}" : ""));
    }

    private static void CloseLeftoverAppProcesses()
    {
        foreach (var process in Process.GetProcessesByName(AppConfig.ProcessName))
        {
            using (process)
            {
                try
                {
                    if (process.HasExited)
                        continue;

                    process.Kill(entireProcessTree: true);
                    process.WaitForExit(5000);
                }
                catch (InvalidOperationException)
                {
                    // Process exited before it could be terminated.
                }
                catch (Win32Exception)
                {
                    // Access denied or process already terminating.
                }
            }
        }
    }
}