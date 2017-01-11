//Victor Kersey
//9-29-2016
//CS 3500

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using SpreadsheetUtilities;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Test to make sure initialization and getCellContents is blank when nothing is in a cell
        /// </summary>
        [TestMethod]
        public void EmptyCellTest()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Assert.AreEqual(ss.GetCellContents("a5"), "");
        }

        /// <summary>
        /// testing to make sure the contents of cell are correct
        /// </summary>
        [TestMethod]
        public void CellContentsTest1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a3", "Hello");
            Assert.AreEqual(ss.GetCellContents("a3"), "Hello");
        }

        /// <summary>
        /// testing that the set returned by SetContentsOfCell is correct
        /// </summary>
        [TestMethod]
        public void CellContentsTest2()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            foreach (string el in ss.SetContentsOfCell("a3", "Hello"))
                Assert.AreEqual(el, "a3");
        }

        /// <summary>
        /// testing that the set returned by SetContentsOfCell is correct
        /// </summary>
        [TestMethod]
        public void returedSetTest()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            List<string> container = new List<string>();
            container.Add("a1");
            container.Add("a2");
            container.Add("a3");
            foreach (string el in ss.SetContentsOfCell("a3", "=a1+a2"))
                Assert.IsTrue(container.Contains(el));
        }

        /// <summary>
        /// make sure old cell contents are cleared before adding new contents
        /// </summary>
        [TestMethod]
        public void replaceContentsFormulaTest()
        {
            List<string> container = new List<string>();
            container.Add("a1");
            container.Add("a2");
            container.Add("a3");
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a3", "=b1+b2");
            foreach (string el in ss.SetContentsOfCell("a3", "=a1+a2"))
                Assert.IsTrue(container.Contains(el));
        }

        /// <summary>
        /// checking getNamesOfAllNonemptyCells
        /// </summary>
        [TestMethod]
        public void nonemptyCellsTest()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a3", "Hello World");
            foreach (string el in ss.GetNamesOfAllNonemptyCells())
                Assert.AreEqual(el, "a3");
        }

        [TestMethod]
        public void replaceContentsTextTest()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a3", "=b1+b2");
            ss.SetContentsOfCell("a3", "Hello");
            Assert.AreEqual(ss.GetCellContents("a3"), "Hello");
        }

        [TestMethod]
        public void replaceContentsDoubleTest()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a3", "=b1+b2");
            ss.SetContentsOfCell("a3", "3.5");
            Assert.AreEqual(ss.GetCellContents("a3"), 3.5);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void getCellContentsNullTest()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.GetCellContents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void getCellContentsInvalidTest()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Assert.AreEqual(ss.GetCellContents("5a"),"");
        }

        /// <summary>
        /// SetContentsOfCell(name,formula) exception tests
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellFormulaNull1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell(null, "=a1+1");
        }

        /// <summary>
        /// SetContentsOfCell(name,formula) exception tests
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellFormulaInvalid()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("5a", "=a1+1");
        }

        /// <summary>
        /// SetContentsOfCell(name,formula) exception tests
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetContentsOfCellFormulaNull2()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a3",null);
        }

        /// <summary>
        /// SetContentsOfCell(name,text) exception tests
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellTextNull1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell(null, "Hello World");
        }

        /// <summary>
        /// SetContentsOfCell(name,text) exception tests
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellTextInvalid()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a5a", "Hello World");
        }

        /// <summary>
        /// SetContentsOfCell(name,text) exception tests
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetContentsOfCellTextNull2()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("_a3", (string)null);
        }

        /// <summary>
        /// SetContentsOfCell(name,double) exception tests
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellDoubleNull()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell(null, "3.5");
        }

        /// <summary>
        /// SetContentsOfCell(name,double) exception tests
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellDoubleInvalid()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a15a", "3.5");
        }

        /// <summary>
        /// Test for circular dependencies
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CircularExceptionTest()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a1", "5");
            ss.SetContentsOfCell("a2", "=a1+1");
            ss.SetContentsOfCell("a3", "=a2+1");
            ss.SetContentsOfCell("a1", "=a3+1");
        }

        /// <summary>
        /// evaluates the value of a cell
        /// </summary>
        [TestMethod]
        public void valueTest1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A2", "3");
            s.SetContentsOfCell("A3", "=A4");
            s.SetContentsOfCell("A4", "6");
            Assert.AreEqual(s.GetCellValue("A1"),9.0);
        }

        /// <summary>
        /// evaluates the value of a cell
        /// </summary>
        [TestMethod]
        public void valueTest2()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=a2");
            s.SetContentsOfCell("a2", "=a3");
            s.SetContentsOfCell("a3", "=a4");
            s.SetContentsOfCell("a4", "=a5");
            s.SetContentsOfCell("a5", "=a6");
            s.SetContentsOfCell("a6", "=a7");
            s.SetContentsOfCell("a7", "=a8 + a8");
            s.SetContentsOfCell("a8", "5");
            Assert.AreEqual(s.GetCellValue("a1"),10.0);
        }

        /// <summary>
        /// make sure a Formula Error gets returned
        /// </summary>
        [TestMethod]
        public void FormulaErrorTest1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "something");
            s.SetContentsOfCell("a2", "2");
            s.SetContentsOfCell("a3", "3");
            s.SetContentsOfCell("a4", "4");
            s.SetContentsOfCell("a5", "=a1*a2");
            s.SetContentsOfCell("a6", "=a3*a4");
            s.SetContentsOfCell("a7", "=a5*a6");
            Assert.AreEqual(s.GetCellValue("a7").GetType(),typeof(FormulaError));
        }

        /// <summary>
        /// Make sure a formula error isn't returned 
        /// </summary>
        [TestMethod]
        public void FormulaErrorTest2()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "something");
            s.SetContentsOfCell("a2", "2");
            s.SetContentsOfCell("a3", "3");
            s.SetContentsOfCell("a4", "4");
            s.SetContentsOfCell("a5", "=a1*a2");
            s.SetContentsOfCell("a6", "=a3*a4");
            s.SetContentsOfCell("a7", "=a5*a6");
            Assert.AreEqual(s.GetCellValue("a6"), 12.0);
        }

        /// <summary>
        /// saves the spreadsheet to a file
        /// </summary>
        [TestMethod]
        public void saveTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=a2");
            s.SetContentsOfCell("a2", "=a3");
            s.SetContentsOfCell("a3", "=a4");
            s.SetContentsOfCell("a4", "=a5");
            s.SetContentsOfCell("a5", "=a6");
            s.SetContentsOfCell("a6", "=a7");
            s.SetContentsOfCell("a7", "=a8 + a8");
            s.SetContentsOfCell("a8", "5");
            s.Save("SaveTest.xml");
        }

        /// <summary>
        /// load the spreadsheet from a file
        /// </summary>
        [TestMethod]
        public void loadTest()
        {
            saveTest();
            AbstractSpreadsheet s = new Spreadsheet("SaveTest.xml", a => true, a => a, "default");
        }

        /// <summary>
        /// load the spreadsheet and make sure the value is able to be returned
        /// </summary>
        [TestMethod]
        public void loadTest2()
        {
            saveTest();
            AbstractSpreadsheet s = new Spreadsheet("SaveTest.xml", a => true, a => a, "default");
            Assert.AreEqual(s.GetCellValue("a1"), 10.0);
        }

        /// <summary>
        /// test the changed property
        /// </summary>
        [TestMethod]
        public void changedTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=a2");
            Assert.IsTrue(s.Changed);
        }

        /// <summary>
        /// test GetSavedVersion
        /// </summary>
        [TestMethod]
        public void versionTest()
        {
            saveTest();
            AbstractSpreadsheet s = new Spreadsheet();
            Assert.AreEqual(s.GetSavedVersion("SaveTest.xml"), "default");
        }

        /// <summary>
        /// simulate a fail to save
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void saveTestFailed()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "5");
            s.Save("save\test.xml");
        }

        /// <summary>
        /// simulate a fail to load
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void loadTestFailed()
        {
            AbstractSpreadsheet s = new Spreadsheet("Save/Test.xml", a => true, a => a, "default");
        }

        /// <summary>
        /// simulate a fail to load for versionTesting
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void versionTestFailed()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.GetSavedVersion("save/test.xml");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void cellValue()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.GetCellValue(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void cellValue1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.GetCellValue("5a");
        }
    }
}