using FlaUI.Core.AutomationElements;
using MauiUkraine.UITests.Infrastructure;

namespace MauiUkraine.UITests.Pages;

public sealed class SelectionControlsPage : BasePage
{
	public SelectionControlsPage(Window window) : base(window)
	{
	}

	public void WaitUntilLoaded()
		=> WaitUntilDisplayed(AutomationIds.Selection.NotificationsSwitch);

	public AutomationElement NotificationsSwitch => ById(AutomationIds.Selection.NotificationsSwitch);
	public AutomationElement Checkbox => ById(AutomationIds.Selection.Checkbox);
	public AutomationElement ColorRed => ById(AutomationIds.Selection.ColorRed);
	public AutomationElement ColorGreen => ById(AutomationIds.Selection.ColorGreen);
	public AutomationElement ColorBlue => ById(AutomationIds.Selection.ColorBlue);
	public AutomationElement SelectedLabel => ById(AutomationIds.Selection.SelectedLabel);
	public AutomationElement PlatformPicker => ById(AutomationIds.Selection.PlatformPicker);
	public AutomationElement PlatformLabel => ById(AutomationIds.Selection.PlatformLabel);
	public AutomationElement DatePicker => ById(AutomationIds.Selection.DatePicker);
	public AutomationElement TimePicker => ById(AutomationIds.Selection.TimePicker);
	public AutomationElement ScheduledLabel => ById(AutomationIds.Selection.ScheduledLabel);
	public AutomationElement TeamSizeStepper => ByIdScrolling(AutomationIds.Selection.TeamSizeStepper);
	public AutomationElement VolumeSlider => ByIdScrolling(AutomationIds.Selection.VolumeSlider);
	public AutomationElement VolumeLabel => ByIdScrolling(AutomationIds.Selection.VolumeLabel);

	public void ToggleNotifications()
		=> Toggle(AutomationIds.Selection.NotificationsSwitch);

	public bool AreNotificationsOn()
		=> IsOn(AutomationIds.Selection.NotificationsSwitch);

	public void ToggleCheckbox()
		=> Toggle(AutomationIds.Selection.Checkbox);

	public bool IsCheckboxChecked()
		=> IsOn(AutomationIds.Selection.Checkbox);

	public void SelectColorRed()
	{
		Select(AutomationIds.Selection.ColorRed);
		WaitForNameContains(AutomationIds.Selection.SelectedLabel, EnglishTexts.Selection.Red, TimeSpan.FromSeconds(5));
	}

	public void SelectColorBlue()
	{
		Select(AutomationIds.Selection.ColorBlue);
		WaitForNameContains(AutomationIds.Selection.SelectedLabel, EnglishTexts.Selection.Blue, TimeSpan.FromSeconds(5));
	}

	public void SelectPlatform(string platform)
	{
		UiActions.SelectComboItem(PlatformPicker, platform);
		WaitForNameContains(AutomationIds.Selection.PlatformLabel, platform, TimeSpan.FromSeconds(5));
	}

	public void IncrementTeamSize()
		// Prefer label-relative interaction; Stepper AutomationId is unreliable on WinUI.
		=> UiActions.IncrementTeamSizeFromLabel(Window);

	public void SetVolume(double value)
	{
		UiActions.SetRangeValue(VolumeSlider, value);
		Thread.Sleep(300);
	}

	public string SelectedColorText => GetName(AutomationIds.Selection.SelectedLabel);
	public string PlatformText => GetName(AutomationIds.Selection.PlatformLabel);
	public string ScheduledText => GetName(AutomationIds.Selection.ScheduledLabel);
	public string VolumeText => GetName(AutomationIds.Selection.VolumeLabel);
}
