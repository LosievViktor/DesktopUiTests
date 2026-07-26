using FlaUI.Core.AutomationElements;
using MauiUkraine.UITests.Infrastructure;

namespace MauiUkraine.UITests.Pages;

public abstract class BasePage
{
	protected BasePage(Window window)
	{
		Window = window;
	}

	protected Window Window { get; }

	protected AutomationElement ById(string automationId, TimeSpan? timeout = null)
		=> UiWait.WaitForAutomationId(Window, automationId, timeout);

	protected void Click(string automationId, TimeSpan? timeout = null)
	{
		UiWait.ClickByAutomationId(Window, automationId, timeout);
		Thread.Sleep(300);
	}

	public bool IsDisplayed(string pageAutomationId, TimeSpan? timeout = null)
		=> UiWait.Exists(Window, pageAutomationId, timeout ?? AppConfig.DefaultTimeout);

	public void WaitUntilDisplayed(string pageAutomationId, TimeSpan? timeout = null)
		=> UiWait.WaitForAutomationId(Window, pageAutomationId, timeout);
}
