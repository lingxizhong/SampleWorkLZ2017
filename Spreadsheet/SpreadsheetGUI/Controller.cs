using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS;
using System.IO;
using System.Text.RegularExpressions;
using Formulas;

/// <summary>
/// Author: Lingxi Zhong U0770136 and Osama Kergaye u0767339
/// </summary>
namespace SpreadsheetGUI
{
    public class Controller
    {

        /// <summary>
        /// Interface window to be used by controller, memes memes, nobody reads me.. :(
        /// </summary>
        private ISpreadsheet window;

        /// <summary>
        /// Model
        /// </summary>
        private Spreadsheet data;

        /// <summary>
        /// Current cell sellected, used here for convinence
        /// </summary>
        private string currentCellName;

        /// <summary>
        /// Controller constructor for opperating the model and view
        /// </summary>
        /// <param name="window"></param>
        public Controller(ISpreadsheet window)
        {
            this.data = new Spreadsheet();
            this.window = window;
            window.NewEvent += HandleNew; // "New" button in file menu
            window.SelectionEvent += HandleSelection; // Selecting an item in the panel 
            window.InitialContentChange += ContentSetting; // First phase of the context box enter 
            window.CellRecalcEvent += valueRecalculating; // Second phase of the context box enter
            window.SaveEvent += saveFile; // Saving a file
            window.OpenEvent += openFile; // Opening a file
            window.WasSomethingChangedInDataEvent += closeForm;
        }


        /// <summary>
        /// updates the view to know if the spreadsheet was changed
        /// </summary>
        private void closeForm()
        {
            if (data.Changed == true)
            {
                window.WasChanged = true;
            }else
            {
                window.WasChanged = false;
            }
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
            
            var isform = data.GetCellContents(currentCellName);
            if (isform is Formula)
            {
                window.CellContents = "=" + isform.ToString();
            }
            else
            {
                window.CellContents = data.GetCellContents(currentCellName).ToString();

            }
        }


        /// <summary>
        /// gives the view information about what cells need to be redisplayed
        /// </summary>
        /// <param name="contents"></param>
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
