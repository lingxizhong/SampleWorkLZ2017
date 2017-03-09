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
            Stub stub = new Stub();
            Controller five = new Controller(stub);

            stub.FireOpenEvent(@"C:\Users\Osama\Google Drive\Visual Studio 2015\GitRepo\SOL\Spreadsheet\ControllerTester\Spreadsheet.ss");


        }





       
    }
}
