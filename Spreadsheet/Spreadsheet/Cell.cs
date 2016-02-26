///created by Neal Phelps u0669056 on 2/18/2016
/// Updated by Neal Phelps on 2/25/2016
using Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS
{
    /// <summary>
    /// Class that creates a basic Cell to be implemented in a spreadsheet
    /// </summary>
    class Cell
    {
        private String name;
        private Object contents;
        private bool isEmpty = false;
        private Object value;
        /// <summary>
        /// 
        /// Constructer for a cell
        /// </summary>
        public Cell()
        {
        }

        /// <summary>
        /// Get value of cell
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            return value;

        }

        // If a cell's contents is a string, its value is that string.
        /// 
        /// If a cell's contents is a double, its value is that double.
        /// 
        /// If a cell's contents is a Formula, its value is either a double or a FormulaError.
        /// The value of a Formula, of course, can depend on the values of variables.  The value 
        /// of a Formula variable is the value of the spreadsheet cell it names (if that cell's 
        /// value is a double) or is undefined (otherwise).  If a Formula depends on an undefined
        /// variable or on a division by zero, its value is a FormulaError.  Otherwise, its value
        /// is a double, as specified in Formula.Evaluate.
        public void SetValue(Object newValue)
        {
                value = newValue;
        }
        /// <summary>
        /// Get contents of cell
        /// </summary>
        /// <returns></returns>
        public Object GetContents()
        {
            return contents;
        }

        /// <summary>
        /// Check if cell is empty
        /// </summary>
        /// <returns></returns>
        public bool IsCellEmpty()
        {
            if(GetContents().Equals(""))
            {
                isEmpty = true;
            }
            return isEmpty;
        }

        /// <summary>
        /// Set new contents for cell
        /// </summary>
        /// <param name="newContent"></param>
        public void SetContents(Object newContent)
        {
            contents = newContent;
            this.SetValue(contents);
        }
    }
}
