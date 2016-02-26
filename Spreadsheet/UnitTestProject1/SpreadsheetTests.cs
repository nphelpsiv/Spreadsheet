///Created by Neal Phelps u0669056 on 2/18/2016
///Updated By neal Phelps on 2/25/2016
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;
using System.Collections.Generic;
using System.IO;

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
            Spreadsheet ss = new Spreadsheet();
            ss.GetCellContents(null);
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException2()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.GetCellContents("A02");
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException3()
        {
           Spreadsheet ss = new Spreadsheet();
            ss.GetCellContents("2a");
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException4()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.GetCellContents("X1X");
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException5()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.GetCellContents("AXYAHNKD234902A8558");
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException6()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.GetCellContents("AXYAHNKD23490&8558");
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException18()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.GetCellValue(null);
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException19()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.GetCellValue("fniu4h&*Y*(&*(&U*UH#u904u");
        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestException7()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a1", null);
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException8()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell(null, "2");
        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestException9()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a1", null);
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException10()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("aff4fq4rffrf4", "3");
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException11()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell(null, "x5");
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException12()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("f454f", "x5");
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException13()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell(null, "hi");
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException14()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("fh87hfh4iu", "hi");
        }
        //test method contents
        [TestMethod()]
        public void TestGetCellContents()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Assert.AreEqual("", ss.GetCellContents("z1"));
        }
        [TestMethod()]
        public void TestSetContentsOfCellText()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a1", "a2");
            Assert.AreEqual("a2", ss.GetCellContents("a1"));
            ss.SetContentsOfCell("a1", "a3");
            Assert.AreEqual("a3", ss.GetCellContents("a1"));

        }
        [TestMethod()]
        public void TestSetContentsOfCellDouble()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a1", "2");
            Assert.AreEqual(2d, ss.GetCellContents("a1"));
            ss.SetContentsOfCell("a1", "7");
            Assert.AreEqual(7d, ss.GetCellContents("a1"));

        }
        [TestMethod()]
        public void TestSetContentsOfCellComplexFormula()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a1", "2");
            ss.SetContentsOfCell("b1", "=a1*2");
            ss.SetContentsOfCell("c1", "=b1+a1");
            ss.SetContentsOfCell("d1", "=b1+c1");
            HashSet<string> set = (HashSet<string>) ss.SetContentsOfCell("a1", "3");
            Assert.IsTrue(set.Contains("A1"));
            Assert.IsTrue(set.Contains("B1"));
            Assert.IsTrue(set.Contains("C1"));
            Assert.IsTrue(set.Contains("D1"));

        }
        [TestMethod()]
        public void TestSetContentsNewValue()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "3.0");
            ss.SetContentsOfCell("B1", "2.0");
            ss.SetContentsOfCell("C1", "4.0");

            Assert.AreEqual(3.0, ss.GetCellValue("A1"));
            Assert.AreEqual(2.0, ss.GetCellValue("B1"));
            Assert.AreEqual(4.0, ss.GetCellValue("C1"));

            ss.SetContentsOfCell("D1", "=A1*2.0");
            Assert.AreEqual(6.0, ss.GetCellValue("D1"));

            ss.SetContentsOfCell("F1", "=B1*C1");
            Assert.AreEqual(8.0, ss.GetCellValue("F1"));

            ss.SetContentsOfCell("E1", "=D1*C1");
            Assert.AreEqual(24.0, ss.GetCellValue("E1"));

            ss.SetContentsOfCell("G1", "=D1*E1");
            Assert.AreEqual(144.0, ss.GetCellValue("G1"));
        }
        [TestMethod()]
        public void TestSetContentsNewValueBadFormula()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "3.0");
            ss.SetContentsOfCell("B1", "2.0");
            ss.SetContentsOfCell("C1", "0");

            Assert.AreEqual(3.0, ss.GetCellValue("A1"));
            Assert.AreEqual(2.0, ss.GetCellValue("B1"));
            Assert.AreEqual(0.0, ss.GetCellValue("C1"));

            ss.SetContentsOfCell("D1", "=A1*2.0");
            Assert.AreEqual(6.0, ss.GetCellValue("D1"));

            ss.SetContentsOfCell("F1", "=B1/C1");
            Assert.AreEqual(new FormulaError(), ss.GetCellValue("F1"));

        }
        [TestMethod()]
        public void TestSave()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "3.0");
            ss.SetContentsOfCell("B1", "2.0");
            ss.SetContentsOfCell("C1", "4.0");

            Assert.AreEqual(3.0, ss.GetCellValue("A1"));
            Assert.AreEqual(2.0, ss.GetCellValue("B1"));
            Assert.AreEqual(4.0, ss.GetCellValue("C1"));

            ss.SetContentsOfCell("D1", "=A1*2.0");
            Assert.AreEqual(6.0, ss.GetCellValue("D1"));

            ss.SetContentsOfCell("F1", "=B1*C1");
            Assert.AreEqual(8.0, ss.GetCellValue("F1"));

            ss.SetContentsOfCell("E1", "=D1*C1");
            Assert.AreEqual(24.0, ss.GetCellValue("E1"));

            ss.SetContentsOfCell("G1", "=D1*E1");
            ss.SetContentsOfCell("F1", "hello");
            Assert.AreEqual(144.0, ss.GetCellValue("G1"));

            TextWriter tw = new StreamWriter("../../XMLFile1 - Copy.xml");
            ss.Save(tw);

            StreamReader sr = new StreamReader("../../XMLFile1 - Copy.xml");
            Spreadsheet s2 = new Spreadsheet(sr);

            Assert.AreEqual(3.0, s2.GetCellValue("A1"));
            Assert.AreEqual(2.0, s2.GetCellValue("B1"));
            Assert.AreEqual(4.0, s2.GetCellValue("C1"));
            Assert.AreEqual(24.0, s2.GetCellValue("E1"));
            Assert.AreEqual(144.0, s2.GetCellValue("G1"));
            Assert.AreEqual("hello", s2.GetCellValue("F1"));
        }
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestSaveException()
        {

            StreamReader sr = new StreamReader("../../XMLFileBad.xml");
            Spreadsheet s2 = new Spreadsheet(sr);


        }
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestSaveException1()
        {

            StreamReader sr = new StreamReader("../../XMLFileBadFormula.xml");
            Spreadsheet s2 = new Spreadsheet(sr);


        }
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestSaveException2()
        {

            StreamReader sr = new StreamReader("../../XMLFileCirc.xml");
            Spreadsheet s2 = new Spreadsheet(sr);


        }
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestSaveException3()
        {

            StreamReader sr = new StreamReader("../../XMLFileBadFormula - Copy.xml");
            Spreadsheet s2 = new Spreadsheet(sr);

        }

        //OLD Exception Tests
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestException20()
        {
            TesttableSpreadsheet ss = new TesttableSpreadsheet();
            ss.SetCellContents("a1", null);
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException21()
        {
            TesttableSpreadsheet ss = new TesttableSpreadsheet();
            ss.SetCellContents(null, 2);
        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestException22()
        {
            TesttableSpreadsheet ss = new TesttableSpreadsheet();
            ss.SetCellContents("a1", null);
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException23()
        {
            TesttableSpreadsheet ss = new TesttableSpreadsheet();
            ss.SetCellContents("aff4fq4rffrf4", 3);
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException24()
        {
            TesttableSpreadsheet ss = new TesttableSpreadsheet();
            ss.SetCellContents(null, "x5");
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException25()
        {
            TesttableSpreadsheet ss = new TesttableSpreadsheet();
            ss.SetCellContents("f454f", "x5");
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException26()
        {
            TesttableSpreadsheet ss = new TesttableSpreadsheet();
            ss.SetCellContents(null, "hi");
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException27()
        {
            TesttableSpreadsheet ss = new TesttableSpreadsheet();
            ss.SetCellContents("fh87hfh4iu", "hi");
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException28()
        {
            TesttableSpreadsheet ss = new TesttableSpreadsheet();
            ss.GetDirectDependents("fh87hfh4iu");
        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestException29()
        {
            TesttableSpreadsheet ss = new TesttableSpreadsheet();
            ss.GetDirectDependents(null);
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException30()
        {
            TesttableSpreadsheet ss = new TesttableSpreadsheet();
            ss.SetCellContents(null, new Formula("x+5"));
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestException31()
        {
            TesttableSpreadsheet ss = new TesttableSpreadsheet();
            ss.SetCellContents("fh87hfh4iu", new Formula("x+5"));
        }
    }
    public class TesttableSpreadsheet : Spreadsheet

    {

        public new ISet<string> SetCellContents(string name, string text)

        {

            return base.SetCellContents(name, text);

        }

        public new ISet<string> SetCellContents(string name, double number)

        {

            return base.SetCellContents(name, number);

        }

        public new ISet<string> SetCellContents(string name, Formula formula)

        {

            return base.SetCellContents(name, formula);

        }
        public new IEnumerable<string> GetDirectDependents(string name)

        {

            return base.GetDirectDependents(name);

        }

    }
}
