using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS;

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
            window.NewEvent += HandleNew;
            window.SelectionEvent += HandleSelection;
            window.InitialContentChange += ContentSetting;
            window.CellRecalcEvent += valueRecalculating;
        }


        private void HandleNew()
        {
            window.OpenNew();
        }

        private void HandleSelection(int column, int row)
        {
            currentCellName = getCellName(column, row);
            window.CellValue = (string)data.GetCellValue(currentCellName);
        }

        private void ContentSetting(string contents)
        {
            window.cellRecalc = data.SetContentsOfCell(currentCellName, contents);
        }

        private void valueRecalculating(string contents)
        {
            window.CellValue = data.GetCellValue(contents).ToString();
        }

        private string getCellName(int column, int row)
        {
            return "" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[column] + (row + 1);
        }

    }
}
