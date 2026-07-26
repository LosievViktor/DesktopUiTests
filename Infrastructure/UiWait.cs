using System.Drawing;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;

namespace MauiUkraine.UITests.Infrastructure;

public static class UiWait
{
	public static AutomationElement WaitForAutomationId(
		AutomationElement root,
		string automationId,
		TimeSpan? timeout = null)
	{
		var deadline = DateTime.UtcNow + (timeout ?? AppConfig.DefaultTimeout);
		Exception? lastError = null;

		while (DateTime.UtcNow < deadline)
		{
			try
			{
				var element = root.FindFirstDescendant(cf => cf.ByAutomationId(automationId));
				if (element is not null)
				{
					return element;
				}
			}
			catch (Exception ex)
			{
				lastError = ex;
			}

			Thread.Sleep(AppConfig.PollInterval);
		}

		var detail = lastError is null ? string.Empty : $" Last error: {lastError.Message}";
		throw new TimeoutException(
			$"Element with AutomationId '{automationId}' was not found within {(timeout ?? AppConfig.DefaultTimeout).TotalSeconds:0}s.{detail}");
	}

	public static bool Exists(AutomationElement root, string automationId, TimeSpan? timeout = null)
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

	public static void ClickByAutomationId(Window window, string automationId, TimeSpan? timeout = null)
	{
		window.Focus();
		try
		{
			window.SetForeground();
		}
		catch
		{
			// ignored
		}

		var element = WaitForAutomationId(window, automationId, timeout);
		ClickElement(element);
	}

	public static void ClickElement(AutomationElement element)
	{
		try
		{
			element.Focus();
		}
		catch
		{
			// ignored
		}

		if (element.Patterns.Invoke.IsSupported)
		{
			element.Patterns.Invoke.Pattern.Invoke();
			return;
		}

		try
		{
			var point = element.GetClickablePoint();
			Mouse.Click(point);
			return;
		}
		catch
		{
			// fall through
		}

		var bounds = element.BoundingRectangle;
		if (bounds.Width > 0 && bounds.Height > 0)
		{
			Mouse.Click(new Point(bounds.X + Math.Min(40, bounds.Width / 2), bounds.Y + Math.Min(20, bounds.Height / 2)));
			return;
		}

		element.Click();
	}
}
