using SpreadsheetGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSGui;

namespace ControllerTester
{

    /// <summary>
    /// Class created to aid testing the control class in the spreadsheet
    /// </summary>
    class Stub : IAnalysisView
    {
        public bool CalledDoClose
        {
            get; private set;
        }

        public bool CalledOpenNew
        {
            get; private set;
        }
        public bool CalledAbout
        {
            get; private set;
        }
        public bool CalledCellChange
        {
            get; private set;
        }
        public bool CalledCellClicked
        {
            get; private set;
        }
        public bool CalledFileChosen
        {
            get; private set;
        }
        public bool CalledSave
        {
            get; private set;
        }

        // These four methods cause events to be fired
        public void FireCloseEvent()
        {
            if (CloseEvent != null)
            {
                CloseEvent();
            }
        }

        public void FireAboutEvent()
        {
            if (AboutEvent != null)
            {
                AboutEvent();
            }
        }

        public void FireSaveEvent()
        {
            if (SaveEvent != null)
            {
                SaveEvent(".../.../Test.ss");
            }
        }

        public void FireFileChosenEvent(string filename, SpreadsheetPanel sp)
        {
            if (FileChosenEvent != null)
            {
                FileChosenEvent(filename, sp);
            }
        }

        public void FireClickedEvent(SpreadsheetPanel sp, string content, string value, string name)
        {
            if (CellClickEvent != null)
            {
                CellClickEvent(sp, content, value, name);
            }
        }

        public void FireCellEvent(SpreadsheetPanel sp, string content)
        {
            if (CellChanged != null)
            {
                CellChanged(sp, content);
            }
        }

        public void FireNewEvent()
        {
            if (NewEvent != null)
            {
                NewEvent();
            }
        }
        public string contentBox
        {
            set; get;
        }

        public string Message
        {
            set; get;
        }

        public string nameBox
        {
            set; get;
        }

        public string Title
        {
            set; get;
        }

        public string valueBox
        {
            set; get;
        }


        public event Action AboutEvent;
        public event Action<SpreadsheetPanel, string> CellChanged;
        public event Action<SpreadsheetPanel, string, string, string> CellClickEvent;
        public event Action CloseEvent;
        public event Action<string, SpreadsheetPanel> FileChosenEvent;
        public event Action NewEvent;
        public event Action<string> SaveEvent;

        public void DoClose()
        {
            CalledDoClose = true;
        }

        public void OpenNew()
        {
            CalledOpenNew = true;
        }
        public void TestHelp()
        {
            CalledAbout = true;
        }
        public void TestHelp1()
        {
            CalledCellClicked = true;
        }
        public void TestHelp2()
        {
            CalledCellChange = true;
        }
        public void TestHelp3()
        {
            CalledFileChosen = true;
        }
        public void TestHelp4()
        {
            CalledSave = true;
        }
    }
}
