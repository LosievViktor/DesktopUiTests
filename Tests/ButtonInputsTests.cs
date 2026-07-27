using MauiUkraine.UITests.Infrastructure;
using MauiUkraine.UITests.Pages;

namespace MauiUkraine.UITests.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public sealed class ButtonInputsTests : UiTestBase
{
	private ButtonsAndInputsPage OpenPage()
		=> new HomePage(MainWindow).OpenButtonsAndInputs();

	[Test]
	public void ButtonsPage_FilledButton_IsEnabledWithEnglishText()
	{
		var page = OpenPage();

		Assert.That(page.FilledButton.IsEnabled, Is.True, 
			"Check that Filled button is enabled.");

		Assert.That(page.FilledButton.Name, Does.Contain(EnglishTexts.Buttons.Filled), 
			"Check that Filled button shows English text.");
	}

	[Test]
	public void ButtonsPage_OutlineButton_IsEnabledWithEnglishText()
	{
		var page = OpenPage();

		Assert.That(page.OutlineButton.IsEnabled, Is.True, 
			"Check that Outline button is enabled.");

		Assert.That(page.OutlineButton.Name, Does.Contain(EnglishTexts.Buttons.Outline), 
			"Check that Outline button shows English text.");
	}

	[Test]
	public void ButtonsPage_DisabledButton_IsDisabledWithEnglishText()
	{
		var page = OpenPage();

		Assert.That(page.DisabledButton.IsEnabled, Is.False, 
			"Check that Disabled button is disabled.");

		Assert.That(page.DisabledButton.Name, Does.Contain(EnglishTexts.Buttons.Disabled),
			"Check that Disabled button shows English text.");
	}

	[Test]
	public void ButtonsPage_NameEntry_UpdatesHelloLabelInEnglish()
	{
		var page = OpenPage();
		const string name = "Ada Lovelace";

		page.EnterFullName(name);

		Assert.That(page.HelloText, Does.Contain(EnglishTexts.Buttons.HelloPrefix), 
			"Check that hello label shows English greeting prefix.");

		Assert.That(page.HelloText, Does.Contain(name), 
			"Check that hello label contains the entered name.");
	}

	[Test]
	public void ButtonsPage_PasswordEntry_AcceptsText()
	{
		var page = OpenPage();

		Assert.That(page.PasswordEntry.IsEnabled, Is.True, 
			"Check that Password entry is enabled.");

		Assert.DoesNotThrow(() => page.EnterPassword("s3cret!"), 
			"Check that Password entry accepts text input.");
	}

	[Test]
	public void ButtonsPage_NumericEntry_AcceptsDigits()
	{
		var page = OpenPage();

		page.EnterNumeric("42");

		Assert.That(UiActions.GetText(page.NumericEntry), Does.Contain("42"), 
			"Check that Numeric entry contains entered digits.");
	}

	[Test]
	public void ButtonsPage_HelloLabel_IsDisplayed()
	{
		var page = OpenPage();

		Assert.That(page.HelloLabel, Is.Not.Null,
			"Check that hello label is displayed.");

		Assert.That(page.HelloText, Does.Contain(EnglishTexts.Buttons.HelloPrefix), 
			"Check that hello label shows English greeting prefix.");
	}

	[Test]
	public void ButtonsPage_SearchBar_FiltersResults()
	{
		var page = OpenPage();

		Assert.That(page.SearchBar, Is.Not.Null, "Check that SearchBar is displayed.");

		page.Search("MAUI");

		UiWait.WaitUntil(() => page.HasSearchResultNamed(".NET MAUI"), TimeSpan.FromSeconds(5),
			"Expected filtered search result '.NET MAUI'.");

		Assert.That(page.HasSearchResultNamed("Blazor Hybrid"), Is.False, 
			"Check that unrelated search result is filtered out.");
	}

	[Test]
	public void ButtonsPage_SearchBar_UpdatesResultsForDifferentQueries()
	{
		var page = OpenPage();

		page.Search("Blazor");
		UiWait.WaitUntil(() => page.HasSearchResultNamed("Blazor Hybrid"), TimeSpan.FromSeconds(5),
			"Expected 'Blazor Hybrid' for query 'Blazor'.");

		page.Search("Shell");

		UiWait.WaitUntil(() => page.HasSearchResultNamed("Shell Navigation"), TimeSpan.FromSeconds(5),
			"Expected 'Shell Navigation' for query 'Shell'.");
		
		Assert.That(page.HasSearchResultNamed("Blazor Hybrid"), Is.False, 
			"Check that previous search result is no longer shown.");
	}
}
