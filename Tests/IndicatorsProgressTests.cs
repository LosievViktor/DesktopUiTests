using MauiUkraine.UITests.Infrastructure;
using MauiUkraine.UITests.Pages;

namespace MauiUkraine.UITests.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public sealed class IndicatorsProgressTests : UiTestBase
{
	private IndicatorsAndProgressPage OpenPage()
		=> new HomePage(MainWindow).OpenIndicatorsAndProgress();

	[Test]
	public void IndicatorsPage_ActivityIndicator_IsDisplayed()
	{
		var page = OpenPage();

		Assert.That(page.ActivityIndicator, Is.Not.Null, "Check that activity indicator is displayed.");
	}

	[Test]
	public void IndicatorsPage_ActivitySwitch_Toggles()
	{
		var page = OpenPage();

		Assert.That(page.ActivitySwitch, Is.Not.Null, "Check that activity switch is displayed.");
		var initial = page.IsActivityEnabled();

		page.ToggleActivity();

		Assert.That(page.IsActivityEnabled(), Is.EqualTo(!initial), "Check that activity switch toggles its state.");
	}

	[Test]
	public void IndicatorsPage_ProgressBarAndLabel_AreDisplayedInEnglish()
	{
		var page = OpenPage();

		Assert.That(page.ProgressBar, Is.Not.Null, "Check that progress bar is displayed.");
		Assert.That(page.ProgressLabel, Is.Not.Null, "Check that progress label is displayed.");
		Assert.That(page.ProgressText, Does.Contain(EnglishTexts.Indicators.ProgressPrefix), "Check that progress label shows English prefix.");
	}

	[Test]
	public void IndicatorsPage_SimulateButton_RunsUploadToCompletion()
	{
		var page = OpenPage();

		Assert.That(page.SimulateButton.IsEnabled, Is.True, "Check that Simulate upload button is enabled.");
		Assert.That(page.SimulateButton.Name, Does.Contain(EnglishTexts.Indicators.Simulate), "Check that Simulate upload button shows English text.");

		page.SimulateUploadAndWaitForCompletion();

		Assert.That(page.ProgressText, Does.Contain("100%"), "Check that progress label shows 100% after upload completes.");
		Assert.That(page.SimulateButton.IsEnabled, Is.True, "Check that Simulate upload button is re-enabled after upload completes.");
	}
}
