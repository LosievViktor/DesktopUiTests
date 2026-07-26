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
	public AutomationElement DisabledButton => ById(AutomationIds.Buttons.DisabledButton);
	public AutomationElement NameEntry => ById(AutomationIds.Buttons.NameEntry);
	public AutomationElement PasswordEntry => ById(AutomationIds.Buttons.PasswordEntry);
	public AutomationElement NumericEntry => ById(AutomationIds.Buttons.NumericEntry);
	public AutomationElement HelloLabel => ById(AutomationIds.Buttons.HelloLabel);
	public AutomationElement SearchBar => ByIdScrolling(AutomationIds.Buttons.SearchBar);

	public void EnterFullName(string name)
	{
		SetText(AutomationIds.Buttons.NameEntry, name);
		WaitForNameContains(AutomationIds.Buttons.HelloLabel, name, TimeSpan.FromSeconds(5));
	}

	public void EnterPassword(string password)
		=> SetText(AutomationIds.Buttons.PasswordEntry, password);

	public void EnterNumeric(string value)
		=> SetText(AutomationIds.Buttons.NumericEntry, value);

	public void Search(string query)
	{
		var searchBar = SearchBar;
		UiActions.SetText(searchBar, query);
		Thread.Sleep(500);
	}

	public bool HasSearchResultNamed(string name)
		=> Window.FindFirstDescendant(cf => cf.ByName(name)) is not null;

	public string HelloText => GetName(AutomationIds.Buttons.HelloLabel);
}
