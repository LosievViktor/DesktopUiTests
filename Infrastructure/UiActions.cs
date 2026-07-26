using System.Drawing;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;

namespace MauiUkraine.UITests.Infrastructure;

public static class UiActions
{
	public static string GetName(AutomationElement element)
		=> element.Name ?? string.Empty;

	public static void SetText(AutomationElement element, string text)
	{
		element.Focus();
		UiWait.ClickElement(element);

		// Always clear first — SearchBar/Entry on WinUI often appends when Value is set repeatedly.
		try
		{
			Keyboard.TypeSimultaneously(VirtualKeyShort.CONTROL, VirtualKeyShort.KEY_A);
			Keyboard.Type(VirtualKeyShort.DELETE);
			Thread.Sleep(100);
		}
		catch
		{
			// ignored
		}

		try
		{
			var textBox = element.AsTextBox();
			if (textBox is not null)
			{
				textBox.Text = text;
				Thread.Sleep(200);
				return;
			}
		}
		catch
		{
			// fall through to Value pattern / keyboard
		}

		if (element.Patterns.Value.IsSupported)
		{
			element.Patterns.Value.Pattern.SetValue(text);
			Thread.Sleep(200);
			return;
		}

		Keyboard.Type(text);
		Thread.Sleep(200);
	}

	public static string GetText(AutomationElement element)
	{
		try
		{
			var textBox = element.AsTextBox();
			if (textBox is not null && textBox.Text is not null)
			{
				return textBox.Text;
			}
		}
		catch
		{
			// ignored
		}

		if (element.Patterns.Value.IsSupported)
		{
			return element.Patterns.Value.Pattern.Value ?? string.Empty;
		}

		return GetName(element);
	}

	public static void Toggle(AutomationElement element)
	{
		if (element.Patterns.Toggle.IsSupported)
		{
			element.Patterns.Toggle.Pattern.Toggle();
			Thread.Sleep(200);
			return;
		}

		UiWait.ClickElement(element);
		Thread.Sleep(200);
	}

	public static bool IsOn(AutomationElement element)
	{
		if (element.Patterns.Toggle.IsSupported)
		{
			return element.Patterns.Toggle.Pattern.ToggleState == ToggleState.On;
		}

		if (element.Patterns.SelectionItem.IsSupported)
		{
			return element.Patterns.SelectionItem.Pattern.IsSelected;
		}

		return false;
	}

	public static void Select(AutomationElement element)
	{
		if (element.Patterns.SelectionItem.IsSupported)
		{
			element.Patterns.SelectionItem.Pattern.Select();
			Thread.Sleep(200);
			return;
		}

		UiWait.ClickElement(element);
		Thread.Sleep(200);
	}

	public static void SetRangeValue(AutomationElement element, double value)
	{
		if (!element.Patterns.RangeValue.IsSupported)
		{
			throw new InvalidOperationException(
				$"Element '{element.AutomationId}' does not support RangeValue pattern.");
		}

		element.Patterns.RangeValue.Pattern.SetValue(value);
		Thread.Sleep(200);
	}

	public static double? TryGetRangeValue(AutomationElement element)
	{
		if (!element.Patterns.RangeValue.IsSupported)
		{
			return null;
		}

		return element.Patterns.RangeValue.Pattern.Value;
	}

	public static void SelectComboItem(AutomationElement picker, string itemName)
	{
		var combo = picker.AsComboBox();
		if (combo is not null)
		{
			try
			{
				combo.Select(itemName);
				Thread.Sleep(300);
				return;
			}
			catch
			{
				// fall through
			}

			try
			{
				combo.Expand();
				Thread.Sleep(300);
			}
			catch
			{
				// ignored
			}
		}
		else
		{
			UiWait.ClickElement(picker);
			Thread.Sleep(300);
		}

		var item = UiWait.WaitForName(picker.Parent ?? picker, itemName, ControlType.ListItem, TimeSpan.FromSeconds(5));
		UiWait.ClickElement(item);
		Thread.Sleep(300);
	}

