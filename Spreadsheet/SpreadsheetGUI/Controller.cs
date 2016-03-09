///Neal Phelps 3/8/2016 U0669056
using SSGui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Class for controlling a spreadsheetpanel window
    /// </summary>
    public class Controller
    {

        // The window being controlled
        private IAnalysisView window;

        // The model being used
        private Model model;

        /// <summary>
        /// Begins controlling window.
        /// </summary>
        public Controller(IAnalysisView window)
        {
            this.window = window;
            this.model = new Model();
            window.CellChanged += SetContents;
            window.CellClickEvent += UpdateBoxes;
            window.FileChosenEvent += HandleFileChosen;
            window.CloseEvent += HandleClose;
            window.NewEvent += HandleNew;
            window.SaveEvent += HandleSave;
            window.AboutEvent += HandleAbout;
        }


        /// <summary>
        /// Handles a request to open a file.
        /// </summary>
        private void HandleFileChosen(String filename, SpreadsheetPanel sp)
        {
            try
            {

                model.ReadFile(filename);
                window.Title = filename;
                HashSet<string> set = new HashSet<string>();

                set = model.NonEmptyCells();
                foreach (String s in set)
                {
                    this.SetValues(sp,s);
                }
            }
            catch (Exception ex)
            {
                window.Message = "Unable to open file\n" + ex.Message;
            }
        }

        /// <summary>
        /// Handles a request to save a file.
        /// </summary>
        private void HandleSave(String filename)
        {
            try
            {
                model.Save(filename);
                window.TestHelp4();
            }
            catch (Exception ex)
            {
                window.Message = "Unable to save file\n" + ex.Message;
            }
        }

        /// <summary>
        /// Handles a request to close the window
        /// </summary>
        private void HandleClose()
        {
            if (model.Changed())
            {
                var result = MessageBox.Show("You are about to exit without saving!" + "\n" + "Contine?", "WARNING!",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    window.DoClose();
                }
                else
                {

                }
            }
            else
            {
                window.DoClose();
            }
           
        }

        /// <summary>
        /// Handles a request to open a new window.
        /// </summary>
        private void HandleNew()
        {

            window.OpenNew();

        }
        /// <summary>
        /// Handle the about event and display the information about hte program
        /// </summary>
        private void HandleAbout()
        {
            MessageBox.Show("1. Input the desired content in a cell by clicking on it and then" + "\n" + "tryping a string, double or formula int the content box." + "\n" + "2. You must denote a formula by first type a equals sign," + "\n" + "otherwise it will be taken as a string." + "\n" + "3. If you try to use a double and string in a formula you will get a formula error." + "\n" + "4. If you input a incorrect cell name or value you will recieve an error message." + "\n" + "5. In order to open a previously saved spreadsheet, please open a new window first." , "ABOUT THIS PROGRAM");
            window.TestHelp();
        }
        /// <summary>
        /// Handles the button press of the set button and sets the oject in the contentBox to the correct cell
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="contents"></param>
        private void SetContents(SpreadsheetPanel sp, string contents)
        {
            window.TestHelp2();

            int row, col;
            String coordinate;
            HashSet<string> set = new HashSet<string>();
            sp.GetSelection(out col, out row);
            char c = (char) (col + 65);
            coordinate = c.ToString() + (row + 1).ToString();


            try {
                set = model.setContents(coordinate, contents);
            }
            catch (Exception e)
            {
                window.Message = "Error:" + " " + e.Message;
            }
            foreach(String s in set)
            {
                int newCol = (int)s.First<char>() - 65;
                int newRow;
                int.TryParse(s.Substring(1), out newRow);
                newRow = newRow - 1;

                sp.SetValue(newCol, newRow, model.getValue(s));


                window.valueBox = model.getValue(s);
                window.contentBox = model.getContents(s);
            }

            char ch = (char)(col + 65);
            String newCoordinate = c.ToString() + (row + 1).ToString();

            UpdateBoxes(sp, model.getContents(newCoordinate), model.getValue(newCoordinate), newCoordinate);

        }
        /// <summary>
        /// Handles events from clicking on any cells, updates the corresponding textBoxes
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="contents"></param>
        /// <param name="value"></param>
        /// <param name="name"></param>
        private void UpdateBoxes(SpreadsheetPanel sp, string contents, string value, string name)
        {
            window.TestHelp1();

            int row, col;
            String coordinate;

            sp.GetSelection(out col, out row);
            char c = (char)(col + 65);
            coordinate = c.ToString() + (row + 1).ToString();


            window.valueBox = model.getValue(coordinate);
            window.nameBox = coordinate;
            window.contentBox = model.getContents(coordinate);
        }
        /// <summary>
        /// Sets values for newly opened file.
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="s"></param>
        private void SetValues(SpreadsheetPanel sp, string s)
        {

            int newCol = (int)s.First<char>() - 65;
            int newRow;
            int.TryParse(s.Substring(1), out newRow);
            newRow = newRow - 1;


            sp.SetValue(newCol, newRow, model.getValue(s));

   
            window.valueBox = model.getValue(s);
            window.contentBox = model.getContents(s);
            window.nameBox = s;
            window.TestHelp3();
        }


    }
}

