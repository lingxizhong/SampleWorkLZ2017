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


        public event Action<string> OpenEvent;

        public void FireOpenEvent(string fileName)
        {

            if (OpenEvent != null)
            {
                OpenEvent(fileName);
            }

        }


        public event Action<string> CellRecalcEvent;

        public void FireCellRecalcEvent(string s)
        {

            if (CellRecalcEvent != null)
            {
                CellRecalcEvent(s);
            }

        }


        public event Action<string> InitialContentChange;

        public void FireInitialContentChange(string s)
        {

            if (InitialContentChange != null)
            {
                InitialContentChange(s);
            }

        }

        public event Action NewEvent;

        public void FireNewEvent()
        {

            if (NewEvent != null)
            {
                NewEvent();
            }

        }
        public event Action<string> SaveEvent;

        public void FireSaveEvent(string s)
        {

            if (SaveEvent != null)
            {
                SaveEvent(s);
            }

        }
        public event Action<int, int> SelectionEvent;

        public void FireSelectionEvent(int row, int collumn)
        {

            if (SelectionEvent != null)
            {
                SelectionEvent(row, collumn);
            }

        }


        public event Action WasSomethingChangedInDataEvent;

        public void FireWasSomethingChangedInDataEvent()
        {

            if (WasSomethingChangedInDataEvent != null)
            {
                WasSomethingChangedInDataEvent();
            }

        }


        public string CellContents
        {
            get;
            set;
        }

        public IEnumerable<string> cellRecalc
        {
            get;
            set;
        }

        public string CellValue
        {
            get;
            set;
        }

        public Exception errorProperty
        {
            get;
            set;
        }

        public bool WasChanged
        {
            get;
            set;
        }


        public void OpenNew()
        {
           if(NewEvent != null)
            {
                NewEvent();
            }
        }
    }
}
