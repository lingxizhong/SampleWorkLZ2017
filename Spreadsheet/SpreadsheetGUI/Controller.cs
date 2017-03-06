using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    class Controller
    {
        private SpreadsheetGUI window;
        public Controller(SpreadsheetGUI window)
        {
            this.window = window;
            window.NewEvent += HandleNew;
        }


        private void HandleNew()
        {
            window.OpenNew();
        }
    }
}
