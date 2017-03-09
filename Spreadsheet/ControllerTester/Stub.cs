using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetGUI;
using SS;
using System.Windows.Forms;


namespace ControllerTester
{
    class Stub : ISpreadsheet
    {
        public string CellContents
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<string> cellRecalc
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string CellValue
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int column
        {
            set
            {
                throw new NotImplementedException();
            }
        }

        public Exception errorProperty
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int row
        {
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool WasChanged
        {
            set
            {
                throw new NotImplementedException();
            }
        }

        public event Action<string> CellRecalcEvent;
        public event Action<string> InitialContentChange;
        public event Action NewEvent;
        public event Action<string> OpenEvent;
        public event Action<string> SaveEvent;
        public event Action<int, int> SelectionEvent;
        public event Action WasSomethingChangedInDataEvent;

        public void ContentsKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void fileMenuNewWindowButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void getRowCol(string input, out int col, out int row)
        {
            throw new NotImplementedException();
        }

        public void HelpMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OpenMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OpenNew()
        {
            throw new NotImplementedException();
        }

        public void SaveMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void ssPanelCellChange(global::SSGui.SpreadsheetPanel sender)
        {
            throw new NotImplementedException();
        }
    }
}
