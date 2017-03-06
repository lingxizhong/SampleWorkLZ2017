using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public partial class SpreadsheetGUI : Form
    {
        public SpreadsheetGUI()
        {
            InitializeComponent();
        }


        public event Action NewEvent;

        /// <summary>
        /// This is the New button in the File menu 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileMenuNewWindowButton_Click(object sender, EventArgs e)
        {
            if (NewEvent != null)
            {
                NewEvent();
            }
        }

        public void OpenNew()
        {
            SpreadsheetGUIApplicationContext.GetContext().RunNew();
        }
    }
}
