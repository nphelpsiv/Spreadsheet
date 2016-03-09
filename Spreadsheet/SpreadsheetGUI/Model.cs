///Neal Phelps u0669056 3/8/2016
using SS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    class Model
    {
        //intitialize spreadsheet
        private Spreadsheet ss;

        /// <summary>
        /// Constructs a Model with empty spreadsheet
        /// </summary>
        public Model()
        {
            ss = new Spreadsheet();
        }

        /// <summary>
        /// Makes the contents of the named file the new value of contents
        /// </summary>
        public void ReadFile(string filename)
        {
            ss = new Spreadsheet(new StreamReader(filename));
        }
        /// <summary>
        /// Returns the number of chars in contents.
        /// </summary>
        public string getValue(String name)
        {
            return ss.GetCellValue(name).ToString();
        }
        /// <summary>
        /// Returns the number of chars in contents.
        /// </summary>
        public HashSet<string> setContents(String name, String contents)
        {
            HashSet<string> set = new HashSet<string>();
            set = (HashSet<string>) ss.SetContentsOfCell(name, contents);
            return set;
        }
        /// <summary>
        /// Returns the number of chars in contents.
        /// </summary>
        public string getContents(String name)
        {
            return ss.GetCellContents(name).ToString();
        }
        /// <summary>
        /// Get save method from spreadheet to save the file, send to controller
        /// </summary>
        /// <param name="filename"></param>
        public void Save(String filename)
        {
            ss.Save(new StreamWriter(filename));
        }
        /// <summary>
        /// Used to get names of all empty cells inorder to repopulated opened file
        /// </summary>
        /// <returns></returns>
        public HashSet<String> NonEmptyCells()
        {
            return (HashSet<string>) ss.GetNamesOfAllNonemptyCells();
        }
        /// <summary>
        /// Returns true if spreadsheet has been changed
        /// </summary>
        /// <returns></returns>
        public bool Changed()
        {
            return ss.Changed;
        }
    }
}
