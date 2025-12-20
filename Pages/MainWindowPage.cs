using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using FlaUI.UIA3;

namespace DesktopUiTests.Pages
{
    public class MainWindowPage
    {
        private readonly Window _window;
        private readonly UIA3Automation _automation;
        private readonly Application _app;

        public MainWindowPage(Application app, UIA3Automation automation)
        {
            _window = Retry.WhileNull(() => app.GetMainWindow(automation), timeout: TimeSpan.FromSeconds(5)).Result;
            _app = app;
            _automation = automation;
        }

        private AutomationElement? FindElement(string AutomationId)
        {

            return _window.FindFirstDescendant(cf => cf.ByAutomationId(AutomationId));
        }

        public CheckBox GroupCheckBox => FindElement(AutomationIds.GroupCheckbox).AsCheckBox();

        public CheckBox ShowOnlyCheckBox => FindElement(AutomationIds.ShowOnlyCheckbox).AsCheckBox();

        public CheckBox SortCategoryAndDateCheckBox => FindElement(AutomationIds.SortCategoryAndDateCheckBox)?.AsCheckBox();

        public Label DescriptionLabel => FindElement(AutomationIds.DescriptionLabel).AsLabel();
        public Label StartPriceLabel => FindElement(AutomationIds.StartPriceLabel).AsLabel();
        public Label StartDateLabel => FindElement(AutomationIds.StartDateLabel).AsLabel();
        public Label CategoryLabel => FindElement(AutomationIds.CategoryLabel).AsLabel();
        public Label OwnersNameLabel => FindElement(AutomationIds.OwnersNameLabel).AsLabel();
        public Label OwnersRatingLabel => FindElement(AutomationIds.OwnersRatingLabel).AsLabel();
        public Label MemberSinceLabel => FindElement(AutomationIds.MemberSinceLabel).AsLabel();

        public Button AddProductButton => FindElement(AutomationIds.AddProductButton).AsButton();

        public List<ListBoxItem> GetListItemsForSale() 
        {
            ListBox ListItemsForSale = FindElement(AutomationIds.ListItemsForSale).AsListBox();
            return  ListItemsForSale.Items.ToList();
        }

        public List<string> GetAllWindowsOfApplication() 
        {
            var windows = _app.GetAllTopLevelWindows(_automation);
            return windows.Select(x => x.Title).ToList();
        }
    }
}