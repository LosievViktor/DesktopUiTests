using MauiUkraine.UITests.Infrastructure;
using MauiUkraine.UITests.Pages;

namespace MauiUkraine.UITests.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public sealed class HomeNavigationTests : UiTestBase
{
	[Test]
	public void HomePage_IsDisplayed_OnLaunch()
	{
		var home = new HomePage(MainWindow);
		home.WaitUntilLoaded();

		Assert.That(home.HeroTitle, Is.Not.Null);
		Assert.That(home.AboutButton.IsEnabled, Is.True);
		Assert.That(UiWait.Exists(MainWindow, AutomationIds.Home.MenuButtons), Is.True);
	}

	[Test]
	public void HomePage_OpenButtonsAndInputs_ViaHomeMenuButton()
	{
		var home = new HomePage(MainWindow);
		var page = home.OpenButtonsAndInputs();

		Assert.That(page.FilledButton.IsEnabled, Is.True);
		Assert.That(page.OutlineButton.IsEnabled, Is.True);
		Assert.That(page.NameEntry, Is.Not.Null);
		Assert.That(UiWait.Exists(MainWindow, AutomationIds.Home.HeroTitle, TimeSpan.FromSeconds(1)), Is.False);
	}

	[Test]
	public void HomePage_OpenSelectionControls_ViaHomeMenuButton()
	{
		var home = new HomePage(MainWindow);
		var page = home.OpenSelectionControls();

		Assert.That(page.NotificationsSwitch, Is.Not.Null);
		Assert.That(page.Checkbox, Is.Not.Null);
		Assert.That(page.PlatformPicker, Is.Not.Null);
	}

	[Test]
	public void HomePage_OpenCollections_ViaHomeMenuButton()
	{
		var home = new HomePage(MainWindow);
		var page = home.OpenCollections();

		Assert.That(page.NewTodoEntry, Is.Not.Null);
		Assert.That(page.AddButton.IsEnabled, Is.True);
	}

	[Test]
	public void HomePage_OpenIndicatorsAndProgress_ViaHomeMenuButton()
	{
		var home = new HomePage(MainWindow);
		var page = home.OpenIndicatorsAndProgress();

		Assert.That(page.ActivitySwitch, Is.Not.Null);
		Assert.That(page.SimulateButton.IsEnabled, Is.True);
	}

	[Test]
	public void HomePage_OpenAlertsAndDialogs_ViaHomeMenuButton()
	{
		var home = new HomePage(MainWindow);
		var page = home.OpenAlertsAndDialogs();

		Assert.That(page.DisplayAlertButton.IsEnabled, Is.True);
		Assert.That(page.OpenModalButton.IsEnabled, Is.True);
        
    }

    [Test]
    public void HomePage_OpenAlertsThenModal_ViaHomeMenuButton()
    {
        var home = new HomePage(MainWindow);       
        var alerts = home.OpenAlertsAndDialogs();
		var modal = alerts.OpenModal();

		Assert.That(modal.Heading, Is.Not.Null);
		Assert.That(modal.CloseButton.IsEnabled, Is.True);

		modal.Close();
		Assert.That(UiWait.Exists(MainWindow, AutomationIds.Alerts.DisplayAlert), Is.True);
	}
}
