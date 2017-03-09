using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetGUI;
using SS;

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

            Boolean wasTrue = stub.WasChanged;
            Assert.IsFalse(wasTrue);
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Spreadsheet data = new Spreadsheet();
            //data.SetContentsOfCell("A1", "hi");
            //data.SetContentsOfCell("A2", "hi");

            Stub stub = new Stub();
            Controller five = new Controller(stub);


            stub.FireSelectionEvent(0,0);
            stub.FireInitialContentChange("5");
            //Assert.IsTrue(stub.CellContents == "5");

            
            stub.FireSelectionEvent(0, 1);
            stub.FireInitialContentChange("5");

            stub.FireSelectionEvent(0, 2);
            stub.FireInitialContentChange("=A1+A2");

            stub.FireSelectionEvent(0, 0);
            stub.FireInitialContentChange("6");


            HashSet<string> set = new HashSet<string>();
            foreach (string s in stub.cellRecalc)
            {
                set.Add(s);

            }


            Assert.IsTrue(set.Contains("A1"));
            Assert.IsTrue(set.Contains("A2"));


        }




    }
}
