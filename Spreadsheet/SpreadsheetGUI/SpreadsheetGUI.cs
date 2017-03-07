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

namespace SpreadsheetGUI
{
    public partial class SpreadsheetGUI : Form
    {
        public SpreadsheetGUI()
        {
            InitializeComponent();
        }


        public event Action NewEvent;
        public event Action<string> InitialContentChange;
        public event Action<int, int> SelectionEvent;
        public event Action<string> CellRecalcEvent;
        public event Action<string> SaveEvent;
        public event Action<string> OpenEvent;
        private int row;
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

        public string CellValue { get; set; }
        public string CellContents { get; set; }

        public IEnumerable<string> cellRecalc { get; set; }

        public void OpenNew()
        {
            SpreadsheetGUIApplicationContext.GetContext().RunNew();
        }

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

        private void ContentsKeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
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

        private void getRowCol(string input, out int col, out int row)
        {
            char temp = input[0];
            col = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(temp);
            input = input.Remove(0, 1);
            Int32.TryParse(input, out row);
            row = row - 1;
        }


        private void contentBoxChange(object sender, EventArgs e)
        {

        }

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
            string filename = ""; // Edit this
            if (SaveEvent != null)
            {
                SaveEvent(filename);
            }
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            // Open File Explorer, get user input for what to open
            OpenFileDialog Open = new OpenFileDialog();
            Open.Title = "Open";
            Open.Filter = "Spreadsheet (*.ss) | *.ss";
            Open.AddExtension = true;
            Open.DefaultExt = ".ss";
            Open.ShowDialog();
            string filename = ""; // Edit this
            if(OpenEvent != null)
            {
                OpenEvent(filename);
            }

        }
    }
}
