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
	public AutomationElement PlatformPicker => ById(AutomationIds.Selection.PlatformPicker);
}
