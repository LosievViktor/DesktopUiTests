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

	public AutomationElement ActivitySwitch => ById(AutomationIds.Indicators.ActivitySwitch);
	public AutomationElement SimulateButton => ById(AutomationIds.Indicators.SimulateButton);
}
