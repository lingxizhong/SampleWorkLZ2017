using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System.Collections.Generic;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadSheetTests
    {
        [TestMethod]
        public void ConstructorTest()
        {
            Spreadsheet test = new Spreadsheet();
        }

        [TestMethod]
        public void BasicSetCellContentsTest()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetCellContents("A1", 5.0);
            test.SetCellContents("A2", new Formulas.Formula("1+3"));
            test.SetCellContents("A3", "Hello World");
            test.GetCellContents("A1");
        }

    }
}
