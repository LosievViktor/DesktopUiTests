using MauiUkraine.UITests.Infrastructure;
using MauiUkraine.UITests.Pages;

namespace MauiUkraine.UITests.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public sealed class CollectionTests : UiTestBase
{
	private CollectionsPage OpenPage()
		=> new HomePage(MainWindow).OpenCollections();

	[Test]
	public void CollectionsPage_NewTodoEntry_IsDisplayed()
	{
		var page = OpenPage();

		Assert.That(page.NewTodoEntry, Is.Not.Null, 
			"Check that new todo entry is displayed.");
	}

	[Test]
	public void CollectionsPage_AddButton_IsEnabledWithEnglishText()
	{
		var page = OpenPage();

		Assert.That(page.AddButton.IsEnabled, Is.True, 
			"Check that Add button is enabled.");

		Assert.That(page.AddButton.Name, Does.Contain(EnglishTexts.Collections.Add), 
			"Check that Add button shows English text.");
	}

	[Test]
	public void CollectionsPage_RefreshView_IsDisplayed()
	{
		var page = OpenPage();

		// RefreshView AutomationId is not always exposed by WinUI; TodoList is nested inside it.
		Assert.That(() => page.TodoList, Throws.Nothing,
			"Check that collections list inside RefreshView is available.");

		Assert.That(page.TodoList, Is.Not.Null, "Check that todo list is displayed.");
	}

	[Test]
	public void CollectionsPage_TodoList_ShowsSeededEnglishItems()
	{
		var page = OpenPage();

		Assert.That(page.TodoList, Is.Not.Null, "Check that todo list is displayed.");

		Assert.That(page.HasTodoWithTitle(EnglishTexts.Collections.Todo1), Is.True, 
			"Check that seeded English todo item is present.");

		Assert.That(page.GetTodoDoneCheckboxes().Length, Is.GreaterThanOrEqualTo(1), 
			"Check that at least one todo done checkbox is present.");
	}

	[Test]
	public void CollectionsPage_AddTodo_InsertsNewItem()
	{
		var page = OpenPage();
		var title = $"UI test todo {Guid.NewGuid():N}"[..24];

		page.AddTodo(title);

		Assert.That(page.HasTodoWithTitle(title), Is.True, 
			"Check that newly added todo item is present in the list.");

		Assert.That(page.IsEmptyLabelVisible(), Is.False, 
			"Check that empty list label is not visible after adding a todo.");
	}

	[Test]
	public void CollectionsPage_TodoDoneCheckbox_CanToggle()
	{
		var page = OpenPage();
		var checkboxes = page.GetTodoDoneCheckboxes();

		Assert.That(checkboxes.Length, Is.GreaterThan(0), 
			"Check that at least one todo done checkbox is present.");

		var initial = UiActions.IsOn(checkboxes[0]);

		page.ToggleFirstTodoDone();

		var after = UiActions.IsOn(page.GetTodoDoneCheckboxes()[0]);

		Assert.That(after, Is.EqualTo(!initial), 
			"Check that todo done checkbox toggles its state.");
	}
}
