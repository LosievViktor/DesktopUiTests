using DesktopUiTests.Pages;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Tools;
using FlaUI.UIA3;


namespace DesktopUiTests.Tests
{
    [TestFixture]
    public class DesktopTests:BaseTest
    {
        [Test]
        public void DesktopTest() 
        {
            var mainWindow = new MainWindowPage(App, Automation);

            mainWindow.GroupCheckBox.Click();
            mainWindow.ShowOnlyCheckBox.Click();
            mainWindow.SortCategoryAndDateCheckBox.Click();


            Assert.That(mainWindow.DescriptionLabel.Text == "Inside C#, second edition");
            Assert.That(mainWindow.StartPriceLabel.Text == "10");
            Assert.That(mainWindow.StartDateLabel.Text == "29.05.2005");
            Assert.That(mainWindow.CategoryLabel.Text == "Books");
            Assert.That(mainWindow.OwnersNameLabel.Text == "John");
            Assert.That(mainWindow.OwnersRatingLabel.Text == "12");
            Assert.That(mainWindow.MemberSinceLabel.Text == "20.04.2003");

            mainWindow.GetListItemsForSale()[1].Click();

            Assert.That(mainWindow.DescriptionLabel.Text == "My DVD Collection");
            Assert.That(mainWindow.StartPriceLabel.Text == "5");
            Assert.That(mainWindow.StartDateLabel.Text == "03.08.2005");
            Assert.That(mainWindow.CategoryLabel.Text == "DvDs");
            Assert.That(mainWindow.OwnersNameLabel.Text == "Mary");
            Assert.That(mainWindow.OwnersRatingLabel.Text == "10");
            Assert.That(mainWindow.MemberSinceLabel.Text== "02.05.2000");

            mainWindow.AddProductButton.Click();
        }
    }
}