using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetGUI;

namespace ControllerTester
{
    [TestClass]
    public class ControllerTesting 
    {
        [TestMethod]
        public void TestMethod1()
        {
            Stub test = new Stub();

            Controller five = new Controller(test);
        }





       
    }
}
