namespace SpreadsheetGUI
{
    partial class SpreadsheetGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileMenuNewWindowButton = new System.Windows.Forms.ToolStripMenuItem();
            this.spreadsheetPanel = new SSGui.SpreadsheetPanel();
            this.CellNameTextBox = new System.Windows.Forms.TextBox();
            this.ValueTextBox = new System.Windows.Forms.TextBox();
            this.ContentsTextBox = new System.Windows.Forms.TextBox();
            this.StaticCellNameTextBox = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1547, 49);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuNewWindowButton});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(75, 45);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // fileMenuNewWindowButton
            // 
            this.fileMenuNewWindowButton.Name = "fileMenuNewWindowButton";
            this.fileMenuNewWindowButton.Size = new System.Drawing.Size(193, 46);
            this.fileMenuNewWindowButton.Text = "New";
            this.fileMenuNewWindowButton.Click += new System.EventHandler(this.fileMenuNewWindowButton_Click);
            // 
            // spreadsheetPanel
            // 
            this.spreadsheetPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel.AutoScroll = true;
            this.spreadsheetPanel.Location = new System.Drawing.Point(0, 204);
            this.spreadsheetPanel.Name = "spreadsheetPanel";
            this.spreadsheetPanel.Size = new System.Drawing.Size(1547, 576);
            this.spreadsheetPanel.TabIndex = 2;
            this.spreadsheetPanel.SelectionChanged += new SSGui.SelectionChangedHandler(this.ssPanelCellChange);
            // 
            // CellNameTextBox
            // 
            this.CellNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.CellNameTextBox.Location = new System.Drawing.Point(76, 70);
            this.CellNameTextBox.Name = "CellNameTextBox";
            this.CellNameTextBox.ReadOnly = true;
            this.CellNameTextBox.Size = new System.Drawing.Size(106, 38);
            this.CellNameTextBox.TabIndex = 3;
            // 
            // ValueTextBox
            // 
            this.ValueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ValueTextBox.Location = new System.Drawing.Point(378, 70);
            this.ValueTextBox.Name = "ValueTextBox";
            this.ValueTextBox.ReadOnly = true;
            this.ValueTextBox.Size = new System.Drawing.Size(1128, 38);
            this.ValueTextBox.TabIndex = 4;
            // 
            // ContentsTextBox
            // 
            this.ContentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ContentsTextBox.Location = new System.Drawing.Point(141, 131);
            this.ContentsTextBox.Name = "ContentsTextBox";
            this.ContentsTextBox.Size = new System.Drawing.Size(1365, 38);
            this.ContentsTextBox.TabIndex = 5;
            this.ContentsTextBox.TextChanged += new System.EventHandler(this.contentBoxChange);
            // 
            // StaticCellNameTextBox
            // 
            this.StaticCellNameTextBox.Location = new System.Drawing.Point(13, 70);
            this.StaticCellNameTextBox.Name = "StaticCellNameTextBox";
            this.StaticCellNameTextBox.ReadOnly = true;
            this.StaticCellNameTextBox.Size = new System.Drawing.Size(57, 38);
            this.StaticCellNameTextBox.TabIndex = 6;
            this.StaticCellNameTextBox.Text = "Cell";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(294, 70);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(78, 38);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = "Value";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(13, 131);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(122, 38);
            this.textBox2.TabIndex = 8;
            this.textBox2.Text = "Contents";
            // 
            // SpreadsheetGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1547, 792);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.StaticCellNameTextBox);
            this.Controls.Add(this.ContentsTextBox);
            this.Controls.Add(this.ValueTextBox);
            this.Controls.Add(this.CellNameTextBox);
            this.Controls.Add(this.spreadsheetPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SpreadsheetGUI";
            this.Text = "Spreadsheet";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileMenuNewWindowButton;
        private SSGui.SpreadsheetPanel spreadsheetPanel;
        private System.Windows.Forms.TextBox CellNameTextBox;
        private System.Windows.Forms.TextBox ValueTextBox;
        private System.Windows.Forms.TextBox ContentsTextBox;
        private System.Windows.Forms.TextBox StaticCellNameTextBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
    }
}

