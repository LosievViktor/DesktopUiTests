using DesktopUiTests.Pages;

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

            Assert.That(mainWindow.DescriptionLabel.Text == "Inside C#, second edition", 
                "Check Description Label.");

            Assert.That(mainWindow.StartPriceLabel.Text == "10", 
                "Check Price Label.");

            Assert.That(mainWindow.StartDateLabel.Text == "29.05.2005",
                "Check Start Date Label.");

            Assert.That(mainWindow.CategoryLabel.Text == "Books", 
                "Check Category Label.");

            Assert.That(mainWindow.OwnersNameLabel.Text == "John",
                "Check Owners Name Label.");

            Assert.That(mainWindow.OwnersRatingLabel.Text == "12", 
                "Check Owners Rating Label.");

            Assert.That(mainWindow.MemberSinceLabel.Text == "20.04.2003", 
                "Check Member Since Label.");

            mainWindow.GetListItemsForSale()[1].Click();

            Assert.That(mainWindow.DescriptionLabel.Text == "My DVD Collection",
                "Check Description Label.");

            Assert.That(mainWindow.StartPriceLabel.Text == "5",
                "Check Price Label.");

            Assert.That(mainWindow.StartDateLabel.Text == "03.08.2005", 
                "Check Start Date Label.");

            Assert.That(mainWindow.CategoryLabel.Text == "DvDs", 
                "Check Category Label.");

            Assert.That(mainWindow.OwnersNameLabel.Text == "Mary", 
                "Check Owners Name Label.");
            
            Assert.That(mainWindow.OwnersRatingLabel.Text == "10", 
                "Check Owners Rating Label.");

            Assert.That(mainWindow.MemberSinceLabel.Text== "02.05.2000", 
                "Check Member Since Label.");

            mainWindow.AddProductButton.Click();

            Assert.That(mainWindow.GetAllWindowsOfApplication().Contains("Add Product Listing"),
                "Check that new Window appeared.");
        }
    }
}