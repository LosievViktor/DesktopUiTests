using FlaUI.Core.AutomationElements;
using MauiUkraine.UITests.Infrastructure;

namespace MauiUkraine.UITests.Pages;

public sealed class DemoModalPage : BasePage
{
	public DemoModalPage(Window window) : base(window)
	{
	}

	public void WaitUntilLoaded()
		=> WaitUntilDisplayed(AutomationIds.Modal.Heading);

	public AutomationElement Heading => ById(AutomationIds.Modal.Heading);
	public AutomationElement Body => ById(AutomationIds.Modal.Body);
	public AutomationElement CloseButton => ById(AutomationIds.Modal.CloseButton);

	public string HeadingText => GetName(AutomationIds.Modal.Heading);
	public string BodyText => GetName(AutomationIds.Modal.Body);

	public void Close()
		=> Click(AutomationIds.Modal.CloseButton);
}
