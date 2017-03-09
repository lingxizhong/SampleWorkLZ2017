using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public class SpreadsheetGUIApplicationContext : ApplicationContext
    {
        /// <summary>
        /// Number of open Forms
        /// </summary>
        private int windowCount = 0;

        /// <summary>
        /// Singleton ApplicationContext
        /// </summary>
        private static SpreadsheetGUIApplicationContext context;

        private SpreadsheetGUIApplicationContext()
        {
        }

        public static SpreadsheetGUIApplicationContext GetContext()
        {
            if (context == null)
            {
                context = new SpreadsheetGUIApplicationContext();
            }
            return context;
        }

        public void RunNew()
        {
            SpreadsheetGUI window = new SpreadsheetGUI();
            new Controller(window);
            // Window Counting Increment
            windowCount++;

            // Figure out wtf this line does:
            window.FormClosed += (o, e) => { if (--windowCount <= 0) ExitThread(); };
            // Runs the form?
            window.Show();
        }
    }
}
