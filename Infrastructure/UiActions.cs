using System.Drawing;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;

namespace MauiUkraine.UITests.Infrastructure;

public static class UiActions
{
    private const int UiDelayMs = 200;
    private const int ComboDelayMs = 300;
    private const int DialogDelayMs = 400;

    public static string GetName(AutomationElement element) => element.Name ?? string.Empty;

    public static void SetText(AutomationElement element, string text)
    {
        element.Focus();
        Click(element);

        // Clear existing value.
        Keyboard.TypeSimultaneously(VirtualKeyShort.CONTROL, VirtualKeyShort.KEY_A);
        Keyboard.Type(VirtualKeyShort.DELETE);
        WaitForUi(100);

        var textBox = element.AsTextBox();

        if (textBox != null)
        {
            textBox.Text = text;
            WaitForUi();
            return;
        }

        if (element.Patterns.Value.IsSupported)
        {
            element.Patterns.Value.Pattern.SetValue(text);
            WaitForUi();
            return;
        }

        Keyboard.Type(text);
        WaitForUi();
    }

    public static string GetText(AutomationElement element)
    {
        var textBox = element.AsTextBox();

        if (textBox != null && !string.IsNullOrEmpty(textBox.Text))
        {
            return textBox.Text;
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
            WaitForUi();
            return;
        }

        Click(element);
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
            WaitForUi();
            return;
        }

        Click(element);
    }

    public static void SetRangeValue(AutomationElement element, double value)
    {
        if (!element.Patterns.RangeValue.IsSupported)
        {
            throw new InvalidOperationException(
                $"Element '{element.AutomationId}' does not support RangeValue pattern.");
        }

        element.Patterns.RangeValue.Pattern.SetValue(value);
        WaitForUi();
    }

    public static double? TryGetRangeValue(AutomationElement element)
    {
        return element.Patterns.RangeValue.IsSupported
            ? element.Patterns.RangeValue.Pattern.Value
            : null;
    }

    public static void SelectComboItem(AutomationElement picker, string itemName)
    {
        var combo = picker.AsComboBox();

        if (combo != null)
        {
            try
            {
                combo.Select(itemName);
                WaitForUi(ComboDelayMs);
                return;
            }
            catch
            {
                combo.Expand();
                WaitForUi(ComboDelayMs);
            }
        }
        else
        {
            Click(picker);
            WaitForUi(ComboDelayMs);
        }

        var searchRoot = picker.Parent ?? picker;

        var item = UiWait.WaitForName(
            searchRoot,
            itemName,
            ControlType.ListItem,
            TimeSpan.FromSeconds(5));

        Click(item);
        WaitForUi(ComboDelayMs);
    }

    public static void ClickDialogButton(Window window, string buttonName, TimeSpan? timeout = null)
    {
        var deadline = DateTime.UtcNow + (timeout ?? AppConfig.DefaultTimeout);
        var desktop = window.Automation.GetDesktop();

        while (DateTime.UtcNow < deadline)
        {
            if (TryClick(window, buttonName) ||
                TryClick(desktop, buttonName))
            {
                WaitForUi(DialogDelayMs);
                return;
            }

            Thread.Sleep(AppConfig.PollInterval);
        }

        throw new TimeoutException(
            $"Dialog option '{buttonName}' was not found.");
    }

    public static bool TryClickDialogButton(
        Window window,
        string buttonName,
        TimeSpan? timeout = null)
    {
        try
        {
            ClickDialogButton(window, buttonName, timeout);
            return true;
        }
        catch (TimeoutException)
        {
            return false;
        }
    }

    public static void IncrementStepper(AutomationElement stepper)
    {
        var buttons = stepper.FindAllDescendants(
            cf => cf.ByControlType(ControlType.Button));

        if (buttons.Length == 0)
        {
            buttons = stepper.Parent?.FindAllDescendants(
                cf => cf.ByControlType(ControlType.Button))
                ?? Array.Empty<AutomationElement>();
        }

        switch (buttons.Length)
        {
            case >= 2:
                Click(buttons[^1]);
                return;

            case 1:
                Click(buttons[0]);
                return;
        }

        var bounds = stepper.BoundingRectangle;

        if (bounds.Width <= 0 || bounds.Height <= 0)
        {
            throw new InvalidOperationException(
                "Could not locate stepper increment control.");
        }

        Mouse.Click(new Point(
            bounds.Right - 12,
            bounds.Y + bounds.Height / 2));

        WaitForUi();
    }

    public static void IncrementTeamSizeFromLabel(Window window)
    {
        AutomationElement? label = null;

        foreach (var name in new[]
                 {
                     "Team size: 3",
                     "Team size: 2"
                 })
        {
            label = window.FindFirstDescendant(cf => cf.ByName(name));

            if (label != null)
                break;
        }

        label ??= UiWait.WaitForName(
            window,
            "Team size: 3",
            timeout: TimeSpan.FromSeconds(10));

        UiWait.EnsureOnScreen(window, label);

        AutomationElement? container = label;
        AutomationElement[] buttons = Array.Empty<AutomationElement>();

        while (container != null)
        {
            buttons = container.FindAllDescendants(
                cf => cf.ByControlType(ControlType.Button));

            if (buttons.Length >= 2)
                break;

            container = container.Parent;
        }

        if (buttons.Length >= 2)
        {
            var plusButton = buttons
                .OrderBy(b => b.BoundingRectangle.X)
                .Last();

            Click(plusButton);
            WaitForUi(DialogDelayMs);
            return;
        }

        var bounds = label.BoundingRectangle;

        Mouse.Click(new Point(
            bounds.Right + 120,
            bounds.Y + Math.Max(8, bounds.Height / 2)));

        WaitForUi(DialogDelayMs);
    }

    private static bool TryClick(AutomationElement root,string name)
    {
        var element =
            root.FindFirstDescendant(cf =>
                cf.ByName(name)
                  .And(cf.ByControlType(ControlType.Button)))
            ??
            root.FindFirstDescendant(cf =>
                cf.ByName(name)
                  .And(cf.ByControlType(ControlType.ListItem)))
            ??
            root.FindFirstDescendant(cf =>
                cf.ByName(name));

        if (element == null)
            return false;

        Click(element);
        return true;
    }

    private static void Click(AutomationElement element)
    {
        UiWait.ClickElement(element);
        WaitForUi();
    }

    private static void WaitForUi(int milliseconds = UiDelayMs)
    {
        Thread.Sleep(milliseconds);
    }
}