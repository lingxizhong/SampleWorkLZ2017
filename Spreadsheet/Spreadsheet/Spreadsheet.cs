﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;
using Dependencies;
using System.Text.RegularExpressions;
/// <summary>
/// Implementation by Lingxi Zhong U0770136
/// </summary>
namespace SS
{
    /// <summary>
    /// Core for the spreadsheet project Class that has back end code calls
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// This is the dictionary 
        /// </summary>
        private Dictionary<string, Cell> data;
        private DependencyGraph depGraph;
        /// <summary>
        /// An AbstractSpreadsheet object represents the state of a simple spreadsheet.  A 
        /// spreadsheet consists of an infinite number of named cells.
        /// 
        /// A string s is a valid cell name if and only if it consists of one or more letters, 
        /// followed by a non-zero digit, followed by zero or more digits.
        /// 
        /// For example, "A15", "a15", "XY32", and "BC7" are valid cell names.  On the other hand, 
        /// "Z", "X07", and "hello" are not valid cell names.
        /// 
        /// A spreadsheet contains a cell corresponding to every possible cell name.  
        /// In addition to a name, each cell has a contents and a value.  The distinction is
        /// important, and it is important that you understand the distinction and use
        /// the right term when writing code, writing comments, and asking questions.
        /// 
        /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
        /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
        /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
        /// 
        /// In an empty spreadsheet, the contents of every cell is the empty string.
        ///  
        /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
        /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
        /// in the grid.)
        /// 
        /// If a cell's contents is a string, its value is that string.
        /// 
        /// If a cell's contents is a double, its value is that double.
        /// 
        /// If a cell's contents is a Formula, its value is either a double or a FormulaError.
        /// The value of a Formula, of course, can depend on the values of variables.  The value 
        /// of a Formula variable is the value of the spreadsheet cell it names (if that cell's 
        /// value is a double) or is undefined (otherwise).  If a Formula depends on an undefined
        /// variable or on a division by zero, its value is a FormulaError.  Otherwise, its value
        /// is a double, as specified in Formula.Evaluate.
        /// 
        /// Spreadsheets are never allowed to contain a combination of Formulas that establish
        /// a circular dependency.  A circular dependency exists when a cell depends on itself.
        /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
        /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
        /// dependency.
        /// </summary>
        public Spreadsheet()
        {
            data = new Dictionary<string, Cell>();
            depGraph = new DependencyGraph();
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            if (name == null)
            {
                throw new InvalidNameException();
            }
            Cell temp;
            Boolean check = data.TryGetValue(name, out temp);
            if(check == false)
            {
                temp = new Cell();
            }
            return temp.getContents();
        }


        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            HashSet<string> names = new HashSet<string>();
            foreach (string s in data.Keys)
            {
                names.Add(s);
            }
            return names;            
        }


        /// <summary>
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            // Checking for validity, if the dictionary already contains item, remove it. 
            if (!validityCheck(name))
            {
                throw new InvalidNameException();
            }
            if (data.ContainsKey(name))
            {
                data.Remove(name);
            }
            // Gonna add some stuff to my dependency Graph
            ISet<string> deps = formula.GetVariables();
            HashSet<string> depsReplace = new HashSet<string>();
            foreach (string s in deps)
            {
                depsReplace.Add(s);
            }
            depGraph.ReplaceDependents(name, depsReplace);
            // Ok now we can add to cell and evaluate
            Cell temp = new Cell();
            temp.setDataF(formula);
            data.Add(name, temp);
            IEnumerable<string> tempList = GetCellsToRecalculate(name);
            HashSet<string> result = new HashSet<string>();
            foreach(string s in tempList)
            {
                result.Add(s);
            }
            return result;
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, string text)
        {
            if(text == null)
            {
                throw new ArgumentNullException();
            }
            // Check for validity and if item already exists
            if (!validityCheck(name))
            {
                throw new InvalidNameException();
            }
            if (data.ContainsKey(name))
            {
                data.Remove(name);
            }
            
            // Need to remove this cell from dependency graph it was there
            HashSet<string> empty = new HashSet<string>();
            depGraph.ReplaceDependents(name, empty);
            Cell temp = new Cell();
            temp.setData(text);
            if(text != "")
            {
                data.Add(name, temp);
            }
            
            IEnumerable<string> tempList = GetCellsToRecalculate(name);
            HashSet<string> result = new HashSet<string>();
            foreach (string s in tempList)
            {
                result.Add(s);
            }
            return result;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, double number)
        {
            if (!validityCheck(name))
            {
                throw new InvalidNameException();
            }
            if (data.ContainsKey(name))
            {
                data.Remove(name);
            }
            HashSet<string> empty = new HashSet<string>();
            depGraph.ReplaceDependents(name, empty);
            Cell temp = new Cell();
            temp.setData(number);
            data.Add(name, temp);
            IEnumerable<string> tempList = GetCellsToRecalculate(name);
            HashSet<string> result = new HashSet<string>();
            foreach (string s in tempList)
            {
                result.Add(s);
            }
            return result;
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            return depGraph.GetDependents(name);
        }


        /// <summary>
        /// In here, the name is checked to see that it is an actual cell name.
        /// It also needs to check if the name is already in the data dictionary. If it is, we need to remove it. 
        /// </summary>
        /// <param name="name"></param>
        /// name is the name of the cell you want to check validity for. 
        /// <returns></returns>
        private Boolean validityCheck(String name)
        {
            if(name == null)
            {
                return false;
            }
            if(!(Regex.IsMatch(name, @"^[a-zA-Z]+[1-9]+[0-9]*$")))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        public class Cell
        {
            /// <summary>
            /// The value of the cell
            /// </summary>
            private Object value;
            /// <summary>
            /// The contents of a cell
            /// </summary>
            private Object contents; 
            /// <summary>
            /// Nested class object for handling spreadsheet cells
            /// </summary>
            public Cell()
            {
                string empVal = "";
                string empCon = "";
                value = empVal;
                contents = empCon;
            }
            /// <summary>
            /// Method call for setting the contents and value of a cell, if the input is a string or double
            /// </summary>
            /// <param name="inputValue"></param>
            public void setData(Object inputValue)
            {
                contents = inputValue;
                value = inputValue;
            }

            /// <summary>
            /// Special method call for setting the contents and value of a cell, if the input is a formula
            /// </summary>
            /// <param name="inputValue"></param>
            public void setDataF(Formula inputValue)
            {
                contents = inputValue;
                value = inputValue;
                // Future proofing? May have to do something different with formula "values" in the future. 
            }

            /// <summary>
            /// Returns the value of a cell
            /// </summary>
            /// <returns></returns>
            //public object getValue()
            //{
            //    return value;
            //}
            /// <summary>
            /// Returns the contents of a cell
            /// </summary>
            /// <returns></returns>
            public object getContents()
            {
                return contents;
            }
        }
    }


}
