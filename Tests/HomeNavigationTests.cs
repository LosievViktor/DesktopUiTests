using MauiUkraine.UITests.Infrastructure;
using MauiUkraine.UITests.Pages;

namespace MauiUkraine.UITests.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public sealed class HomeNavigationTests : UiTestBase
{
	[Test]
	public void HomePage()
	{
		var home = new HomePage(MainWindow);
		home.WaitUntilLoaded();

		home.OpenAboutAndDismiss();

		Assert.That(home.IsLoaded(), Is.True, "Check that Home page is still loaded after dismissing About alert");
		Assert.That(home.AboutButton.IsEnabled, Is.True, "Check that About button is enabled after dismissing About alert");

		home.SelectEnglish();

		Assert.That(home.HeroTitle.Name, Does.Contain(EnglishTexts.Home.HeroTitle), "Check that hero title contains English app name after selecting English");
		Assert.That(home.HeroTitle, Is.Not.Null, "Check that hero title is displayed");
		Assert.That(home.AboutButton.IsEnabled, Is.True, "Check that About button is enabled");
		Assert.That(UiWait.Exists(MainWindow, AutomationIds.Home.MenuButtons), Is.True, "Check that Buttons menu card is present");
		Assert.That(home.HeroTitle.Name, Does.Contain(EnglishTexts.Home.HeroTitle), "Check that hero title shows English text");
		Assert.That(home.AboutButton.Name, Does.Contain(EnglishTexts.Home.AboutButton), "Check that About button shows English text");
		Assert.That(home.MenuSection.Name, Does.Contain(EnglishTexts.Home.MenuSection), "Check that menu section shows English text");
		Assert.That(home.LanguageEnglish, Is.Not.Null, "Check that English language radio button is present");
		Assert.That(home.LanguageUkrainian, Is.Not.Null, "Check that Ukrainian language radio button is present");
		Assert.That(home.HeroSubtitle, Is.Not.Null, "Check that hero subtitle is present");
		Assert.That(home.MenuButtons, Is.Not.Null, "Check that Buttons menu card is present");
		Assert.That(home.MenuSelection, Is.Not.Null, "Check that Selection menu card is present");
		Assert.That(home.MenuCollections, Is.Not.Null, "Check that Collections menu card is present");
		Assert.That(home.MenuIndicators, Is.Not.Null, "Check that Indicators menu card is present");
		Assert.That(home.MenuAlerts, Is.Not.Null, "Check that Alerts menu card is present");
	}
}
