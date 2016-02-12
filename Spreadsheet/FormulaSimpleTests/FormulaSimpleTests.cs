// Written by Joe Zachary for CS 3500, January 2016.
// Repaired error in Evaluate5.  Added TestMethod Attribute
//    for Evaluate4 and Evaluate5 - JLZ January 25, 2016
// Corrected comment for Evaluate3 - JLZ January 29, 2016
//Added Tests by Neal Phelps on 2/11/2016

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace FormulaTestCases
{
    /// <summary>
    /// These test cases are in no sense comprehensive!  They are intended to show you how
    /// client code can make use of the Formula class, and to show you how to create your
    /// own (which we strongly recommend).  To run them, pull down the Test menu and do
    /// Run > All Tests.
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        /// <summary>
        /// This tests that a syntactically incorrect parameter to Formula results
        /// in a FormulaFormatException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct1()
        {
            Formula f = new Formula("_");
        }

        /// <summary>
        /// This is another syntax error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct2()
        {
            Formula f = new Formula("2++3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct3()
        {
            Formula f = new Formula("2 3");
        }
        
        /// <summary>
          /// Another syntax error.
          /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct4()
        {
            Formula f = new Formula("(2+3))");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct5()
        {
            Formula f = new Formula("((2/3)");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct6()
        {
            Formula f = new Formula("()2*3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct7()
        {
            Formula f = new Formula("2m +* 3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct8()
        {
            Formula f = new Formula("(+25 + 7)");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct9()
        {
            Formula f = new Formula("2m $ 3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct10()
        {
            Formula f = new Formula("2m + 3(");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct11()
        {
            Formula f = new Formula("(x(x + 3)");
        }
        /// <summary>
        /// Another syntax error.        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct12()
        {
            Formula f = new Formula("");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct13()
        {
            Formula f = new Formula("               ");
        }
        /// <summary>
        /// Another syntax error.
        /// </summary>
        /// [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct14()
        {
            Formula f = new Formula("$%^");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct15()
        {
            Formula f = new Formula("$ (5 + 6)");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct16()
        {
            Formula f = new Formula(null);
        }

        /// <summary>
        /// Divide by zero
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate6()
        {
            Formula f = new Formula("(x + y) * (z / 0) * 1.0");
            Assert.AreEqual(f.Evaluate(Lookup4), 20.0, 1e-6);
        }
        /// <summary>
        /// Bad variable
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate7()
        {
            Formula f = new Formula("(x + y) * (l / x) * 1.0");
            Assert.AreEqual(f.Evaluate(Lookup4), 20.0, 1e-6);
        }        
        
        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        [TestMethod]
        public void Evaluate8()
        {
            Formula f = new Formula("((x + y) * (z / x)) * (x * z) + ((5 + 13)/2)");
            Assert.AreEqual(f.Evaluate(Lookup4), 649.0, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        [TestMethod]
        public void Evaluate9()
        {
            Formula f = new Formula("5.0 * 1.0");
            Assert.AreEqual(f.Evaluate(Lookup4), 5.0, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        [TestMethod]
        public void Evaluate10()
        {
            Formula f = new Formula("10 + x + (x + x) * (z / x) * 5 + 6 + x - y");
            Assert.AreEqual(f.Evaluate(Lookup4), 98.0, 1e-6);
        }

        /// <summary>
        /// Makes sure that "2+3" evaluates to 5.  Since the Formula
        /// contains no variables, the delegate passed in as the
        /// parameter doesn't matter.  We are passing in one that
        /// maps all variables to zero.
        /// </summary>
        [TestMethod]
        public void Evaluate1()
        {
            Formula f = new Formula("2+3");
            Assert.AreEqual(f.Evaluate(v => 0), 5.0, 1e-6);
        }

        /// <summary>
        /// The Formula consists of a single variable (x5).  The value of
        /// the Formula depends on the value of x5, which is determined by
        /// the delegate passed to Evaluate.  Since this delegate maps all
        /// variables to 22.5, the return value should be 22.5.
        /// </summary>
        [TestMethod]
        public void Evaluate2()
        {
            Formula f = new Formula("x5");
            Assert.AreEqual(f.Evaluate(v => 22.5), 22.5, 1e-6);
        }

        /// <summary>
        /// Here, the delegate passed to Evaluate always throws a
        /// UndefinedVariableException (meaning that no variables have
        /// values).  The test case checks that the result of
        /// evaluating the Formula is a FormulaEvaluationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate3()
        {
            Formula f = new Formula("x + y");
            f.Evaluate(v => { throw new UndefinedVariableException(v); });
        }

        /// <summary>
        /// The delegate passed to Evaluate is defined below.  We check
        /// that evaluating the formula returns in 10.
        /// </summary>
        [TestMethod]
        public void Evaluate4()
        {
            Formula f = new Formula("x + y");
            Assert.AreEqual(f.Evaluate(Lookup4), 10.0, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        [TestMethod]
        public void Evaluate5()
        {
            Formula f = new Formula("(x + y) * (z / x) * 1.0");
            Assert.AreEqual(f.Evaluate(Lookup4), 20.0, 1e-6);
        }
        /// <summary>
        /// Test if normalizer works by changing oppisites and GetVariables Method
        /// </summary>
        [TestMethod]
        public void FormulaTest1()
        {
            Formula f = new Formula("(x + y) * (z / x) * 1.0", NormalizerOpposite, ValidatorTrue);
            HashSet<String> set = (HashSet<String>)f.GetVariables();
            Assert.IsTrue(set.Contains("a"));
            Assert.IsTrue(set.Contains("b"));
            Assert.IsTrue(set.Contains("c"));
            Assert.IsFalse(set.Contains("x"));
        }
        /// <summary>
        /// 
        /// Test Normilzer Uppercase and getVariables method
        /// </summary>
        [TestMethod]
        public void FormulaTest2()
        {
            Formula f = new Formula("(x + y) * (z / x) * 1.0", NormalizerUppercase, ValidatorTrue);
            HashSet<String> set = (HashSet<String>)f.GetVariables();
            Assert.IsTrue(set.Contains("X"));
            Assert.IsTrue(set.Contains("Y"));
            Assert.IsTrue(set.Contains("Z"));
            Assert.IsFalse(set.Contains("x"));
            Assert.IsFalse(set.Contains("A"));
        }
        /// <summary>
        /// Test the Normalizer returning bad variables
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaTest3()
        {
            Formula f = new Formula("(x + y) * (z / x) * 1.0", NormalizerException, ValidatorTrue);

        }

        /// <summary>
        /// Test Validator for no lower cases
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaTest4()
        {
            Formula f = new Formula("x + y", NormalizerNoChange, ValidatorNoLowerCase);

        }

        /// <summary>
        /// Test Normalizeruppercase and vaiddator lowercase
        /// </summary>
        [TestMethod]
        public void FormulaTest5()
        {
            Formula f = new Formula("x + y", NormalizerUppercase, ValidatorNoLowerCase);
            Assert.AreEqual(f.Evaluate(Lookup5), 10.0, 1e-6);

        }

        /// <summary>
        /// Like previous method but with uppercase
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaTest6()
        {
            Formula f = new Formula("x + y", NormalizerUppercase, ValidatorNoUpperCase);

        }
        /// <summary>
        /// Test for no digits on validator
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaTest7()
        {
            Formula f = new Formula("x5", NormalizerNoChange, ValidatorNoDigits);

        }

        /// <summary>
        /// Test the toString Method
        /// </summary>
        [TestMethod]
        public void FormulaTest8()
        {
            Formula f1 = new Formula("(x + y) * (z / x) * 1.0");
            Formula f = new Formula(f1.ToString(), NormalizerNoChange, ValidatorNoDigits);
            Assert.AreEqual(f.Evaluate(Lookup4), 20.0, 1e-6);

        }
        /// <summary>
        /// Test To string method again
        /// </summary>
        [TestMethod]
        public void FormulaTest9()
        {
            String str = "(x+y)*(z/x)*1.0";
            Formula f1 = new Formula("(x + y) * (z / x) * 1.0");
            Assert.AreEqual(f1.ToString(), str);

        }
        /// <summary>
        /// Test for zero argument constructor
        /// </summary>
        [TestMethod]
        public void FormulaTest10()
        {
            String zero = "0";
            Formula f1 = new Formula("0");
            Assert.AreEqual(0, f1.Evaluate(Lookup4));
            Formula f2 = new Formula();
            Assert.AreEqual(0, f2.Evaluate(Lookup4));
            Assert.AreEqual(zero, f2.ToString());


        }
        /// <summary>
        /// A Lookup method that maps x to 4.0, y to 6.0, and z to 8.0.
        /// All other variables result in an UndefinedVariableException.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double Lookup4(String v)
        {
            switch (v)
            {
                case "x": return 4.0;
                case "y": return 6.0;
                case "z": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }
        /// <summary>
        /// New lookup method for uppercase variables
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double Lookup5(String v)
        {
            switch (v)
            {
                case "X": return 4.0;
                case "Y": return 6.0;
                case "Z": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }

        /// <summary>
        /// 
        /// Normalizer with no change
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string NormalizerNoChange(string s)
        {
            return s;
        }
        /// <summary>
        /// Chagne variables to differnt variables
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string NormalizerOpposite(string s)
        {
            switch (s)
            {
                case "x": return "c";
                case "y": return "b";
                case "z": return "a";
                default: throw new UndefinedVariableException(s);
            }
        }
        /// <summary>
        /// Cahnge variables to uppercase
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string NormalizerUppercase(string s)
        {
            return s.ToUpper();
        }
        /// <summary>
        /// Set bad varaibles
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string NormalizerException(string s)
        {
            switch (s)
            {
                case "x": return "$";
                case "y": return "@";
                case "z": return "?";
                default: throw new UndefinedVariableException(s);
            }
        }

        /// <summary>
        /// always true
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool ValidatorTrue(string s)
        {
            return true;
        }

        /// <summary>
        /// Always false
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool ValidatorFalse(string s)
        {
            return false;
        }

        /// <summary>
        /// Validator constrains lowercase
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool ValidatorNoLowerCase(string s)
        {
            if (Regex.IsMatch(s, @"[a-z]"))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Constrains uppercase
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool ValidatorNoUpperCase(string s)
        {
            if (Regex.IsMatch(s, @"^[A-Z]+"))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Constains digits
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool ValidatorNoDigits(string s)
        {
            if (Regex.IsMatch(s, @"\d"))
            {
                return false;
            }
            return true;
        }
    }
}
