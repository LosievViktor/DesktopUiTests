using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace DesktopUiTests
{
    public class Tests
    {
        private Application _app;

        [SetUp]
        public void Setup()
        {
             _app = Application.Launch("C:\\repository\\DesktopUiTests\\Application\\DataBindingDemo.exe");

        }

        [Test]
        public void Test()
        {
            using (var automation = new UIA3Automation())
            {
                var window = _app.GetMainWindow(automation);
                var button1 = window.FindFirstDescendant(cf => cf.ByText("1"))?.AsButton();
                button1?.Invoke();
            }           
        }

        [TearDown]
        public void TearDown()
        {
            _app.Dispose();
            _app.Kill();

        }
    }
}
