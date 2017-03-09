using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Author: Lingxi Zhong U0770136 and Osama Kergaye PUT YOUR UID HERE
/// </summary>
namespace SpreadsheetGUI 
{
    public interface ISpreadsheet
    {
        /// <summary>
        /// New Button event
        /// </summary>
        event Action NewEvent;

        /// <summary>
        /// Method for the button "Open" in the File menu
        /// </summary>
        void OpenNew();



        /// <summary>
        /// Event for selecting something in the panel
        /// </summary>
        event Action<int, int> SelectionEvent;

        /// <summary>
        /// Property for getting and setting the value of a cell. Reference property for the controller to use. 
        /// </summary>
        string CellValue { set; }

        /// <summary>
        /// Property for getting and setting contents of a cell. Reference property for the Controller to use. 
        /// </summary>
        string CellContents { set; }



        /// <summary>
        /// Event for textbox input
        /// </summary>
        event Action<string> InitialContentChange;

        /// <summary>
        /// Event for textbox input- Event that helps the panel change values of unselected cells 
        /// </summary>
        event Action<string> CellRecalcEvent;



        /// <summary>
        /// Event for the Save button
        /// </summary>
        event Action<string> SaveEvent;


        /// <summary>
        /// Event for the Open button
        /// </summary>
        event Action<string> OpenEvent;

        /// <summary>
        /// If this ain't null, we got a problem :)
        /// </summary>
        Exception errorProperty {  set; }

        /// <summary>
        /// This is a IEnum property for populating cells in the spreadsheet that are not currently selected.
        /// </summary>
        IEnumerable<string> cellRecalc {  set; }


        /// <summary>
        /// Updates a gui property if the spreadsheet was changed in any way.
        /// </summary>
        event Action WasSomethingChangedInDataEvent;

        /// <summary>
        /// changed property
        /// </summary>
        bool WasChanged { set; }






        
        



    }
}
