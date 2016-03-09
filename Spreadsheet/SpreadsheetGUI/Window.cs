/// Neal Phelps 3/8/2016 U0669056
using SSGui;
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
    /// <summary>
    /// Partial class that fires events to the controller nad interacts with the model
    /// </summary>
    public partial class Window : Form, IAnalysisView
    {
        //Title
        public string Title
        {
            set
            {
                Text = value;
            }
        }
        //Variables for Messages from thrown exceptions
        public string Message
        {
            set
            {
                MessageBox.Show(value);
            }
        }
        //variable for setting the third textbox to the given string value
        public string contentBox
        {
            set
            {
                textBox3.Text = value.ToString();
            }
        }
        //variable for setting the first textbox to the given string value
        public string nameBox
        {
            set
            {
                textBox1.Text = value.ToString();
            }
        }
        //variable for setting the second textbox to the given string value
        public string valueBox
        {
            set
            {
                textBox2.Text = value.ToString();
            }
        }
        /// <summary>
        /// Window that fires events and interacts with the controller
        /// </summary>
        public Window()
        {
            InitializeComponent();
            spreadsheetPanel1.SelectionChanged += displaySelection;
        }

        //Events
        public event Action<string, SpreadsheetPanel> FileChosenEvent;
        public event Action CloseEvent;
        public event Action NewEvent;
        public event Action<SpreadsheetPanel, string> CellChanged;
        public event Action<SpreadsheetPanel, string, string, string> CellClickEvent;
        public event Action<string> SaveEvent;
        public event Action AboutEvent;

        /// <summary>
        /// Fires Cell Clicked events
        /// </summary>
        /// <param name="ss"></param>
        private void displaySelection(SpreadsheetPanel ss)
        {
            if (CellClickEvent != null)
            {
                CellClickEvent(spreadsheetPanel1, textBox1.Text, textBox2.Text, textBox3.Text);
            }
        }

        /// <summary>
        /// Fire to close program
        /// </summary>
        public void DoClose()
        {
            Close();
        }

        /// <summary>
        /// Fires event to pen new window
        /// </summary>
        public void OpenNew()
        {
            SpreadsheetApplicationContext.GetContext().RunNew();
        }

        /// <summary>
        /// Fires from the set button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Set_Click(object sender, EventArgs e)
        {
            if (CellChanged != null)
            {
                CellChanged(spreadsheetPanel1, textBox3.Text);
            }

        }

        /// <summary>
        /// Controled from drop down to open new SS file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //newToolStripMenuItem_Click(sender, e);
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (FileChosenEvent != null)
                {
                    FileChosenEvent(openFileDialog1.FileName, spreadsheetPanel1);
                }
            }
        }
        /// <summary>
        /// Fires event to open save dialog and save surrent spreadsheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (SaveEvent != null)
                {
                    SaveEvent(saveFileDialog1.FileName);
                }
            }
        }
        /// <summary>
        /// Fires event to open new window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NewEvent != null)
            {
                NewEvent();
            }
        }
        /// <summary>
        /// Fires event to close program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeCurrentWindowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CloseEvent != null)
            {
                CloseEvent();
            }
        }
        /// <summary>
        /// Fires Event for the about to help
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            if (AboutEvent != null)
            {
                AboutEvent();
            }
        }
        //unused GUI item interactions
        private void textBox2_TextChanged(object sender, EventArgs e)
        {


        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Method to help me test
        /// </summary>
        public void TestHelp()
        {

        }

        /// <summary>
        /// Method to help me test
        /// </summary>
        public void TestHelp1()
        {

        }

        /// <summary>
        /// Method to help me test
        /// </summary>
        public void TestHelp2()
        {

        }
        /// <summary>
        /// Method to help me test
        /// </summary>
        public void TestHelp3()
        {

        }
        /// <summary>
        /// Method to help me test
        /// </summary>
        public void TestHelp4()
        {

        }
    }
}