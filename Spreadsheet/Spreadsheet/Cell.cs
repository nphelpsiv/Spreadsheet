///created by Neal Phelps u0669056 on 2/18/2016
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
        /// <summary>
        /// 
        /// Constructer for a cell
        /// </summary>
        public Cell()
        {
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
        }
    }
}
