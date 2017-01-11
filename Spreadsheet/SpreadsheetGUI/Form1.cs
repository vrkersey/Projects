using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SS;
using SpreadsheetUtilities;

namespace SpreadsheetGUI
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Initialize a new form
        /// </summary>
        /// <param name="FileName">"" if new form</param>
        public Form1(string FileName)
        {
            
            InitializeComponent();

            Spreadsheet ss;
            if (FileName == "")
                ss = new Spreadsheet(validator, s => s.ToUpper(), "ps6");
            else
            {
                ss = new Spreadsheet(FileName, validator, s => s.ToUpper(), "ps6");
                this.Text = FileName.Substring(FileName.LastIndexOf("\\") + 1);
            }

            spreadsheetPanel1.setSS(ss);
            spreadsheetPanel1.setSelectedCell("A1");
            spreadsheetPanel1.SetSelection(0, 0);

            // Adding event handlers to Form items
            // Some of the handlers take in additional variables to use
            spreadsheetPanel1.SelectionChanged += displaySelection; 
            newMenuItem.Click += newMenuItem_Click;
            closeMenuItem.Click += closeMenuItem_Click;
            openMenuItem.Click += (sender, e) => openMenuItem_Click(sender,e,spreadsheetPanel1, ref FileName);
            saveMenuItem.Click += (sender, e) => saveMenuItem_Click(sender, e, spreadsheetPanel1, ref FileName);
            saveAsMenuItem.Click += (sender, e) => saveAsMenuItem_Click(sender, e, spreadsheetPanel1,  ref FileName);
            cancelBtn.Click += (sender,e) => cancelBtn_Click(sender,e,spreadsheetPanel1);
            goBtn.Click += (sender,e) => goBtn_Click(sender,e, spreadsheetPanel1);
            OpeningSpreadsheet.DoWork += new System.ComponentModel.DoWorkEventHandler((sender, e) => OpenSpreadsheet_Worker(sender, e, spreadsheetPanel1));
            KeyDown += new System.Windows.Forms.KeyEventHandler((sender,e) => Form1_KeyDown(sender,e,spreadsheetPanel1));
            FormClosing += new System.Windows.Forms.FormClosingEventHandler((sender, e) => Form1_FormClosing(sender, e, spreadsheetPanel1));
            ContentTxtBox.TextChanged += new System.EventHandler((sender, e) => ContentTxtBox_TextChanged(sender,e, spreadsheetPanel1));

            // This needs to be set to true allow the arrow keys to work
            this.KeyPreview = true; 

            // run a worker thread to load all the cells that may contain data
            OpeningSpreadsheet.RunWorkerAsync();
           
            ContentTxtBox.Focus();
        }

        // Finished Helper Methods
        #region Helper Methods

        //Helper Methods for opening an existing spreadsheet
        private void updateALLcells(SpreadsheetPanel sspanel)
        {
            // loop through all nonempy cells and set the corresponding spreadsheet cell to the correct value 
            foreach (string s in sspanel.getSS().GetNamesOfAllNonemptyCells())
            {
                int col, row;
                cellAddress(s, out col, out row);
                if (object.ReferenceEquals(sspanel.getSS().GetCellValue(s).GetType(), typeof(FormulaError)))
                    sspanel.SetValue(col, row, "=" + sspanel.getSS().GetCellContents(s).ToString());
                else
                    sspanel.SetValue(col, row, sspanel.getSS().GetCellValue(s).ToString());
            }
        }

        // Method for updating all cells effected by changing a cell
        private void updateCells(SpreadsheetPanel sspanel, string startingCell, string startingCellValue)
        {
            int col, row;
            cellAddress(startingCell, out col, out row);
            //catches a bug where if you set the cell = to itself
            if (startingCellValue.Count() > 0 && startingCellValue.Substring(1).ToUpper() == startingCell)
            {
                MessageBox.Show("Formula Formatting Error: You're Dumb.. Can't set the contents of a cell to itself");
                spreadsheetPanel1.SetValue(col, row, "");
                sspanel.getSS().SetContentsOfCell(startingCell, "");
                ContentTxtBox.Text = "";
            }
            // don't want to update any cells containing ""
            else //if (startingCellValue != "")
            {
                // update the set
                foreach (string s in sspanel.getSS().SetContentsOfCell(startingCell, startingCellValue))
                {
                    cellAddress(s, out col, out row);
                    if (object.ReferenceEquals(sspanel.getSS().GetCellValue(s).GetType(), typeof(FormulaError)))
                        sspanel.SetValue(col, row, "=" + sspanel.getSS().GetCellContents(s).ToString());
                    else
                        sspanel.SetValue(col, row, sspanel.getSS().GetCellValue(s).ToString());
                }

                // adds a * to the form title when the spreadsheet has been modified
                if (sspanel.getSS().Changed == true && this.Text.Substring(this.Text.Count() - 1) != "*")
                    this.Text += " *";
            }
        }

        // converts a 0 based row and column to string
        // ex. cellName(0,0) = "A1"
        private string cellName(int col, int row)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((char)(65 + col));
            sb.Append((row + 1));
            return sb.ToString();
        }

        // converts a string cell name to a 0 based row and column
        private bool cellAddress(string s, out int col, out int row)
        {
            string cell = s.ToUpper();
            try
            {
                col = (int)cell[0] - 65;
                int.TryParse(cell.Substring(1), out row);
                row--;
                return true;
            }
            catch
            {
                col = -1;
                row = -1;
                return false;
            }
        }

        // Helper Method for determining if a cell is valid
        // Cells can only be valid if they are [A-Z][1-99]
        private bool validator(string cell)
        {
            int col, row;
            cellAddress(cell, out col, out row);

            if (col >= 0 && col < 26 && row >= 0 && row <= 100)
                return true;
            return false;
        }

        #endregion

        // Finished Event Handlers
        #region Event Handlers

        // Event Handler for changing cells
        // added an additional parameter for keeping track of where the cell is and has been
        private void displaySelection(SpreadsheetPanel sender)
        {
            goBtn.Focus();
            int row, col;
            string value;
            sender.GetSelection(out col, out row);

            string selectedCell = sender.getSelectedCell();

            // Old cell should be updated in the spreadsheet
            cellAddress(selectedCell, out col, out row); //retrieve the row and col of previous selected cell
            sender.GetValue(col, row, out value); //retrieve the value of previous selected cell

            // makes sure formulas are formatted correctly
            try
            {
                updateCells(sender, selectedCell, value);
                //selectedCell should be updated.
                sender.GetSelection(out col, out row); //retrieve the row and col of newly selected cell
                sender.setSelectedCell(cellName(col, row)); //set the name of the newly selected cell

                // Should change the value of ValueTxtBox and ContentTxtBox
                selectedCell = sender.getSelectedCell();
                ValueTxtBox.Text = sender.getSS().GetCellValue(selectedCell).ToString();
                ContentTxtBox.Text = object.ReferenceEquals(sender.getSS().GetCellContents(selectedCell).GetType(), typeof(Formula)) ?
                    "=" + sender.getSS().GetCellContents(selectedCell).ToString() : sender.getSS().GetCellContents(selectedCell).ToString();

            }
            catch (Exception e) //failed to update the cells
            {
                sender.SetSelection(col, row);
                MessageBox.Show("Formula Formatting Error: " + e.Message);
            }
            ContentTxtBox.Focus();
        }

        //Event Handler for when text changes in the ContentTxtBox
        //Actively changes the selectedcell's text as text is typed into the contenttxtbox
        // added an additional parameter for 'Piping' in the active spreadsheetPanel
        private void ContentTxtBox_TextChanged(object sender, EventArgs e, SpreadsheetPanel panel)
        {
            int col, row;
            panel.GetSelection(out col, out row);
            string selectedCell = cellName(col, row);
            cellAddress(selectedCell, out col, out row);
            panel.SetValue(col, row, ContentTxtBox.Text);
        }

        // Event Handler for go button
        // Go Button just sets the cells value to whatever is in the contents Text Box
        // added an additional parameter for 'Piping' in the active spreadsheetPanel
        private void goBtn_Click(object sender, EventArgs e, SpreadsheetPanel panel)
        {
            int col, row;
            panel.GetSelection(out col, out row);
            string selectedCell = cellName(col, row);
            cellAddress(selectedCell, out col, out row);
            try
            {
                updateCells(panel, selectedCell, ContentTxtBox.Text);
                ValueTxtBox.Text = panel.getSS().GetCellValue(selectedCell).ToString();
                string value;
                panel.GetValue(col, row, out value);
                ContentTxtBox.Text = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Formula Formatting Error: " + ex.Message);
            }
            ContentTxtBox.Focus();
        }

        // Event Handler for cancel button
        // Cancel Button reverts the cell back to whatever it was before the edit
        // added an additional parameter for 'Piping' in the selectedCell
        private void cancelBtn_Click(object sender, EventArgs e, SpreadsheetPanel panel)
        {
            object value = panel.getSS().GetCellContents(panel.getSelectedCell());
            if (object.ReferenceEquals(value.GetType(), typeof(Formula)))
                ContentTxtBox.Text = "=" + value.ToString();
            else
                ContentTxtBox.Text = value.ToString();
            ContentTxtBox.Focus();
        }

        //Always happens as Form1 tries to close
        private void Form1_FormClosing(object sender, FormClosingEventArgs e, SpreadsheetPanel panel)
        {
            if (panel.getSS().Changed == true)
            {
                DialogResult r = MessageBox.Show("Do you want to save your changes?", "Save?", MessageBoxButtons.YesNoCancel);
                if (r == DialogResult.Yes)
                {
                    saveMenuItem.PerformClick();
                }
                else if (r == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        //Event Handler for Close Spreadsheet
        private void closeMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Event Handler for New Spreadsheet
        private void newMenuItem_Click(object sender, EventArgs e)
        {
            SSApplicationContext.getAppContext().RunForm(new Form1(""));
        }

        //Event Handler for opening an existing spreadsheet
        // added an additional parameter for 'Piping' in the FileName
        private void openMenuItem_Click(object sender, EventArgs e, SpreadsheetPanel panel, ref string FileName)
        {
            OpenFileDialog newDialog = new OpenFileDialog();

            newDialog.Filter = "Spreadsheet files (*.sprd)|*.sprd|All files (*.*)|*.*";
            if (newDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (panel.getSS().Changed)
                    {
                        // ask if the spreadsheet should be overridden
                        DialogResult r = MessageBox.Show("Do you want to save your changes?", "Save?", MessageBoxButtons.YesNoCancel);
                        if (r == DialogResult.Yes)
                        {
                            saveMenuItem.PerformClick();

                            foreach (string cell in panel.getSS().GetNamesOfAllNonemptyCells())
                            {
                                int col, row;
                                cellAddress(cell, out col, out row);
                                panel.SetValue(col, row, "");
                            }

                            FileName = newDialog.FileName; //updating the FileName
                            panel.setSS(new Spreadsheet(FileName, validator, s => s.ToUpper(), "ps6"));
                            OpeningSpreadsheet.RunWorkerAsync();
                            this.Text = FileName.Substring(FileName.LastIndexOf("\\") + 1);

                        }
                        else if (r == DialogResult.No)
                        {
                            
                            foreach (string cell in panel.getSS().GetNamesOfAllNonemptyCells())
                            {
                                int col, row;
                                cellAddress(cell, out col, out row);
                                panel.SetValue(col, row, "");
                            }

                            FileName = newDialog.FileName; //updating the FileName
                            panel.setSS(new Spreadsheet(FileName, validator, s => s.ToUpper(), "ps6"));
                            OpeningSpreadsheet.RunWorkerAsync();
                            this.Text = FileName.Substring(FileName.LastIndexOf("\\") + 1);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Unable to open file");
                }
            }
        }

        // DoWork EventHandler for when a spreadsheet it opened
        private void OpenSpreadsheet_Worker(object sender, DoWorkEventArgs e, SpreadsheetPanel panel)
        {
            updateALLcells(panel);
        }

        // Event handler for save menu button
        // added an additional parameter for 'Piping' in the FileName
        private void saveMenuItem_Click(object sender, EventArgs e, SpreadsheetPanel panel ,ref string FileName)
        {
            if (FileName == "")
                saveAsMenuItem.PerformClick();
            else
            {
                panel.getSS().Save(FileName);
                this.Text = FileName.Substring(FileName.LastIndexOf("\\") + 1);
            }
        }

        // Event Handler for Saving the spreadsheet as a new .sprd file.
        // added an additional parameter for 'Piping' in the FileName
        private void saveAsMenuItem_Click(object sender, EventArgs e, SpreadsheetPanel panel, ref string FileName)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Spreadsheet files (*.sprd)|*.sprd";
            saveFileDialog1.Title = "Save Spreadsheet File";
            saveFileDialog1.ShowDialog();

            // This try block will catch an error caused by pressing cancel 
            // instead of saving.
            try
            {
                panel.getSS().Save(saveFileDialog1.FileName);
                FileName = saveFileDialog1.FileName;
                this.Text = FileName.Substring(FileName.LastIndexOf("\\") + 1);
            }
            catch (SS.SpreadsheetReadWriteException)
            {
                saveFileDialog1.Dispose();
            }
        }

        //Event Handler for when the Help Menu Item is clicked
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("• Moving the selected cell: \n 1) Click the cell \n 2) Use Arrow Keys \n 3) Use the Tab and Enter keys. \n \n" +
                "• File Menu: \n 1) New: Opens a new untitled Spreadsheet. \n 2) Open: Open an existing Spreadsheet (.sprd) file. \n" +
                " 3) Save: Saves the opened file or opens a Save As dialogue box if the current \t Spreadsheet is unnamed. \n 4) Save As: Opens a new Spreadsheet dialogue box. \n" +
                " 5) Close: Exits the Spreadsheet. \n \n" +
                "Additional Features: \n" +
                "• Ctrl+S saves the file \n" +
                "• Arrow Keys move the selected cell \n" +
                "• Enter, Tab to move the selected Cell \n" +
                "• Get the Filename to display as the window's title \n" +
                "• Show the star next the the filename when ss.Changed == true; \n" +
                "• Background image",
                "Help");
        }

        // This will override the tab key's default function
        // So that we can use it to move cells
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Tab) return false;
            if (keyData == (Keys.Shift | Keys.Tab)) return false;
            return base.ProcessDialogKey(keyData);
        }

        // Additional Functionality for transversing the spreadsheet panel
        // added an additional parameter for 'Piping' in the selecedCell
        private void Form1_KeyDown(object sender, KeyEventArgs e, SpreadsheetPanel panel)
        {
            int col, row;
            string selectedCell = panel.getSelectedCell();
            cellAddress(selectedCell, out col, out row);

            // This will prevent the beeping noise when pressing the Enter or Tab key.
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab || Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.S)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            //ctrl+S to save
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.S)
            {
                saveMenuItem.PerformClick();
            }

            //ctrl+N to open new spreadsheet
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.N)
            {
                newMenuItem_Click(sender, e);
            }

            //Moving around the spreadsheet panel
            if (e.KeyCode == Keys.Left || Control.ModifierKeys == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                if (col - 1 >= 0)
                {
                    panel.SetSelection(col - 1, row);
                    displaySelection(panel);
                }
            }
            else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Tab)
            {
                if (col + 1 < 26)
                {
                    panel.SetSelection(col + 1, row);
                    displaySelection(panel);
                }
            }
            else if (e.KeyCode == Keys.Up || Control.ModifierKeys == Keys.Shift && e.KeyCode == Keys.Return)
            {
                if (row - 1 >= 0)
                {
                    panel.SetSelection(col, row - 1);
                    displaySelection(panel);
                }
            }
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Return)
            {
                if (row + 1 < 100)
                {
                    panel.SetSelection(col, row + 1);
                    displaySelection(panel);
                }
            }
        }
        #endregion

    }
    /// <summary>
    /// adds some extentions to spreadsheetPanel
    /// </summary>
    static class spreadsheetPanelExtention
    {
        private static Spreadsheet ss;
        private static string sCell;

        public static Spreadsheet getSS(this SpreadsheetPanel panel)
        {
            return ss;
        }
        public static void setSS(this SpreadsheetPanel panel, Spreadsheet spreadsheet)
        {
            ss = spreadsheet;
        }

        public static void setSelectedCell(this SpreadsheetPanel panel, string SelectedCell)
        {
            sCell = SelectedCell;
        }
        public static string getSelectedCell(this SpreadsheetPanel panel)
        {
            return sCell;
        }
    }
}
