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
    public class Stub : Form, ISpreadsheet
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

        public void OpenNew()
        {
            throw new NotImplementedException();
        }
    }
}
