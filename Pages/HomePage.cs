using FlaUI.Core.AutomationElements;
using MauiUkraine.UITests.Infrastructure;

namespace MauiUkraine.UITests.Pages;

public sealed class HomePage : BasePage
{
	public HomePage(Window window) : base(window)
	{
	}

	public void WaitUntilLoaded()
		=> WaitUntilDisplayed(AutomationIds.Home.HeroTitle);

	public bool IsLoaded()
		=> IsDisplayed(AutomationIds.Home.HeroTitle, TimeSpan.FromSeconds(2));

	public AutomationElement HeroTitle => ById(AutomationIds.Home.HeroTitle);
	public AutomationElement AboutButton => ById(AutomationIds.Home.AboutButton);

	public ButtonsAndInputsPage OpenButtonsAndInputs()
		=> OpenFeature(AutomationIds.Home.MenuButtons, AutomationIds.Buttons.FilledButton, () => new ButtonsAndInputsPage(Window));

	public SelectionControlsPage OpenSelectionControls()
		=> OpenFeature(AutomationIds.Home.MenuSelection, AutomationIds.Selection.NotificationsSwitch, () => new SelectionControlsPage(Window));

	public CollectionsPage OpenCollections()
		=> OpenFeature(AutomationIds.Home.MenuCollections, AutomationIds.Collections.NewTodoEntry, () => new CollectionsPage(Window));

	public IndicatorsAndProgressPage OpenIndicatorsAndProgress()
		=> OpenFeature(AutomationIds.Home.MenuIndicators, AutomationIds.Indicators.ActivitySwitch, () => new IndicatorsAndProgressPage(Window));

	public AlertsAndDialogsPage OpenAlertsAndDialogs()
		=> OpenFeature(AutomationIds.Home.MenuAlerts, AutomationIds.Alerts.DisplayAlert, () => new AlertsAndDialogsPage(Window));

	private TPage OpenFeature<TPage>(string menuAutomationId, string destinationAutomationId, Func<TPage> factory)
		where TPage : BasePage
	{
		const int maxAttempts = 3;
		Exception? lastError = null;

		for (var attempt = 1; attempt <= maxAttempts; attempt++)
		{
			Click(menuAutomationId);

			try
			{
				UiWait.WaitForAutomationId(Window, destinationAutomationId, TimeSpan.FromSeconds(10));
				return factory();
			}
			catch (Exception ex) when (attempt < maxAttempts)
			{
				lastError = ex;
				// Still on Home — retry the Home card click.
				if (!UiWait.Exists(Window, AutomationIds.Home.HeroTitle, TimeSpan.FromSeconds(1)))
				{
					throw;
				}
			}
		}

		throw new TimeoutException(
			$"Failed to open destination '{destinationAutomationId}' via '{menuAutomationId}' after {maxAttempts} attempts.",
			lastError);
	}
}
