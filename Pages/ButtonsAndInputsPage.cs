using FlaUI.Core.AutomationElements;
using MauiUkraine.UITests.Infrastructure;

namespace MauiUkraine.UITests.Pages;

public sealed class ButtonsAndInputsPage : BasePage
{
	public ButtonsAndInputsPage(Window window) : base(window)
	{
	}

	public void WaitUntilLoaded()
		=> WaitUntilDisplayed(AutomationIds.Buttons.FilledButton);

	public AutomationElement FilledButton => ById(AutomationIds.Buttons.FilledButton);
	public AutomationElement OutlineButton => ById(AutomationIds.Buttons.OutlineButton);
	public AutomationElement NameEntry => ById(AutomationIds.Buttons.NameEntry);
}
