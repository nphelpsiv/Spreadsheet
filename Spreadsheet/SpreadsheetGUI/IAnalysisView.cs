///Neal Phelps 3/8/2016
using SSGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Interface to be used in Controller and window to allow interaction between the two
    /// </summary>
    public interface IAnalysisView
    {
        //Intitialize events
        event Action<string, SpreadsheetPanel> FileChosenEvent;

        event Action<string> SaveEvent;

        event Action<SpreadsheetPanel, string, string, string> CellClickEvent;

        event Action<SpreadsheetPanel, string> CellChanged;

        event Action CloseEvent;

        event Action NewEvent;

        event Action AboutEvent;

        //Title
        string Title { set; }

        //variable for setting the third textbox to the given string value
        string contentBox { set; }

        //variable for setting the first textbox to the given string value
        string nameBox { set; }

        //variable for setting the second textbox to the given string value
        string valueBox { set; }

        //Variables for Messages from thrown exceptions
        string Message { set; }

        /// <summary>
        /// Fire to close program
        /// </summary>
        void DoClose();

        /// <summary>
        /// Fires event to pen new window
        /// </summary>
        void OpenNew();

        /// <summary>
        /// Method to help me test
        /// </summary>
        void TestHelp();

        /// <summary>
        /// Method to help me test
        /// </summary>
        void TestHelp1();

        /// <summary>
        /// Method to help me test
        /// </summary>
        void TestHelp2();
        /// <summary>
        /// Method to help me test
        /// </summary>
        void TestHelp3();
        /// <summary>
        /// Method to help me test
        /// </summary>
        void TestHelp4();
    }
}
