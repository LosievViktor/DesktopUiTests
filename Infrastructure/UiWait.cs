using System.Drawing;
using System.Runtime.InteropServices;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;

namespace MauiUkraine.UITests.Infrastructure;

public static class UiWait
{
    public static AutomationElement WaitForAutomationId(AutomationElement root,string automationId, TimeSpan? timeout = null)
    {
        var waitTime = timeout ?? AppConfig.DefaultTimeout;
        var deadline = DateTime.UtcNow + waitTime;

        Exception? lastError = null;

        while (DateTime.UtcNow < deadline)
        {
            try
            {
                var element = root.FindFirstDescendant(cf => cf.ByAutomationId(automationId));

                if (element != null)
                    return element;
            }
            catch (COMException ex)
            {
                lastError = ex;
            }

            Thread.Sleep(AppConfig.PollInterval);
        }

        throw new TimeoutException(
            $"Element with AutomationId '{automationId}' was not found within {waitTime.TotalSeconds:0}s." +
            (lastError != null ? $" Last error: {lastError.Message}" : ""));
    }

    public static AutomationElement WaitForName(AutomationElement root,string name,ControlType? controlType = null, TimeSpan? timeout = null)
    {
        var waitTime = timeout ?? AppConfig.DefaultTimeout;
        var deadline = DateTime.UtcNow + waitTime;

        Exception? lastError = null;

        while (DateTime.UtcNow < deadline)
        {
            try
            {
                AutomationElement? element = controlType == null
                    ? root.FindFirstDescendant(cf => cf.ByName(name))
                    : root.FindFirstDescendant(cf =>
                        cf.ByName(name)
                            .And(cf.ByControlType(controlType.Value)));

                if (element != null)
                    return element;
            }
            catch (COMException ex)
            {
                lastError = ex;
            }

            Thread.Sleep(AppConfig.PollInterval);
        }

        throw new TimeoutException(
            $"Element with Name '{name}' was not found within {waitTime.TotalSeconds:0}s." +
            (lastError != null ? $" Last error: {lastError.Message}" : ""));
    }

    public static bool Exists(AutomationElement root,string automationId,TimeSpan? timeout = null)
    {
        try
        {
            WaitForAutomationId(root, automationId, timeout ?? TimeSpan.FromSeconds(3));
            return true;
        }
        catch (TimeoutException)
        {
            return false;
        }
    }

    public static void WaitUntil(Func<bool> condition,TimeSpan? timeout = null, string? message = null)
    {
        var deadline = DateTime.UtcNow + (timeout ?? AppConfig.DefaultTimeout);

        while (DateTime.UtcNow < deadline)
        {
            if (condition()) return;
            Thread.Sleep(AppConfig.PollInterval);
        }

        throw new TimeoutException(message ?? "Condition was not met within the allotted timeout.");
    }

    public static void WaitForNameContains(AutomationElement root, string automationId, string expectedSubstring, TimeSpan? timeout = null)
    {
        WaitUntil(
            () =>
            {
                var element = root.FindFirstDescendant(cf => cf.ByAutomationId(automationId));

                return element != null &&
                       element.Name?.Contains(
                           expectedSubstring,
                           StringComparison.OrdinalIgnoreCase) == true;
            },
            timeout,
            $"Element '{automationId}' did not contain '{expectedSubstring}' in Name.");
    }

    public static void ClickByAutomationId( Window window, string automationId, TimeSpan? timeout = null)
    {
        window.Focus();
        window.SetForeground();

        var element = WaitForAutomationId(window, automationId, timeout);

        EnsureOnScreen(window, element);
        ClickElement(element);
    }

    public static AutomationElement WaitForAutomationIdScrolling(Window window,string automationId,TimeSpan? timeout = null)
    {
        var waitTime = timeout ?? AppConfig.DefaultTimeout;
        var deadline = DateTime.UtcNow + waitTime;

        Exception? lastError = null;

        while (DateTime.UtcNow < deadline)
        {
            try
            {
                var element = window.FindFirstDescendant(cf => cf.ByAutomationId(automationId));

                if (element != null)
                {
                    EnsureOnScreen(window, element);
                    return element;
                }
            }
            catch (COMException ex)
            {
                lastError = ex;
            }

            ScrollDown(window);

            Thread.Sleep(AppConfig.PollInterval);
        }

        throw new TimeoutException(
            $"Element with AutomationId '{automationId}' was not found within {waitTime.TotalSeconds:0}s." +
            (lastError != null ? $" Last error: {lastError.Message}" : ""));
    }

    public static void EnsureOnScreen(Window window,AutomationElement element)
    {
        if (element.Patterns.ScrollItem.IsSupported)
        {
            try
            {
                element.Patterns.ScrollItem.Pattern.ScrollIntoView();

                Thread.Sleep(200);

                return;
            }
            catch (COMException)
            {
            }
        }

        var windowBounds = window.BoundingRectangle;

        for (var i = 0; i < 12; i++)
        {
            var bounds = element.BoundingRectangle;

            if (bounds.Width > 0 &&
                bounds.Height > 0 &&
                bounds.Top >= windowBounds.Top &&
                bounds.Bottom <= windowBounds.Bottom)
            {
                return;
            }

            ScrollDown(window);

            Thread.Sleep(150);
        }
    }

    public static void ScrollDown(Window window)
    {
        window.Focus();

        var bounds = window.BoundingRectangle;

        Mouse.Position = new Point(
            bounds.X + bounds.Width / 2,
            bounds.Y + bounds.Height / 2);

        Mouse.Scroll(-3);
    }

    public static void ScrollToTop(Window window)
    {
        window.Focus();

        var bounds = window.BoundingRectangle;

        Mouse.Position = new Point(
            bounds.X + bounds.Width / 2,
            bounds.Y + bounds.Height / 2);

        for (var i = 0; i < 8; i++)
        {
            Mouse.Scroll(5);

            Thread.Sleep(100);
        }
    }

    public static void ClickElement(AutomationElement element)
    {
        element.Focus();

        if (element.Patterns.Invoke.IsSupported)
        {
            element.Patterns.Invoke.Pattern.Invoke();
            return;
        }

        if (element.TryGetClickablePoint(out var point))
        {
            Mouse.Click(point);
            return;
        }

        var bounds = element.BoundingRectangle;

        Mouse.Click(new Point(
            bounds.X + bounds.Width / 2,
            bounds.Y + bounds.Height / 2));
    }
}