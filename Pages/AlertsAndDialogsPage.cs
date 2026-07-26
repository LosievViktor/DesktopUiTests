using FlaUI.Core.AutomationElements;
using MauiUkraine.UITests.Infrastructure;

namespace MauiUkraine.UITests.Pages;

public sealed class AlertsAndDialogsPage : BasePage
{
	public AlertsAndDialogsPage(Window window) : base(window)
	{
	}

	public void WaitUntilLoaded()
		=> WaitUntilDisplayed(AutomationIds.Alerts.DisplayAlert);

	public AutomationElement DisplayAlertButton => ById(AutomationIds.Alerts.DisplayAlert);
	public AutomationElement OpenModalButton => ById(AutomationIds.Alerts.OpenModal);

	public DemoModalPage OpenModal()
	{
		Click(AutomationIds.Alerts.OpenModal);
		var modal = new DemoModalPage(Window);
		modal.WaitUntilLoaded();
		return modal;
	}
}
