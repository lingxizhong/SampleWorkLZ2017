using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS;
using System.IO;
using System.Text.RegularExpressions;

namespace SpreadsheetGUI
{
    class Controller
    {
        private SpreadsheetGUI window;
        private Spreadsheet data;
        private string currentCellName;
        public Controller(SpreadsheetGUI window)
        {
            this.data = new Spreadsheet();
            this.window = window;
            window.NewEvent += HandleNew; // "New" button in file menu
            window.SelectionEvent += HandleSelection; // Selecting an item in the panel 
            window.InitialContentChange += ContentSetting; // First phase of the context box enter 
            window.CellRecalcEvent += valueRecalculating; // Second phase of the context box enter
            window.SaveEvent += saveFile; // Saving a file
            window.OpenEvent += openFile; // Opening a file
        }

        /// <summary>
        /// When trying to open a new spreadsheet GUI. 
        /// </summary>
        private void HandleNew()
        {
            window.OpenNew();
        }

        /// <summary>
        /// When a different cell in the cell panel is selected, this event is called from the spreadsheet GUI. 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        private void HandleSelection(int column, int row)
        {
            currentCellName = getCellName(column, row);
            window.CellValue = data.GetCellValue(currentCellName).ToString();
            window.CellContents = data.GetCellContents(currentCellName).ToString();
        }

        private void ContentSetting(string contents)
        {
            window.cellRecalc = data.SetContentsOfCell(currentCellName, contents);
        }

        /// <summary>
        /// This method gets called in a loop, cell value will change 
        /// </summary>
        /// <param name="contents"></param>
        private void valueRecalculating(string contents)
        {
            window.CellValue = data.GetCellValue(contents).ToString();
        }

        /// <summary>
        /// Concatonates columns and rows 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private string getCellName(int column, int row)
        {
            return "" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[column] + (row + 1);
        }

        /// <summary>
        /// Method for savingFiles
        /// </summary>
        /// <param name="filePath"></param>
        private void saveFile(string filePath)
        {
            TextWriter filePathWriter = new StreamWriter(filePath);
            data.Save(filePathWriter);
        }

        /// <summary>
        /// Method for opening files. 
        /// </summary>
        /// <param name="filePath"></param>
        private void openFile(string filePath)
        {
            try
            {
                TextReader openFileReader = new StreamReader(filePath);
                data = new Spreadsheet(openFileReader, new Regex(@"^[a-zA-Z][1-9][0-9]{0,1}$"));
                window.cellRecalc = data.GetNamesOfAllNonemptyCells();
            } catch(Exception e)
            {
                window.errorProperty = e;
            }
            
        }

    }
}
