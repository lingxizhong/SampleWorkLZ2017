using Formulas;
using SS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Author: Lingxi Zhong U0770136 and Osama Kergaye u0767339
/// </summary>
namespace SpreadsheetGUI
{
    public partial class SpreadsheetGUI : Form, ISpreadsheet
    {
        public SpreadsheetGUI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// New Button event
        /// </summary>
        public event Action NewEvent;

        /// <summary>
        /// Event for textbox input
        /// </summary>
        public event Action<string> InitialContentChange;

        /// <summary>
        /// Event for selecting something in the panel
        /// </summary>
        public event Action<int, int> SelectionEvent;

        /// <summary>
        /// Event for textbox input- Event that helps the panel change values of unselected cells 
        /// </summary>
        public event Action<string> CellRecalcEvent;

        /// <summary>
        /// Event for the Save button
        /// </summary>
        public event Action<string> SaveEvent;

        /// <summary>
        /// Event for the Open button
        /// </summary>
        public event Action<string> OpenEvent;

        /// <summary>
        /// Event for the Close button
        /// </summary>
        public event Action WasSomethingChangedInDataEvent;
        /// <summary>
        /// Row we are currently selected on 
        /// </summary>
        public int row { get; set; }

        /// <summary>
        /// Column we are currently selected on 
        /// </summary>
        public int column { get; set; }

        /// <summary>
        /// If the speadsheet was changed, currenlty false
        /// </summary>
        public Boolean WasChanged { get; set; }

        /// <summary>
        /// This is the New button in the File menu 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void fileMenuNewWindowButton_Click(object sender, EventArgs e)
        {
            if (NewEvent != null)
            {
                NewEvent();
            }
        }

        /// <summary>
        /// Property for getting and setting the value of a cell. Reference property for the controller to use. 
        /// </summary>
        public string CellValue { get; set; }

        /// <summary>
        /// Property for getting and setting contents of a cell. Reference property for the Controller to use. 
        /// </summary>
        public string CellContents { get; set; }

        /// <summary>
        /// This is a IEnum property for populating cells in the spreadsheet that are not currently selected.
        /// </summary>
        public IEnumerable<string> cellRecalc { get; set; }

        /// <summary>
        /// If this ain't null, we got a problem :)
        /// </summary>
        public Exception errorProperty { get; set; }
        /// <summary>
        /// Method for the button "Open" in the File menu
        /// </summary>
        public void OpenNew()
        {
            SpreadsheetGUIApplicationContext.GetContext().RunNew();
        }

        /// <summary>
        /// Action Listener for when a cell in the panel is clicked. 
        /// </summary>
        /// <param name="sender"></param>
        public void ssPanelCellChange(SSGui.SpreadsheetPanel sender)
        {
            int tempCol;
            int tempRow;
            sender.GetSelection(out tempCol, out tempRow);
            column = tempCol;
            row = tempRow;
            
            if (SelectionEvent != null)
            {
                SelectionEvent(column, row);
            }
            CellNameTextBox.Text = "" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[column] + (row + 1);
            ValueTextBox.Text = CellValue;
            ContentsTextBox.Text = CellContents;
        }


        /// <summary>
        /// For when user enters input into content textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ContentsKeyPress(object sender, KeyPressEventArgs e)
        {
            // Try block for error catching
            try
            {
                // Only places input if the key pressed is "Enter". 
                if (e.KeyChar == (char)Keys.Enter)
                {
                    if (InitialContentChange != null)
                    {
                        InitialContentChange((string)ContentsTextBox.Text);
                    }

                    foreach (string cellNames in cellRecalc)
                    {
                        if (CellRecalcEvent != null)
                        {
                            CellRecalcEvent(cellNames);
                        }
                        int rowRC;
                        int colRC;
                        getRowCol(cellNames, out colRC, out rowRC);
                        spreadsheetPanel.SetValue(colRC, rowRC, CellValue);
                    }
                    ValueTextBox.Text = CellValue;
                }
                // Error Catching
            }
            catch (InvalidNameException)
            {
                MessageBox.Show("Please Select a Cell Before Trying to Enter Contents");
            }
            catch (CircularException)
            {
                MessageBox.Show("You have a circular dependency in your spreadsheet");
            }
            catch (FormulaFormatException)
            {
                MessageBox.Show("Invalid Formula");
            }

        }

