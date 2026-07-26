using MauiUkraine.UITests.Infrastructure;
using MauiUkraine.UITests.Pages;

namespace MauiUkraine.UITests.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public sealed class AlertsDialogsTests : UiTestBase
{
	private AlertsAndDialogsPage OpenPage()
		=> new HomePage(MainWindow).OpenAlertsAndDialogs();

	[Test]
	public void AlertsPage_LastResult_ShowsEnglishDefault()
	{
		var page = OpenPage();

		Assert.That(page.LastResult, Is.Not.Null, "Check that last result label is displayed");
		Assert.That(page.LastResultText, Does.Contain(EnglishTexts.Alerts.LastResultDefault), "Check that last result shows English default text");
	}

	[Test]
	public void AlertsPage_AllActionButtons_AreEnabledWithEnglishText()
	{
		var page = OpenPage();

		Assert.That(page.DisplayAlertButton.IsEnabled, Is.True, "Check that DisplayAlert button is enabled");
		Assert.That(page.DisplayAlertButton.Name, Does.Contain(EnglishTexts.Alerts.DisplayAlert), "Check that DisplayAlert button shows English text");

		Assert.That(page.DisplayActionSheetButton.IsEnabled, Is.True, "Check that DisplayActionSheet button is enabled");
		Assert.That(page.DisplayActionSheetButton.Name, Does.Contain(EnglishTexts.Alerts.DisplayActionSheet), "Check that DisplayActionSheet button shows English text");

		Assert.That(page.DisplayPromptButton.IsEnabled, Is.True, "Check that DisplayPromptAsync button is enabled");
		Assert.That(page.DisplayPromptButton.Name, Does.Contain(EnglishTexts.Alerts.DisplayPrompt), "Check that DisplayPromptAsync button shows English text");

		Assert.That(page.ShowToastButton.IsEnabled, Is.True, "Check that Show Toast button is enabled");
		Assert.That(page.ShowToastButton.Name, Does.Contain(EnglishTexts.Alerts.ShowToast), "Check that Show Toast button shows English text");

		Assert.That(page.ShowSnackbarButton.IsEnabled, Is.True, "Check that Show Snackbar button is enabled");
		Assert.That(page.ShowSnackbarButton.Name, Does.Contain(EnglishTexts.Alerts.ShowSnackbar), "Check that Show Snackbar button shows English text");

		Assert.That(page.OpenModalButton.IsEnabled, Is.True, "Check that Open modal page button is enabled");
		Assert.That(page.OpenModalButton.Name, Does.Contain(EnglishTexts.Alerts.OpenModal), "Check that Open modal page button shows English text");
	}

	[Test]
	public void AlertsPage_DisplayAlert_ConfirmDelete_UpdatesLastResult()
	{
		var page = OpenPage();

		page.ConfirmDisplayAlertDelete();

		Assert.That(page.LastResultText, Does.Contain("Delete"), "Check that last result reflects Delete choice");
	}

	[Test]
	public void AlertsPage_DisplayAlert_Cancel_UpdatesLastResult()
	{
		var page = OpenPage();

		page.CancelDisplayAlert();

		Assert.That(page.LastResultText, Does.Contain("Cancel"), "Check that last result reflects Cancel choice");
	}

	[Test]
	public void AlertsPage_ActionSheet_ChoosePriority_UpdatesLastResult()
	{
		var page = OpenPage();

		page.ChooseActionSheetPriority();

		Assert.That(page.LastResultText, Does.Contain(EnglishTexts.Alerts.ActionSheetResultPrefix), "Check that last result shows English action sheet result prefix");
		Assert.That(page.LastResultText, Does.Contain(EnglishTexts.Alerts.SortByPriority), "Check that last result contains Priority choice");
	}

	[Test]
	public void AlertsPage_Prompt_SubmitName_UpdatesLastResult()
	{
		var page = OpenPage();
		const string name = "Grace Hopper";

		page.SubmitPrompt(name);

		Assert.That(page.LastResultText, Does.Contain(EnglishTexts.Alerts.PromptHelloPrefix), "Check that last result shows English prompt hello prefix");
		Assert.That(page.LastResultText, Does.Contain(name), "Check that last result contains the prompted name");
	}

	[Test]
	public void AlertsPage_Toast_UpdatesLastResultInEnglish()
	{
		var page = OpenPage();

		page.ShowToast();

		Assert.That(page.LastResultText, Does.Contain(EnglishTexts.Alerts.ToastShown), "Check that last result shows English toast shown text");
	}

	[Test]
	public void AlertsPage_Snackbar_UpdatesLastResultInEnglish()
	{
		var page = OpenPage();

		page.ShowSnackbar();

		Assert.That(page.LastResultText, Does.Contain(EnglishTexts.Alerts.SnackbarShown), "Check that last result shows English snackbar shown text");
	}

	[Test]
	public void AlertsPage_Modal_OpensWithEnglishContentAndCloses()
	{
		var page = OpenPage();
		var modal = page.OpenModal();

		Assert.That(modal.HeadingText, Does.Contain(EnglishTexts.Modal.Heading), "Check that modal heading shows English text");
		Assert.That(modal.Body, Is.Not.Null, "Check that modal body is displayed");
		Assert.That(modal.CloseButton.Name, Does.Contain(EnglishTexts.Modal.Close), "Check that modal Close button shows English text");

		modal.Close();

		Assert.That(UiWait.Exists(MainWindow, AutomationIds.Alerts.DisplayAlert), Is.True, "Check that Alerts page is displayed after closing modal");
	}
}
