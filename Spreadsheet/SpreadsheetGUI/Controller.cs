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
        private int row;
        private int column;
        private Spreadsheet data;
        public Controller(SpreadsheetGUI window)
        {
            this.data = new Spreadsheet();
            this.window = window;
            window.NewEvent += HandleNew;
            window.SelectionEvent += HandleSelection;
        }


        private void HandleNew()
        {
            window.OpenNew();
        }

        private void HandleSelection(int column, int row)
        {

        }
    }
}
