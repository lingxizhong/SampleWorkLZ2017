using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Author: Lingxi Zhong U0770136 and Osama Kergaye PUT YOUR UID HERE
/// </summary>
namespace SpreadsheetGUI
{
    static class Launch
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var context = SpreadsheetGUIApplicationContext.GetContext();
            SpreadsheetGUIApplicationContext.GetContext().RunNew();
            Application.Run(context);
        }
    }
}
