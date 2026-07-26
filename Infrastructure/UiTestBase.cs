using System.Diagnostics;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace MauiUkraine.UITests.Infrastructure;

public abstract class UiTestBase
{
	private UIA3Automation? _automation;
	private Application? _application;

	protected Window MainWindow { get; private set; } = null!;

	[SetUp]
	public void SetUp()
	{
		EnsureAppExists();
		CloseLeftoverAppProcesses();

		_automation = new UIA3Automation();
		_application = Application.Launch(AppConfig.AppPath);
		MainWindow = WaitForMainWindow(_application, _automation);

		MainWindow.Focus();
		try
		{
			MainWindow.SetForeground();
		}
		catch
		{
			// ignored
		}

		// WinUI/MAUI needs a short settle time before TapGestureRecognizer clicks are reliable.
		UiWait.WaitForAutomationId(MainWindow, AutomationIds.Home.HeroTitle);
		UiWait.WaitForAutomationId(MainWindow, AutomationIds.Home.MenuButtons);
		EnsureEnglishLanguage();
	}

	/// <summary>
	/// App language is persisted in Preferences; force English for this suite.
	/// </summary>
	private void EnsureEnglishLanguage()
	{
		var hero = UiWait.WaitForAutomationId(MainWindow, AutomationIds.Home.HeroTitle);
		if (hero.Name?.Contains(EnglishTexts.Home.HeroTitle, StringComparison.Ordinal) == true)
		{
			return;
		}

		UiWait.ClickByAutomationId(MainWindow, AutomationIds.Home.LanguageEn);
		UiWait.WaitForNameContains(
			MainWindow,
			AutomationIds.Home.HeroTitle,
			EnglishTexts.Home.HeroTitle,
			TimeSpan.FromSeconds(10));
		UiWait.WaitForAutomationId(MainWindow, AutomationIds.Home.MenuButtons);
	}

	[TearDown]
	public void TearDown()
	{
		try
		{
			_application?.Close();
		}
		catch
		{
			// ignored
		}

		try
		{
			_application?.Dispose();
		}
		catch
		{
			// ignored
		}

		_automation?.Dispose();
		_application = null;
		_automation = null;

		CloseLeftoverAppProcesses();
	}

	private static void EnsureAppExists()
	{
		if (File.Exists(AppConfig.AppPath))
		{
			return;
		}

		Assert.Fail(
			$"Check that app exists at '{AppConfig.AppPath}'. " +
			"Build MauiControlsShowcase for Windows (net10.0-windows10.0.19041.0) " +
			"or set MAUI_UKRAINE_APP_PATH to the .exe path.");
	}

	private static Window WaitForMainWindow(Application app, UIA3Automation automation)
	{
		var deadline = DateTime.UtcNow + AppConfig.DefaultTimeout;
		Exception? lastError = null;

		while (DateTime.UtcNow < deadline)
		{
			try
			{
				app.WaitWhileMainHandleIsMissing(AppConfig.DefaultTimeout);
				var window = app.GetMainWindow(automation, TimeSpan.FromSeconds(5));
				if (window is not null)
				{
					window.Focus();
					return window;
				}
			}
			catch (Exception ex)
			{
				lastError = ex;
			}

			Thread.Sleep(AppConfig.PollInterval);
		}

		throw new TimeoutException(
			$"Main window did not appear for '{AppConfig.AppPath}'. Last error: {lastError?.Message}");
	}

	private static void CloseLeftoverAppProcesses()
	{
		foreach (var process in Process.GetProcessesByName(AppConfig.ProcessName))
		{
			try
			{
				if (!process.HasExited)
				{
					process.Kill(entireProcessTree: true);
					process.WaitForExit(5000);
				}
			}
			catch
			{
				// ignored
			}
			finally
			{
				process.Dispose();
			}
		}
	}
}
