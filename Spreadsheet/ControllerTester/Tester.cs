using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetGUI;
using SSGui;

namespace ControllerTester
{
    /// <summary>
    /// Test class to test the controll class
    /// </summary>
    [TestClass]
    public class Tester
    {
        [TestMethod]
        public void TestClose()
        {
            Stub stub = new Stub();
            Controller controller = new Controller(stub);
            stub.FireCloseEvent();
            Assert.IsTrue(stub.CalledDoClose);
        }
        [TestMethod]
        public void TestAbout()
        {
            Stub stub = new Stub();
            Controller controller = new Controller(stub);
            SpreadsheetPanel sp = new SpreadsheetPanel();
            stub.FireAboutEvent();
            Assert.IsTrue(stub.CalledAbout);
        }
        [TestMethod]
        public void TestNew()
        {
            Stub stub = new Stub();
            Controller controller = new Controller(stub);
            stub.FireNewEvent();
            Assert.IsTrue(stub.CalledOpenNew);
        }
        [TestMethod]
        public void TestCellChanged()
        {
            Stub stub = new Stub();
            Controller controller = new Controller(stub);
            SpreadsheetPanel sp = new SpreadsheetPanel();
            stub.FireCellEvent(sp, "1");
            Assert.IsTrue(stub.CalledCellChange);
        }
        [TestMethod]
        public void TestCellClicked()
        {
            Stub stub = new Stub();
            Controller controller = new Controller(stub);
            SpreadsheetPanel sp = new SpreadsheetPanel();
            stub.FireClickedEvent(sp, "1", "", "");
            Assert.IsTrue(stub.CalledCellClicked);
        }
        [TestMethod]
        public void TestFile()
        {
            Stub stub = new Stub();
            Controller controller = new Controller(stub);
            SpreadsheetPanel sp = new SpreadsheetPanel();
            stub.FireFileChosenEvent(".../.../Hello.ss", sp);
            Assert.IsTrue(stub.CalledFileChosen);
        }
        [TestMethod]
        public void TestSave()
        {
            Stub stub = new Stub();
            Controller controller = new Controller(stub);
            stub.FireSaveEvent();
            Assert.IsTrue(stub.CalledSave);
        }
    }
}
