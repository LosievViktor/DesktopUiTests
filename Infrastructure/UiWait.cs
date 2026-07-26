using System.Drawing;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
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

	public static AutomationElement WaitForName(
		AutomationElement root,
		string name,
		ControlType? controlType = null,
		TimeSpan? timeout = null)
	{
		var deadline = DateTime.UtcNow + (timeout ?? AppConfig.DefaultTimeout);
		Exception? lastError = null;

		while (DateTime.UtcNow < deadline)
		{
			try
			{
				var element = controlType is null
					? root.FindFirstDescendant(cf => cf.ByName(name))
					: root.FindFirstDescendant(cf => cf.ByName(name).And(cf.ByControlType(controlType.Value)));

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
			$"Element with Name '{name}' was not found within {(timeout ?? AppConfig.DefaultTimeout).TotalSeconds:0}s.{detail}");
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

	public static void WaitUntil(Func<bool> condition, TimeSpan? timeout = null, string? message = null)
	{
		var deadline = DateTime.UtcNow + (timeout ?? AppConfig.DefaultTimeout);
		while (DateTime.UtcNow < deadline)
		{
			try
			{
				if (condition())
				{
					return;
				}
			}
			catch
			{
				// keep polling
			}

			Thread.Sleep(AppConfig.PollInterval);
		}

		throw new TimeoutException(message ?? "Condition was not met within the allotted timeout.");
	}

	public static void WaitForNameContains(
		AutomationElement root,
		string automationId,
		string expectedSubstring,
		TimeSpan? timeout = null)
	{
		WaitUntil(
			() =>
			{
				var element = root.FindFirstDescendant(cf => cf.ByAutomationId(automationId));
				return element is not null
					&& (element.Name?.Contains(expectedSubstring, StringComparison.OrdinalIgnoreCase) ?? false);
			},
			timeout,
			$"Element '{automationId}' did not contain '{expectedSubstring}' in Name.");
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
		EnsureOnScreen(window, element);
		ClickElement(element);
	}

	/// <summary>
	/// Scrolls the focused page until the element with the given AutomationId appears (or is on-screen).
	/// </summary>
	public static AutomationElement WaitForAutomationIdScrolling(
		Window window,
		string automationId,
		TimeSpan? timeout = null)
	{
		var deadline = DateTime.UtcNow + (timeout ?? AppConfig.DefaultTimeout);
		Exception? lastError = null;

		while (DateTime.UtcNow < deadline)
		{
			try
			{
				var element = window.FindFirstDescendant(cf => cf.ByAutomationId(automationId));
				if (element is not null)
				{
					EnsureOnScreen(window, element);
					return element;
				}
			}
			catch (Exception ex)
			{
				lastError = ex;
			}

			ScrollDown(window);
			Thread.Sleep(AppConfig.PollInterval);
		}

		var detail = lastError is null ? string.Empty : $" Last error: {lastError.Message}";
		throw new TimeoutException(
			$"Element with AutomationId '{automationId}' was not found (with scrolling) within {(timeout ?? AppConfig.DefaultTimeout).TotalSeconds:0}s.{detail}");
	}

	public static void EnsureOnScreen(Window window, AutomationElement element)
	{
		try
		{
			if (element.Patterns.ScrollItem.IsSupported)
			{
				element.Patterns.ScrollItem.Pattern.ScrollIntoView();
				Thread.Sleep(200);
				return;
			}
		}
		catch
		{
			// fall through
		}

		var windowBounds = window.BoundingRectangle;
		for (var i = 0; i < 12; i++)
		{
			var bounds = element.BoundingRectangle;
			if (bounds.Width > 0 && bounds.Height > 0
				&& bounds.Top >= windowBounds.Top
				&& bounds.Bottom <= windowBounds.Bottom)
			{
				return;
			}

			ScrollDown(window);
			Thread.Sleep(150);
		}
	}

	public static void ScrollDown(Window window)
	{
		try
		{
			window.Focus();
			var bounds = window.BoundingRectangle;
			if (bounds.Width > 0 && bounds.Height > 0)
			{
				Mouse.Position = new Point(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);
			}
		}
		catch
		{
			// ignored
		}

		Mouse.Scroll(-3);
	}

	public static void ScrollToTop(Window window)
	{
		for (var i = 0; i < 8; i++)
		{
			try
			{
				window.Focus();
				var bounds = window.BoundingRectangle;
				if (bounds.Width > 0 && bounds.Height > 0)
				{
					Mouse.Position = new Point(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);
				}
			}
			catch
			{
				// ignored
			}

			Mouse.Scroll(5);
			Thread.Sleep(100);
		}
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
