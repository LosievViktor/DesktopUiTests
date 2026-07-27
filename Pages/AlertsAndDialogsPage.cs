using FlaUI.Core.AutomationElements;
using MauiUkraine.UITests.Infrastructure;

namespace MauiUkraine.UITests.Pages;

public sealed class AlertsAndDialogsPage : BasePage
{
	public AlertsAndDialogsPage(Window window) : base(window)
	{
	}

	public void WaitUntilLoaded()
		=> WaitUntilDisplayed(AutomationIds.Alerts.DisplayAlert);

	public AutomationElement LastResult => ById(AutomationIds.Alerts.LastResult);
	public AutomationElement DisplayAlertButton => ById(AutomationIds.Alerts.DisplayAlert);
	public AutomationElement DisplayActionSheetButton => ById(AutomationIds.Alerts.DisplayActionSheet);
	public AutomationElement DisplayPromptButton => ById(AutomationIds.Alerts.DisplayPrompt);
	public AutomationElement ShowToastButton => ById(AutomationIds.Alerts.ShowToast);
	public AutomationElement ShowSnackbarButton => ById(AutomationIds.Alerts.ShowSnackbar);
	public AutomationElement OpenModalButton => ById(AutomationIds.Alerts.OpenModal);

	public string LastResultText => GetName(AutomationIds.Alerts.LastResult);

	public void ConfirmDisplayAlertDelete()
	{
		Click(AutomationIds.Alerts.DisplayAlert);
		UiActions.ClickDialogButton(Window, EnglishTexts.Alerts.Delete, TimeSpan.FromSeconds(10));
		WaitForNameContains(AutomationIds.Alerts.LastResult, "Delete", TimeSpan.FromSeconds(5));
	}

	public void CancelDisplayAlert()
	{
		Click(AutomationIds.Alerts.DisplayAlert);
		UiActions.ClickDialogButton(Window, EnglishTexts.Alerts.Cancel, TimeSpan.FromSeconds(10));
		WaitForNameContains(AutomationIds.Alerts.LastResult, "Cancel", TimeSpan.FromSeconds(5));
	}

	public void ChooseActionSheetPriority()
	{
		Click(AutomationIds.Alerts.DisplayActionSheet);

		// Confirm the English ActionSheet appeared, then pick an option.
		UiWait.WaitUntil(
			() =>
			{
				var desktop = Window.Automation.GetDesktop();
				return Window.FindFirstDescendant(cf => cf.ByName("Sort tasks by...")) is not null
					|| desktop.FindFirstDescendant(cf => cf.ByName("Sort tasks by...")) is not null
					|| Window.FindFirstDescendant(cf => cf.ByName(EnglishTexts.Alerts.SortByPriority)) is not null
					|| desktop.FindFirstDescendant(cf => cf.ByName(EnglishTexts.Alerts.SortByPriority)) is not null
					|| Window.FindFirstDescendant(cf => cf.ByName(EnglishTexts.Alerts.Cancel)) is not null;
			},
			TimeSpan.FromSeconds(10),
			"ActionSheet dialog did not appear.");

		if (!UiActions.TryClickDialogButton(Window, EnglishTexts.Alerts.SortByPriority, TimeSpan.FromSeconds(3))
			&& !UiActions.TryClickDialogButton(Window, EnglishTexts.Alerts.Cancel, TimeSpan.FromSeconds(3)))
		{
			Assert.Fail("Check that ActionSheet options Priority or Cancel can be interacted with.");
		}

		WaitForNameContains(
			AutomationIds.Alerts.LastResult,
			EnglishTexts.Alerts.ActionSheetResultPrefix,
			TimeSpan.FromSeconds(8));
	}

	public void SubmitPrompt(string name)
	{
		Click(AutomationIds.Alerts.DisplayPrompt);
		UiWait.WaitUntil(
			() => Window.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit)) is not null,
			TimeSpan.FromSeconds(10),
			"Prompt edit field did not appear.");

		var edit = Window.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit));
		Assert.That(edit, Is.Not.Null, "Check that prompt edit field is displayed.");
		UiActions.SetText(edit!, name);
		UiActions.ClickDialogButton(Window, EnglishTexts.Alerts.PromptOk, TimeSpan.FromSeconds(10));
		WaitForNameContains(AutomationIds.Alerts.LastResult, name, TimeSpan.FromSeconds(5));
	}

	public void ShowToast()
	{
		Click(AutomationIds.Alerts.ShowToast);
		WaitForNameContains(
			AutomationIds.Alerts.LastResult,
			"Toast shown",
			TimeSpan.FromSeconds(5));
	}

	public void ShowSnackbar()
	{
		Click(AutomationIds.Alerts.ShowSnackbar);
		WaitForNameContains(
			AutomationIds.Alerts.LastResult,
			"Snackbar shown",
			TimeSpan.FromSeconds(5));
	}

	public DemoModalPage OpenModal()
	{
		Click(AutomationIds.Alerts.OpenModal);
		var modal = new DemoModalPage(Window);
		modal.WaitUntilLoaded();
		return modal;
	}
}