        /// <summary>
        /// Helper method that converts string input cell names into columns and rows.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        public void getRowCol(string input, out int col, out int row)
        {
            char temp = input[0];
            col = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(temp);
            input = input.Remove(0, 1);
            Int32.TryParse(input, out row);
            row = row - 1;
        }


        /// <summary>
        /// Saves the current Spreadsheet into a file. Default (*.ss)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SaveMenuItem_Click(object sender, EventArgs e)
        {
            // Open File Explorer, get user input for where to save
            SaveFileDialog Save = new SaveFileDialog();
            Save.Title = "Save";
            Save.FileName = "Spreadsheet";
            Save.Filter = "Spreadsheet (*.ss) | *.ss|All files (*.*)|*.*";
            Save.AddExtension = true;
            Save.DefaultExt = ".ss";
            Save.ShowDialog();
            string filename = Save.FileName;
            // Fires the event for the Controller to save the file. Passes in our file explorer file name as the param. 
            if (SaveEvent != null)
            {
                SaveEvent(filename);
            }
        }

        /// <summary>
        /// Opens a file explorer, then opens the specified file into the current Spreadsheet GUI. 
        /// Defaults files to be in (*.ss) format. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OpenMenuItem_Click(object sender, EventArgs e)
        {
            if (WasSomethingChangedInDataEvent != null)
            {
                WasSomethingChangedInDataEvent();
            }
            if(WasChanged == true)
            {
                var dioloagYesNo = MessageBox.Show("You have unsaved changes, opening a new file will result in a loss of data."
                    + "\n Are you sure you want to continue?"
                   , "WARNING!", MessageBoxButtons.YesNo);
                if (dioloagYesNo == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }
            // Open File Explorer, get user input for what to open
            OpenFileDialog Open = new OpenFileDialog();
            Open.Title = "Open";
            Open.Filter = "Spreadsheet (*.ss) | *.ss|All files (*.*)|*.*";
            Open.AddExtension = true;
            Open.DefaultExt = ".ss";
            Open.ShowDialog();
            string filename = Open.FileName;
            if (filename == "")
            {
                return;
            }

            // Call an event to let the controller load the 
            if (OpenEvent != null)
            {
                OpenEvent(filename);
            }
            // Error checking section
            if (errorProperty != null)
            {
                if (errorProperty is SpreadsheetVersionException)
                {
                    MessageBox.Show("You have invalid or out of range cell names in your source file");
                    errorProperty = null;
                    return;
                }
            }

            // We now have to populate the panel
            spreadsheetPanel.Clear();
            ContentsTextBox.Clear();
            ValueTextBox.Clear();

            foreach (string cellNames in cellRecalc)
            {
                if (CellRecalcEvent != null)
                {
                    CellRecalcEvent(cellNames);
                }
                int rowRC;
                int colRC;
                getRowCol(cellNames, out colRC, out rowRC);
                spreadsheetPanel.SetValue(colRC, rowRC, CellValue);
            }
        }

        /// <summary>
        /// This is the "Help" button under the "File" Menu button
        /// Displays tips and instructions for using our Spreadsheet GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HelpMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To start editing, first choose a cell. Then clicking the contents box will allow you to type."
                + "\n Pressing Enter (return) will parse the contents box into the selected cell."
                + "\n Save will allow you to save files, default in the .ss form. The Open will allow you to open .ss files");
        }



        /// <summary>
        /// Checks if the spreadsheet was changed, if so, prompt the user before closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseClicked(object sender, FormClosingEventArgs e)
        {
            if (WasSomethingChangedInDataEvent != null)
            {
                WasSomethingChangedInDataEvent();
            }
            if (WasChanged == true)
            {
                var dioloagYesNo = MessageBox.Show("You have unsaved changes, are you sure you want to exit?"
                    , "WARNING!", MessageBoxButtons.YesNo);
                if (dioloagYesNo == System.Windows.Forms.DialogResult.Yes)
                {
                    return; 
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void MenuClose(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
