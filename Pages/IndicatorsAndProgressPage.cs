using FlaUI.Core.AutomationElements;
using MauiUkraine.UITests.Infrastructure;

namespace MauiUkraine.UITests.Pages;

public sealed class IndicatorsAndProgressPage : BasePage
{
	public IndicatorsAndProgressPage(Window window) : base(window)
	{
	}

	public void WaitUntilLoaded()
		=> WaitUntilDisplayed(AutomationIds.Indicators.ActivitySwitch);

	public AutomationElement ActivityIndicator => ById(AutomationIds.Indicators.Activity);
	public AutomationElement ActivitySwitch => ById(AutomationIds.Indicators.ActivitySwitch);
	public AutomationElement ProgressBar => ById(AutomationIds.Indicators.ProgressBar);
	public AutomationElement ProgressLabel => ById(AutomationIds.Indicators.ProgressLabel);
	public AutomationElement SimulateButton => ById(AutomationIds.Indicators.SimulateButton);

	public void ToggleActivity()
		=> Toggle(AutomationIds.Indicators.ActivitySwitch);

	public bool IsActivityEnabled()
		=> IsOn(AutomationIds.Indicators.ActivitySwitch);

	public void SimulateUploadAndWaitForCompletion()
	{
		Click(AutomationIds.Indicators.SimulateButton);
		WaitForNameContains(AutomationIds.Indicators.ProgressLabel, "100%", TimeSpan.FromSeconds(20));
		UiWait.WaitUntil(
			() => SimulateButton.IsEnabled,
			TimeSpan.FromSeconds(10),
			"Simulate button did not re-enable after upload.");
	}

	public string ProgressText => GetName(AutomationIds.Indicators.ProgressLabel);
}
