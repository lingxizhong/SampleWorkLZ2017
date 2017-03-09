using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    [TestClass]
    public class ControllerTesting
    {
        [TestMethod]
        public void TestMethod1()
        {
            SpreadsheetGUI.Controller five = new SpreadsheetGUI.Controller();
        }
    }
}
