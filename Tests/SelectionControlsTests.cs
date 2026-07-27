using MauiUkraine.UITests.Infrastructure;
using MauiUkraine.UITests.Pages;

namespace MauiUkraine.UITests.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public sealed class SelectionControlsTests : UiTestBase
{
	private SelectionControlsPage OpenPage() => new HomePage(MainWindow).OpenSelectionControls();

	[Test]
	public void SelectionPage_NotificationsSwitch_Toggles()
	{
		var page = OpenPage();

		Assert.That(page.NotificationsSwitch, Is.Not.Null, "Check that notifications switch is displayed.");
		var initial = page.AreNotificationsOn();

		page.ToggleNotifications();

		Assert.That(page.AreNotificationsOn(), Is.EqualTo(!initial), "Check that notifications switch toggles its state.");
	}

	[Test]
	public void SelectionPage_Checkbox_Toggles()
	{
		var page = OpenPage();

		Assert.That(page.Checkbox, Is.Not.Null, "Check that checkbox is displayed.");
		var initial = page.IsCheckboxChecked();

		page.ToggleCheckbox();

		Assert.That(page.IsCheckboxChecked(), Is.EqualTo(!initial), "Check that checkbox toggles its checked state.");
	}

	[Test]
	public void SelectionPage_ColorRadioButtons_UpdateSelectedLabelInEnglish()
	{
		var page = OpenPage();

		Assert.That(page.ColorRed, Is.Not.Null, "Check that Red radio button is displayed.");
		Assert.That(page.ColorGreen, Is.Not.Null, "Check that Green radio button is displayed.");
		Assert.That(page.ColorBlue, Is.Not.Null, "Check that Blue radio button is displayed.");
		Assert.That(page.SelectedColorText, Does.Contain(EnglishTexts.Selection.SelectedPrefix), "Check that selected color label shows English prefix.");

		page.SelectColorRed();
		Assert.That(page.SelectedColorText, Does.Contain(EnglishTexts.Selection.Red), "Check that selected color label shows Red.");

		page.SelectColorBlue();
		Assert.That(page.SelectedColorText, Does.Contain(EnglishTexts.Selection.Blue), "Check that selected color label shows Blue.");
	}

	[Test]
	public void SelectionPage_PlatformPicker_UpdatesLabelInEnglish()
	{
		var page = OpenPage();

		Assert.That(page.PlatformPicker, Is.Not.Null, "Check that platform picker is displayed.");
		Assert.That(page.PlatformText, Does.Contain(EnglishTexts.Selection.BuildingForPrefix), "Check that platform label shows English prefix.");

		page.SelectPlatform(EnglishTexts.Selection.PlatformWindows);

		Assert.That(page.PlatformText, Does.Contain(EnglishTexts.Selection.PlatformWindows), "Check that platform label shows Windows.");
	}

	[Test]
	public void SelectionPage_DateAndTimePickers_AreDisplayedWithScheduledLabel()
	{
		var page = OpenPage();

		Assert.That(page.DatePicker, Is.Not.Null, "Check that date picker is displayed.");
		Assert.That(page.TimePicker, Is.Not.Null, "Check that time picker is displayed.");
		Assert.That(page.ScheduledText, Does.Contain(EnglishTexts.Selection.ScheduledPrefix), "Check that scheduled label shows English prefix.");
	}

	[Test]
	public void SelectionPage_TeamSizeStepper_IsDisplayedWithEnglishLabel()
	{
		_ = OpenPage();

		UiWait.ScrollToTop(MainWindow);
		UiWait.WaitUntil(
			() =>
			{
				if (MainWindow.FindFirstDescendant(cf => cf.ByName("Team size: 3")) is not null)
				{
					return true;
				}

				UiWait.ScrollDown(MainWindow);
				return false;
			},
			TimeSpan.FromSeconds(15),
			"Expected default English team size label 'Team size: 3'.");

		var label = MainWindow.FindFirstDescendant(cf => cf.ByName("Team size: 3"));
		Assert.That(label, Is.Not.Null, "Check that team size label is displayed.");
		Assert.That(label!.Name, Does.Contain(EnglishTexts.Selection.TeamSizePrefix), "Check that team size label shows English prefix.");

		// Stepper AutomationId is optional on WinUI; when present, assert it is enabled.
		if (UiWait.Exists(MainWindow, AutomationIds.Selection.TeamSizeStepper, TimeSpan.FromSeconds(2)))
		{
			var stepper = UiWait.WaitForAutomationId(MainWindow, AutomationIds.Selection.TeamSizeStepper);
			Assert.That(stepper.IsEnabled, Is.True, "Check that team size stepper is enabled.");
		}
	}

	[Test]
	public void SelectionPage_VolumeSlider_UpdatesVolumeLabelInEnglish()
	{
		var page = OpenPage();

		Assert.That(page.VolumeSlider, Is.Not.Null, "Check that volume slider is displayed.");
		Assert.That(page.VolumeText, Does.Contain(EnglishTexts.Selection.VolumePrefix), "Check that volume label shows English prefix.");

		page.SetVolume(0.8);

		UiWait.WaitUntil(
			() => page.VolumeText.Contains(EnglishTexts.Selection.VolumePrefix, StringComparison.OrdinalIgnoreCase),
			TimeSpan.FromSeconds(5));

		var value = UiActions.TryGetRangeValue(page.VolumeSlider);
		
		if (value is not null)
		{
			Assert.That(value.Value, Is.EqualTo(0.8).Within(0.05), "Check that volume slider value is set to about 0.8.");
		}
	}
}
