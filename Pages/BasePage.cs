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

	protected AutomationElement ByIdScrolling(string automationId, TimeSpan? timeout = null)
		=> UiWait.WaitForAutomationIdScrolling(Window, automationId, timeout);

	protected void Click(string automationId, TimeSpan? timeout = null)
	{
		UiWait.ClickByAutomationId(Window, automationId, timeout);
		Thread.Sleep(300);
	}

	protected void SetText(string automationId, string text)
		=> UiActions.SetText(ById(automationId), text);

	protected string GetName(string automationId)
		=> UiActions.GetName(ById(automationId));

	protected string GetText(string automationId)
		=> UiActions.GetText(ById(automationId));

	protected void Toggle(string automationId)
		=> UiActions.Toggle(ById(automationId));

	protected bool IsOn(string automationId)
		=> UiActions.IsOn(ById(automationId));

	protected void Select(string automationId)
		=> UiActions.Select(ById(automationId));

	protected void WaitForNameContains(string automationId, string expectedSubstring, TimeSpan? timeout = null)
		=> UiWait.WaitForNameContains(Window, automationId, expectedSubstring, timeout);

	public bool IsDisplayed(string pageAutomationId, TimeSpan? timeout = null)
		=> UiWait.Exists(Window, pageAutomationId, timeout ?? AppConfig.DefaultTimeout);

	public void WaitUntilDisplayed(string pageAutomationId, TimeSpan? timeout = null)
		=> UiWait.WaitForAutomationId(Window, pageAutomationId, timeout);
}
