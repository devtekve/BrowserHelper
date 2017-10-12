using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;

namespace BrowserHelper.Tests
{
    [TestClass()]
    public class BrowserHelperTests
    {
        [TestMethod()]
        public void SetElementByIDTest()
        {
            var helper = new BrowserHelper(new WebBrowser());
            helper.SetElementByID(null, null, null);
        }
    }
}