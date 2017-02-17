using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System.Collections.Generic;
using Formulas;

/// <summary>
/// Testing done by Lingxi Zhong U0770136
/// </summary>
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
            test.SetCellContents("A2", new Formula("1+3"));
            test.SetCellContents("A3", "Hello World");
        }

        [TestMethod]
        public void EmptyCellContentTest()
        {
            Spreadsheet empty = new Spreadsheet();
            string result = (string)empty.GetCellContents("A1");
            string expected = "";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DoubleSetCellContents()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetCellContents("A1", 5.5);
            double result = (double)test.GetCellContents("A1");
            double expected = 5.5;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void StringSetCellContent()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetCellContents("A1", "Hello World");
            string result = (string)test.GetCellContents("A1");
        }

        [TestMethod]
        public void FormulaSetCellContentNoVar()
        {
            Spreadsheet test = new Spreadsheet();
            Formula expected = new Formula("1+3");
            test.SetCellContents("A1", expected);
            Formula result = (Formula)test.GetCellContents("A1");
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FormulaSetReplaceWithVars()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetCellContents("A1", 2.0);
            Formula someFormula = new Formula("A1+3");
            ISet<string> tempList = test.SetCellContents("A2", someFormula);
            HashSet<string> result1 = new HashSet<string>();
            foreach(string s in tempList)
            {
                result1.Add(s);
            }
            Assert.IsTrue(result1.Contains("A1"));
            Formula someOtherFormula = new Formula("A3+3");
            ISet<string> tempList2 = test.SetCellContents("A2", someOtherFormula);
            HashSet<string> result2 = new HashSet<string>();
            foreach(string t in tempList2)
            {
                result2.Add(t);
            }
            Assert.IsTrue(result2.Contains("A3"));
            Assert.IsFalse(result2.Contains("A1"));
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void FormulaSetNullElement()
        {
            Spreadsheet test = new Spreadsheet();
            Formula expected = new Formula("1+3");
            test.SetCellContents("A01", expected);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidCellNamesCheck()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetCellContents("Z", "sgsadfjoasjof");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidCellNameCheck2()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetCellContents("X07", "sdgasdf");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidCellNameCheck3()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetCellContents("hello", "sdfasddfasd");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidCellnamecheckWithDouble()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetCellContents("A07", 2.0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void nullNameTest()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetCellContents(null, 2.0);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidCellNameCheck4Null()
        {
            Spreadsheet test = new Spreadsheet();
            test.GetCellContents(null);
        }

        [TestMethod]
        public void getNonEmptyCellsTest()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetCellContents("A1", 5.0);
            test.SetCellContents("A2", new Formula("1+3"));
            test.SetCellContents("A3", "Hello World");
            IEnumerable<string> tempList = test.GetNamesOfAllNonemptyCells();
            HashSet<string> result = new HashSet<string>();
            foreach(string s in tempList)
            {
                result.Add(s);
            }
            Assert.IsTrue(result.Contains("A1"));
            Assert.IsTrue(result.Contains("A2"));
            Assert.IsTrue(result.Contains("A3"));
        }

        [TestMethod]
        public void getNonEmptyCellsTestWithRemove()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetCellContents("A1", 5.0);
            test.SetCellContents("A2", new Formula("1+3"));
            test.SetCellContents("A3", "Hello World");
            test.SetCellContents("A3", "");
            IEnumerable<string> tempList = test.GetNamesOfAllNonemptyCells();
            HashSet<string> result = new HashSet<string>();
            foreach (string s in tempList)
            {
                result.Add(s);
            }
            Assert.IsTrue(result.Contains("A1"));
            Assert.IsTrue(result.Contains("A2"));
            Assert.IsFalse(result.Contains("A3"));
        }

        [TestMethod]
        public void replaceWithDoubleCall()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetCellContents("A1", 5.0);
            test.SetCellContents("A2", new Formula("A1+3"));
            test.SetCellContents("A3", "Hello World");
            ISet<string> temp = test.SetCellContents("A2", 6);
            HashSet<string> result = new HashSet<string>();
            foreach(string s in temp)
            {
                result.Add(s);
            }
            Assert.IsFalse(result.Contains("A1"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void nullTextStringTest()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetCellContents("A!", null);
        }

    }
}
