using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System.Collections.Generic;
using Formulas;
using System.Text.RegularExpressions;
using System.IO;

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
        public void BasicSetContentsOfCellTest()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A1", "5.0");
            test.SetContentsOfCell("A2", "=1+3");
            test.SetContentsOfCell("A3", "Hello World");
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
        public void DoubleSetContentsOfCell()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A1", "5.5");
            double result = (double)test.GetCellContents("A1");
            double expected = 5.5;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void StringSetCellContent()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A1", "Hello World");
            string result = (string)test.GetCellContents("A1");
        }

        [TestMethod]
        public void FormulaSetCellContentNoVar()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A1", "=1+3");
            Formula result = (Formula)test.GetCellContents("A1");
            Assert.AreEqual("1+3", result.ToString());
        }

        [TestMethod]
        public void FormulaSetReplaceWithVars()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A1", "2.0");
            test.SetContentsOfCell("A2", "=A1+3");
            ISet<string> tempList = test.SetContentsOfCell("A1", "3.0");
            HashSet<string> result1 = new HashSet<string>();
            foreach(string s in tempList)
            {
                result1.Add(s);
            }
            Assert.IsTrue(result1.Contains("A2"));
        }

        [TestMethod]
        public void FormulaSetReplaceWithVars2()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("B1", "=A1*2");
            test.SetContentsOfCell("C1", "=B1+A1");
            ISet<string> tempList = test.SetContentsOfCell("A1", "3.0");
            HashSet<string> result1 = new HashSet<string>();
            foreach (string s in tempList)
            {
                result1.Add(s);
            }
            Assert.IsTrue(result1.Contains("A1"));
            Assert.IsTrue(result1.Contains("B1"));
            Assert.IsTrue(result1.Contains("C1"));
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void FormulaSetNullElement()
        {
            Spreadsheet test = new Spreadsheet(new Regex(@"^[a-zA-Z]+[1-9]+[0-9]*$"));
            test.SetContentsOfCell("A01", "=1+3");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidCellNamesCheck()
        {
            Spreadsheet test = new Spreadsheet(new Regex(@"^[a-zA-Z]+[1-9]+[0-9]*$"));
            test.SetContentsOfCell("Z", "sgsadfjoasjof");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidCellNameCheck2()
        {
            Spreadsheet test = new Spreadsheet(new Regex(@"^[a-zA-Z]+[1-9]+[0-9]*$"));
            test.SetContentsOfCell("X07", "sdgasdf");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidCellNameCheck3()
        {
            Spreadsheet test = new Spreadsheet(new Regex(@"^[a-zA-Z]+[1-9]+[0-9]*$"));
            test.SetContentsOfCell("hello", "sdfasddfasd");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidCellnamecheckWithDouble()
        {
            Spreadsheet test = new Spreadsheet(new Regex(@"^[a-zA-Z]+[1-9]+[0-9]*$"));
            test.SetContentsOfCell("A07", "2.0");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void nullNameTest()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetContentsOfCell(null, "2.0");
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
            test.SetContentsOfCell("A1", "5.0");
            test.SetContentsOfCell("A2", "=1+3");
            test.SetContentsOfCell("A3", "Hello World");
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
            test.SetContentsOfCell("A1", "5.0");
            test.SetContentsOfCell("A2", "=1+3");
            test.SetContentsOfCell("A3", "Hello World");
            test.SetContentsOfCell("A3", "");
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
            test.SetContentsOfCell("A1", "5.0");
            test.SetContentsOfCell("A2", "=A1+3");
            test.SetContentsOfCell("A3", "Hello World");
            ISet<string> temp = test.SetContentsOfCell("A2", "6");
            HashSet<string> result = new HashSet<string>();
            foreach(string s in temp)
            {
                result.Add(s);
            }
            Assert.IsFalse(result.Contains("A1"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void nullTextStringTest()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A1", null);
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void circularDependencyException()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A1", "=A2+B1");
            test.SetContentsOfCell("A2", "=A1+B2");
        }

        // PS6 Tests Begin
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void secondConstructorTest()
        {
            Spreadsheet test = new Spreadsheet(new Regex(@"^[a-zA-Z]+[1-9]+[0-9]*$"));
            test.SetContentsOfCell("2A", "wat");
        }

        [TestMethod]
        public void savingAXMLFileTest()
        {
            Spreadsheet xmlTest = new Spreadsheet(new Regex(@"^[a-zA-Z]+[1-9]+[0-9]*$"));
            xmlTest.SetContentsOfCell("A1", "String");
            xmlTest.SetContentsOfCell("A2", "2.3");
            xmlTest.SetContentsOfCell("A3", "=2+3");
            TextWriter firstXMLTest = new StreamWriter("firstXMLTest.xml");
            xmlTest.Save(firstXMLTest);
        }

        [TestMethod]
        public void readinginAXMLFileTest()
        {
            TextReader secondXMLTest = new StreamReader("firstXMLRead.xml");
            Spreadsheet readInXML = new Spreadsheet(secondXMLTest, new Regex(@"^[a-zA-Z]+[1-9]+[0-9]*$"));
            string result1 = (string)readInXML.GetCellValue("A1");
            double result2 = (double)readInXML.GetCellValue("A2");
            double result3 = (double)readInXML.GetCellValue("A3");
            Assert.AreEqual("String", result1);
            Assert.AreEqual(2.3, result2, .001);
            Assert.AreEqual(5, result3, .001);
            Assert.IsFalse(readInXML.Changed);
            readInXML.SetContentsOfCell("A1", "eyy");
            Assert.IsTrue(readInXML.Changed);
            TextWriter secXMLTest = new StreamWriter("secXMLTest.xml");
            readInXML.Save(secXMLTest);
            Assert.IsFalse(readInXML.Changed);
        }
      

    }
}
