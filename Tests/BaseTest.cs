using FlaUI.Core;
using FlaUI.UIA3;

namespace DesktopUiTests.Tests
{
    public class BaseTest
    {
        public Application App;
        public UIA3Automation Automation;

        [SetUp]
        public void SetUp()
        {
            App = Application.Launch("..\\..\\..\\Application\\DataBindingDemo.exe");

            Automation = new UIA3Automation();
        }

        [TearDown]
        public void TearDown()
        {
            Automation?.Dispose();
            App?.Close();
        }
    }
}
