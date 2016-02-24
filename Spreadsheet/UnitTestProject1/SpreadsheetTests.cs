///Created by Neal Phelps u0669056 on 2/18/2016
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;
using System.Collections.Generic;

namespace SS
{
    [TestClass]
    public class SpreadsheetTests
    {
        //Test Expcetions thrown in each method
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.GetCellContents(null);
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException2()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.GetCellContents("A02");
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException3()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.GetCellContents("2a");
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException4()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.GetCellContents("X1X");
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException5()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.GetCellContents("AXYAHNKD234902A8558");
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException6()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.GetCellContents("AXYAHNKD23490&8558");
        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestException7()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetCellContents("a1", null);
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException8()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetCellContents(null, 2);
        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestException9()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetCellContents("a1", null);
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException10()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetCellContents("aff4fq4rffrf4", 3);
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException11()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetCellContents(null, new Formula("x5"));
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException12()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetCellContents("f454f", new Formula("x5"));
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException13()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetCellContents(null, "hi");
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException14()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetCellContents("fh87hfh4iu", "hi");
        }
        //test method contents
        [TestMethod()]
        public void TestGetCellContents()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Assert.AreEqual("", ss.GetCellContents("z1"));
        }
        [TestMethod()]
        public void TestSetCellContentsText()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetCellContents("a1", "a2");
            Assert.AreEqual("a2", ss.GetCellContents("a1"));
            ss.SetCellContents("a1", "a3");
            Assert.AreEqual("a3", ss.GetCellContents("a1"));

        }
        [TestMethod()]
        public void TestSetCellContentsDouble()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetCellContents("a1", 2);
            Assert.AreEqual(2d, ss.GetCellContents("a1"));
            ss.SetCellContents("a1", 7);
            Assert.AreEqual(7d, ss.GetCellContents("a1"));

        }
        [TestMethod()]
        public void TestSetCellContentsComplexFFormula()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetCellContents("a1", 2);
            ss.SetCellContents("b1", new Formula("a1*2"));
            ss.SetCellContents("c1", new Formula("b1+a1"));
            ss.SetCellContents("d1", new Formula("b1+c1"));
            ss.SetCellContents("z2", new Formula("z1+z11"));
            ss.SetCellContents("z2", new Formula("x5"));
            HashSet<string> set = (HashSet<string>) ss.SetCellContents("a1", 3);
            Assert.IsTrue(set.Contains("A1"));
            Assert.IsTrue(set.Contains("B1"));
            Assert.IsTrue(set.Contains("C1"));
            Assert.IsTrue(set.Contains("D1"));
            Assert.IsFalse(set.Contains("Z2"));
        }
    }
}
