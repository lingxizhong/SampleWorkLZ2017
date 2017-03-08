using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI 
{
    interface SpreadsheetInterface
    {
        /// <summary>
        /// New Button event
        /// </summary>
        event Action NewEvent;

        /// <summary>
        /// Event for textbox input
        /// </summary>
        event Action<string> InitialContentChange;

        /// <summary>
        /// Event for selecting something in the panel
        /// </summary>
        event Action<int, int> SelectionEvent;

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
        /// Row we are currently selected on 
        /// </summary>
        int row { set; }

        /// <summary>
        /// Column we are currently selected on 
        /// </summary>
        int column { set; }

        /// <summary>
        /// This is the New button in the File menu 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fileMenuNewWindowButton_Click(object sender, EventArgs e);
        

        /// <summary>
        /// Property for getting and setting the value of a cell. Reference property for the controller to use. 
        /// </summary>
        string CellValue { get; set; }

        /// <summary>
        /// Property for getting and setting contents of a cell. Reference property for the Controller to use. 
        /// </summary>
        string CellContents { get; set; }

        /// <summary>
        /// This is a IEnum property for populating cells in the spreadsheet that are not currently selected.
        /// </summary>
        IEnumerable<string> cellRecalc { get; set; }

        /// <summary>
        /// If this ain't null, we got a problem :)
        /// </summary>
        Exception errorProperty { get; set; }
        /// <summary>
        /// Method for the button "Open" in the File menu
        /// </summary>
        void OpenNew();

        /// <summary>
        /// Action Listener for when a cell in the panel is clicked. 
        /// </summary>
        /// <param name="sender"></param>
        void ssPanelCellChange(SSGui.SpreadsheetPanel sender);


        /// <summary>
        /// For when user enters input into content textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ContentsKeyPress(object sender, KeyPressEventArgs e);



        /// <summary>
        /// Helper method that converts string input cell names into columns and rows.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        void getRowCol(string input, out int col, out int row);


        /// <summary>
        /// Saves the current Spreadsheet into a file. Default (*.ss)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SaveMenuItem_Click(object sender, EventArgs e);


        /// <summary>
        /// Opens a file explorer, then opens the specified file into the current Spreadsheet GUI. 
        /// Defaults files to be in (*.ss) format. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OpenMenuItem_Click(object sender, EventArgs e);

        /// <summary>
        /// This is the "Help" button under the "File" Menu button
        /// Displays tips and instructions for using our Spreadsheet GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HelpMenuItem_Click(object sender, EventArgs e);
    }
}
