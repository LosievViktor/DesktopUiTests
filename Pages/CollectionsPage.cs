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
	public AutomationElement Refresh => ByIdScrolling(AutomationIds.Collections.Refresh);
	public AutomationElement TodoList => ByIdScrolling(AutomationIds.Collections.TodoList);

	public void AddTodo(string title)
	{
		SetText(AutomationIds.Collections.NewTodoEntry, title);
		Click(AutomationIds.Collections.AddButton);
		UiWait.WaitUntil(
			() => Window.FindFirstDescendant(cf => cf.ByName(title)) is not null,
			TimeSpan.FromSeconds(5),
			$"Todo '{title}' did not appear in the list.");
	}

	public bool HasTodoWithTitle(string title)
		=> Window.FindFirstDescendant(cf => cf.ByName(title)) is not null;

	public AutomationElement[] GetTodoItems()
		=> Window.FindAllDescendants(cf => cf.ByAutomationId(AutomationIds.Collections.TodoItem));

	public AutomationElement[] GetTodoDoneCheckboxes()
		=> Window.FindAllDescendants(cf => cf.ByAutomationId(AutomationIds.Collections.TodoDone));

	public void ToggleFirstTodoDone()
	{
		var checkboxes = GetTodoDoneCheckboxes();
		Assert.That(checkboxes.Length, Is.GreaterThan(0), "Check that at least one todo done checkbox is present.");
		UiActions.Toggle(checkboxes[0]);
	}

	public bool IsEmptyLabelVisible()
		=> UiWait.Exists(Window, AutomationIds.Collections.EmptyLabel, TimeSpan.FromSeconds(1));
}
