using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;
using Dependencies;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Xml.Schema;

// Implementation by Lingxi Zhong U0770136
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
        private Regex isValidRegex;

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression accepts every string.
        /// </summary>
        public Spreadsheet()
        {
            data = new Dictionary<string, Cell>();
            depGraph = new DependencyGraph();
            isValidRegex = new Regex(".*?");
            Changed = false;
        }

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression is provided as the parameter
        /// </summary>
        /// <param name="isValid"></param>
        public Spreadsheet(Regex isValid)
        {
            data = new Dictionary<string, Cell>();
            depGraph = new DependencyGraph();
            isValidRegex = isValid;
            Changed = false;
        }

        /// <summary>
        /// Creates a Spreadsheet that is a duplicate of the spreadsheet saved in source.
        ///
        /// See the AbstractSpreadsheet.Save method and Spreadsheet.xsd for the file format 
        /// specification.  
        ///
        /// If there's a problem reading source, throws an IOException.
        ///
        /// Else if the contents of source are not consistent with the schema in Spreadsheet.xsd, 
        /// throws a SpreadsheetReadException.  
        ///
        /// Else if the IsValid string contained in source is not a valid C# regular expression, throws
        /// a SpreadsheetReadException.  (If the exception is not thrown, this regex is referred to
        /// below as oldIsValid.)
        ///
        /// Else if there is a duplicate cell name in the source, throws a SpreadsheetReadException.
        /// (Two cell names are duplicates if they are identical after being converted to upper case.)
        ///
        /// Else if there is an invalid cell name or an invalid formula in the source, throws a 
        /// SpreadsheetReadException.  (Use oldIsValid in place of IsValid in the definition of 
        /// cell name validity.)
        ///
        /// Else if there is an invalid cell name or an invalid formula in the source, throws a
        /// SpreadsheetVersionException.  (Use newIsValid in place of IsValid in the definition of
        /// cell name validity.)
        ///
        /// Else if there's a formula that causes a circular dependency, throws a SpreadsheetReadException. 
        ///
        /// Else, create a Spreadsheet that is a duplicate of the one encoded in source except that
        /// the new Spreadsheet's IsValid regular expression should be newIsValid.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newIsValid"></param>
        public Spreadsheet(TextReader source, Regex newIsValid)
        {
            data = new Dictionary<string, Cell>();
            depGraph = new DependencyGraph();


            // Create the XmlSchemaSet class.  Anything with the namespace "urn:states-schema" will
            // be validated against states3.xsd.
            XmlSchemaSet sc = new XmlSchemaSet();

            // NOTE: To read states3.xsd this way, it must be stored in the same folder with the
            // executable.  To arrange this, I set the "Copy to Output Directory" propery of states3.xsd to
            // "Copy If Newer", which will copy states3.xsd as part of each build (if it has changed
            // since the last build).
            sc.Add(null, "Spreadsheet.xsd");

            // Configure validation.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;
            settings.ValidationEventHandler += ValidationCallback;
            using (XmlReader reader = XmlReader.Create(source, settings))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "spreadsheet":
                                try
                                {
                                    isValidRegex = new Regex(reader["IsValid"]);
                                }
                                catch (Exception)
                                {
                                    throw new SpreadsheetReadException("Regex not Valid");
                                }

                                break;

                            case "cell":
                                try
                                {
                                    Cell temp;
                                    Boolean flag = data.TryGetValue(reader["name"], out temp);
                                    if (flag == true)
                                    {
                                        throw new SpreadsheetReadException("Cell Already Exists");
                                    }
                                    SetContentsOfCell(reader["name"], reader["contents"]);
                                }
                                catch (Exception)
                                {
                                    throw new SpreadsheetReadException("Something went wrong");
                                }

                                break;
                        }
                    }
                }
            }
            isValidRegex = newIsValid;
            IEnumerable<string> versionCheckList = GetNamesOfAllNonemptyCells();
            foreach(string cellNames in versionCheckList)
            {
                if(!Regex.IsMatch(cellNames, newIsValid.ToString()))
                {
                    throw new SpreadsheetVersionException("Invalid cell name with new regex");
                }
            }
            Changed = false;
        }

        // Display any validation errors.
        private static void ValidationCallback(object sender, ValidationEventArgs e)
        {
            Console.WriteLine(" *** Validation Error: {0}", e.Message);
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            if (!validityCheck(name))
            {
                throw new InvalidNameException();
            }
            name = name.ToUpper();
            Cell temp;
            Boolean check = data.TryGetValue(name, out temp);
            if (check == false)
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
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            // Checking for validity, if the dictionary already contains item, remove it. 
            if (!validityCheck(name))
            {
                throw new InvalidNameException();
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
            IEnumerable<string> tempList = GetCellsToRecalculate(name);
            if (data.ContainsKey(name))
            {
                data.Remove(name);
            }
            Cell temp = new Cell();
            Cell paramCell = new Cell();
            data.TryGetValue(name, out paramCell);
            temp.setDataF(formula);
            data.Add(name, temp);
            HashSet<string> result = new HashSet<string>();
            foreach (string s in tempList)
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
        protected override ISet<string> SetCellContents(string name, string text)
        {
            if (text == null)
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
            if (text != "")
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
        protected override ISet<string> SetCellContents(string name, double number)
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
            return depGraph.GetDependees(name);
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
            if (name == null)
            {
                return false;
            }
            if (!(isValidRegex.IsMatch(name)))
            {
                return false;
            }
            return true;
        }

        // ADDED FOR PS6
        /// <summary>
        /// Writes the contents of this spreadsheet to dest using an XML format.
        /// The XML elements should be structured as follows:
        ///
        /// <spreadsheet IsValid="IsValid regex goes here">
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        /// </spreadsheet>
        ///
        /// The value of the IsValid attribute should be IsValid.ToString()
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.
        /// If the cell contains a string, the string (without surrounding double quotes) should be written as the contents.
        /// If the cell contains a double d, d.ToString() should be written as the contents.
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        ///
        /// If there are any problems writing to dest, the method should throw an IOException.
        /// </summary>
        public override void Save(TextWriter dest)
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(dest))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("spreadsheet");
                xmlWriter.WriteAttributeString("IsValid", isValidRegex.ToString());
                Cell tempCell;
                IEnumerable<string> list = GetNamesOfAllNonemptyCells();
                foreach (string cellAddress in list)
                {
                    data.TryGetValue(cellAddress, out tempCell);
                    xmlWriter.WriteStartElement("cell");
                    xmlWriter.WriteAttributeString("name", cellAddress);
                    if (tempCell.getContents() is Formula)
                    {
                        string formulaS = tempCell.getContents().ToString();
                        formulaS = "=" + formulaS;
                        xmlWriter.WriteAttributeString("contents", formulaS);
                    }
                    else
                    {
                        xmlWriter.WriteAttributeString("contents", tempCell.getContents().ToString());
                    }
                    xmlWriter.WriteFullEndElement();
                }
            }
            Changed = false;
        }

        // ADDED FOR PS6
        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed
        {
            get;
            protected set;
        }

        // ADDED FOR PS6
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            name = name.ToUpper();
            if (!validityCheck(name))
            {
                throw new InvalidNameException();
            }
            Cell temp;
            Boolean check = data.TryGetValue(name, out temp);
            if (check == false)
            {
                temp = new Cell();
            }
            return temp.getValue();
        }

        // ADDED FOR PS6
        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        ///
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor with s => s.ToUpper() as the normalizer and a validator that
        /// checks that s is a valid cell name as defined in the AbstractSpreadsheet
        /// class comment.  There are then three possibilities:
        ///
        ///   (1) If the remainder of content cannot be parsed into a Formula, a
        ///       Formulas.FormulaFormatException is thrown.
        ///
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///
        ///   (3) Otherwise, the contents of the named cell becomes f.
        ///
        /// Otherwise, the contents of the named cell becomes content.
        ///
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            if (name == null)
            {
                throw new InvalidNameException();
            }
            name = name.ToUpper();
            validityCheck(name);
            if (content == null)
            {
                throw new ArgumentNullException();
            }
            if (content == "")
            {
                Changed = true;
                return SetCellContents(name, content);
            }
            double doubResult;
            Boolean doubBool = Double.TryParse(content, out doubResult);
            ISet<string> list;
            if (doubBool == true)
            {
                list = SetCellContents(name, doubResult);
                list.Remove(name);
                foreach (string s in list)
                {
                    Cell temp;
                    bool check = data.TryGetValue(s, out temp);
                    if (check == false)
                    {
                        temp = new Cell();
                    }
                    try
                    {
                        temp.value = ((Formula)temp.getContents()).Evaluate(formulaCheck);
                    }
                    catch
                    {
                        temp.value = new FormulaError();
                    }
                }
                list.Add(name);
                Changed = true;
                return list;
            }
            // If input is a formula
            if (content[0] == '=')
            {


                content = content.Remove(0, 1); // We're going to remove the equals sign
                Formula formulaParse = new Formula(content, s => s.ToUpper(), s => validityCheck(s)); // Create a formula out of it, and make sure its valid
                list = SetCellContents(name, formulaParse); // We're going to set the contents, and by doing so 
                foreach (string s in list)
                {
                    Cell temp;
                    bool check = data.TryGetValue(s, out temp);
                    if (check == false)
                    {
                        temp = new Cell();
                    }
                    try
                    {
                        temp.value = ((Formula)temp.getContents()).Evaluate(formulaCheck);
                    }
                    catch
                    {
                        temp.value = new FormulaError();
                    }
                }
                Changed = true;
                return list;
            }

            Changed = true;
            list = SetCellContents(name, content);
            list.Remove(name);
            foreach (string s in list)
            {
                Cell temp;
                bool check = data.TryGetValue(s, out temp);
                if (check == false)
                {
                    temp = new Cell();
                }
                try
                {
                    temp.value = ((Formula)temp.getContents()).Evaluate(formulaCheck);
                }
                catch
                {
                    temp.value = new FormulaError();
                }
            }
            list.Add(name);
            Changed = true;
            return list;
        }

        private double formulaCheck(string name)
        {
            Cell temp;
            Boolean flag = data.TryGetValue(name, out temp);
            if (flag == false || !(temp.getValue() is double))
            {
                throw new FormulaEvaluationException("Cell Does not exist");
            }
            return (double)temp.getValue();
        }
        /// <summary>
        /// 
        /// </summary>
        public class Cell
        {
            /// <summary>
            /// The value of the cell
            /// </summary>
            public Object value;
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
                // Future proofing? May have to do something different with formula "values" in the future. 
            }

            /// <summary>
            /// Returns the value of a cell
            /// </summary>
            /// <returns></returns>
            public object getValue()
            {
                return value;
            }
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
