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
        public void TestOpenFile()
        {
            Stub stub = new Stub();
            Controller five = new Controller(stub);

            stub.FireOpenEvent(@"C:\Users\Osama\Google Drive\Visual Studio 2015\GitRepo\SOL\Spreadsheet\ControllerTester\Spreadsheet.ss");

            Boolean wasTrue = stub.WasChanged;
            Assert.IsFalse(wasTrue);
        }

        [TestMethod]
        public void TestCellSelectoinAndContent()
        {
            //Spreadsheet data = new Spreadsheet();
            //data.SetContentsOfCell("A1", "hi");
            //data.SetContentsOfCell("A2", "hi");

            Stub stub = new Stub();
            Controller five = new Controller(stub);


            stub.FireSelectionEvent(0, 0);
            stub.FireInitialContentChange("5");


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
            Assert.IsTrue(set.Contains("A3"));


        }


        [TestMethod]
        public void TestCloseEvent()
        {
            Stub stub = new Stub();
            Controller five = new Controller(stub);

            stub.FireWasSomethingChangedInDataEvent();
            Boolean wasTrue = stub.WasChanged;
            Assert.IsFalse(wasTrue);

            stub.FireSelectionEvent(0, 0);
            stub.FireInitialContentChange("5");
            stub.FireWasSomethingChangedInDataEvent();

            wasTrue = stub.WasChanged;
            Assert.IsTrue(wasTrue);

        }





        [TestMethod]
        public void TestFormulaSelected()
        {
            Stub stub = new Stub();
            Controller five = new Controller(stub);

            stub.FireSelectionEvent(0, 0);
            stub.FireInitialContentChange("=A2");
            stub.FireSelectionEvent(0, 0);

            Assert.AreEqual(stub.CellValue, "SS.FormulaError");


        }

        


        [TestMethod]
        public void TestCellRecalcEvent()
        {
            Stub stub = new Stub();
            Controller five = new Controller(stub);

            stub.FireSelectionEvent(0, 0);
            stub.FireInitialContentChange("4");

            stub.FireCellRecalcEvent("A1");

            Assert.AreEqual("4",stub.CellValue);
        }



        [TestMethod]
        public void TestSaveFileEvent()
        {
            Stub stub = new Stub();
            Controller five = new Controller(stub);

            stub.FireSelectionEvent(0, 0);
            stub.FireInitialContentChange("4");

            stub.FireSaveEvent("testSheet.ss");



        }

        [TestMethod]
        public void TestOpenFileError()
        {
            Stub stub = new Stub();
            Controller five = new Controller(stub);

            stub.FireSelectionEvent(0, 0);
            stub.FireInitialContentChange("4");

            stub.FireOpenEvent(@"C:\t");

            var e = stub.errorProperty;

        }

    }
}
