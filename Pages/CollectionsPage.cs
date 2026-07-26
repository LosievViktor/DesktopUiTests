using FlaUI.Core.AutomationElements;
using MauiUkraine.UITests.Infrastructure;

namespace MauiUkraine.UITests.Pages;

public sealed class CollectionsPage : BasePage
{
	public CollectionsPage(Window window) : base(window)
	{
	}

	public void WaitUntilLoaded()
		=> WaitUntilDisplayed(AutomationIds.Collections.NewTodoEntry);

	public AutomationElement NewTodoEntry => ById(AutomationIds.Collections.NewTodoEntry);
	public AutomationElement AddButton => ById(AutomationIds.Collections.AddButton);
}