	public static void ClickDialogButton(Window window, string buttonName, TimeSpan? timeout = null)
	{
		var deadline = DateTime.UtcNow + (timeout ?? AppConfig.DefaultTimeout);
		Exception? lastError = null;
		var desktop = window.Automation.GetDesktop();

		while (DateTime.UtcNow < deadline)
		{
			try
			{
				foreach (var root in new[] { (AutomationElement)window, desktop })
				{
					// Prefer a true Button, but ActionSheets on Windows may expose options as other types.
					var button = root.FindFirstDescendant(cf =>
						cf.ByName(buttonName).And(cf.ByControlType(ControlType.Button)));
					if (button is not null)
					{
						UiWait.ClickElement(button);
						Thread.Sleep(400);
						return;
					}

					var listItem = root.FindFirstDescendant(cf =>
						cf.ByName(buttonName).And(cf.ByControlType(ControlType.ListItem)));
					if (listItem is not null)
					{
						UiWait.ClickElement(listItem);
						Thread.Sleep(400);
						return;
					}

					var any = root.FindFirstDescendant(cf => cf.ByName(buttonName));
					if (any is not null)
					{
						UiWait.ClickElement(any);
						Thread.Sleep(400);
						return;
					}
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
			$"Dialog option '{buttonName}' was not found within {(timeout ?? AppConfig.DefaultTimeout).TotalSeconds:0}s.{detail}");
	}

	public static bool TryClickDialogButton(Window window, string buttonName, TimeSpan? timeout = null)
	{
		try
		{
			ClickDialogButton(window, buttonName, timeout ?? TimeSpan.FromSeconds(5));
			return true;
		}
		catch (TimeoutException)
		{
			return false;
		}
	}

	public static void IncrementStepper(AutomationElement stepper)
	{
		// MAUI Stepper on Windows exposes two buttons; prefer the rightmost / "+" control.
		var buttons = stepper.FindAllDescendants(cf => cf.ByControlType(ControlType.Button));
		if (buttons.Length == 0)
		{
			buttons = stepper.Parent?.FindAllDescendants(cf => cf.ByControlType(ControlType.Button))
				?? Array.Empty<AutomationElement>();
		}

		if (buttons.Length >= 2)
		{
			UiWait.ClickElement(buttons[^1]);
			Thread.Sleep(200);
			return;
		}

		if (buttons.Length == 1)
		{
			UiWait.ClickElement(buttons[0]);
			Thread.Sleep(200);
			return;
		}

		var bounds = stepper.BoundingRectangle;
		if (bounds.Width > 0 && bounds.Height > 0)
		{
			Mouse.Click(new Point(bounds.Right - 12, bounds.Y + bounds.Height / 2));
			Thread.Sleep(200);
			return;
		}

		throw new InvalidOperationException("Could not locate stepper increment control.");
	}

	public static void IncrementTeamSizeFromLabel(Window window)
	{
		var label = window.FindFirstDescendant(cf => cf.ByName("Team size: 3"))
			?? window.FindFirstDescendant(cf => cf.ByName("Team size: 2"))
			?? UiWait.WaitForName(window, "Team size: 3", timeout: TimeSpan.FromSeconds(10));

		UiWait.EnsureOnScreen(window, label);

		// Walk up a few parents to reach the Grid that also hosts the Stepper buttons.
		AutomationElement? container = label;
		AutomationElement[] buttons = Array.Empty<AutomationElement>();
		for (var depth = 0; depth < 4 && container is not null; depth++)
		{
			buttons = container.FindAllDescendants(cf => cf.ByControlType(ControlType.Button));
			if (buttons.Length >= 2)
			{
				break;
			}

			container = container.Parent;
		}

		if (buttons.Length >= 2)
		{
			// Rightmost button is typically "+".
			var ordered = buttons.OrderBy(b => b.BoundingRectangle.X).ToArray();
			UiWait.ClickElement(ordered[^1]);
			Thread.Sleep(400);
			return;
		}

		// Fallback: click the right half of the stepper area to the right of the label.
		var bounds = label.BoundingRectangle;
		Mouse.Click(new Point(bounds.Right + 120, bounds.Y + Math.Max(8, bounds.Height / 2)));
		Thread.Sleep(400);
	}
}
