﻿using Formulas;
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

namespace SpreadsheetGUI
{
    public partial class SpreadsheetGUI : Form
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
        /// Row we are currently selected on 
        /// </summary>
        private int row;

        /// <summary>
        /// Column we are currently selected on 
        /// </summary>
        private int column;

        /// <summary>
        /// This is the New button in the File menu 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileMenuNewWindowButton_Click(object sender, EventArgs e)
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
        private void ssPanelCellChange(SSGui.SpreadsheetPanel sender)
        {
            sender.GetSelection(out column, out row);
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
        private void ContentsKeyPress(object sender, KeyPressEventArgs e)
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
            } catch(InvalidNameException)
            {
                MessageBox.Show("Please Select a Cell Before Trying to Enter Contents");
            } catch(CircularException)
            {
                MessageBox.Show("You have a circular dependency in your spreadsheet");
            } catch(FormulaFormatException)
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
        private void getRowCol(string input, out int col, out int row)
        {
            char temp = input[0];
            col = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(temp);
            input = input.Remove(0, 1);
            Int32.TryParse(input, out row);
            row = row - 1;
        }


        /// <summary>
        /// Saves the current Spreadsheet into a (*.ss) file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
            // Open File Explorer, get user input for where to save
            SaveFileDialog Save = new SaveFileDialog();
            Save.Title = "Save";
            Save.FileName = "Spreadsheet";
            Save.Filter = "Spreadsheet (*.ss) | *.ss";
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
        /// Requires files to be in (*.ss) format. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            // Open File Explorer, get user input for what to open
            OpenFileDialog Open = new OpenFileDialog();
            Open.Title = "Open";
            Open.Filter = "Spreadsheet (*.ss) | *.ss";
            Open.AddExtension = true;
            Open.DefaultExt = ".ss";
            Open.ShowDialog();
            string filename = Open.FileName;
            // Call an event to let the controller load the 
            if(OpenEvent != null)
            {
                OpenEvent(filename);
            }

        }

        /// <summary>
        /// This is the "Help" button under the "File" Menu button
        /// Displays tips and instructions for using our Spreadsheet GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
