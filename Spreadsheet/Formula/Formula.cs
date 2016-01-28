﻿// Skeleton written by Joe Zachary for CS 3500, January 2015
// Revised by Joe Zachary, January 2016
// JLZ Repaired pair of mistakes, January 23, 2016

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Formulas
{
    /// <summary>
    /// Represents formulas written in standard infixC:\Program Files\Development Game Projects\CS3500\Repos\Spreadsheet\Spreadsheet\Formula\Formula.cs notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>
    public class Formula
    {
        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// Examples of a valid parameter to this constructor are:
        ///     "2.5e9 + x5 / 17"
        ///     "(5 * 2) + 8"
        ///     "x*y-2+35/9"
        ///     
        /// Examples of invalid parameters are:
        ///     "_"
        ///     "-5.3"
        ///     "2 5 + 3"
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// </summary>
        public Formula(String formula)
        {
            int count = 0;
            int countlp = 0;
            int countrp = 0;
            bool isDorVARorRP = false;
            IEnumerable<String> tokens = GetTokens(formula);
            foreach (String token in tokens)
            {
                bool isVar = false;
                bool isOper = false;
                bool islp = false;
                bool isrp = false;
                bool isDouble = false;
                double n;
                if (count == 0 && token.Equals(""))
                {
                    throw new FormulaFormatException("Formula must have at least one token");
                }
                if (Regex.IsMatch(token, @"[a-zA-Z][0-9a-zA-Z]*"))
                {
                    if (isDorVARorRP == true)
                    {
                        throw new FormulaFormatException("Any token that immediately follows a number, a variable, or a closing parenthesis must be either an operator or a closing parenthesis.");
                    }
                    isVar = true;
                    isDorVARorRP = true;
                    count++;
                }
                else if (Regex.IsMatch(token, @"[\+\-*/]"))
                {
                    if (isDorVARorRP == false)
                    {
                        throw new FormulaFormatException("Any token that immediately follows an opening parenthesis or an operator must be either a number, a variable, or an opening parenthesis.");
                    }
                    isOper = true;
                    isDorVARorRP = false;
                    count++;
                }
                else if (Regex.IsMatch(token, @"^[(]+$"))
                {
                    if (isDorVARorRP == true)
                    {
                        throw new FormulaFormatException("Any token that immediately follows a number, a variable, or a closing parenthesis must be either an operator or a closing parenthesis.");
                    }
                    islp = true;
                    isDorVARorRP = false;
                    count++;
                    countlp++;
                }
                else if (Regex.IsMatch(token, @"^[)]+$"))
                {
                    if (isDorVARorRP == false)
                    {
                        throw new FormulaFormatException("Any token that immediately follows an opening parenthesis or an operator must be either a number, a variable, or an opening parenthesis.");
                    }
                    isrp = true;
                    isDorVARorRP = true;
                    count++;
                    countrp++;
                    if(countrp > countlp)
                    {
                        throw new FormulaFormatException("Too many close paranthesis");
                    }
                }
                else if (Double.TryParse(token, out n))
                {
                    if (isDorVARorRP == true)
                    {
                        throw new FormulaFormatException("Any token that immediately follows a number, a variable, or a closing parenthesis must be either an operator or a closing parenthesis.");
                    }
                    isDouble = true;
                    isDorVARorRP = true;
                    count++;
                }
                else
                {
                    throw new FormulaFormatException("There is a character that is not valid");
                }
                if(count == 1 && !(islp || isDouble || isVar))
                {
                    throw new FormulaFormatException("The first token of a formula must be a number, a variable, or an opening parenthesis.");
                }
                if (count == formula.Length)
                {
                    if (!(isrp || isDouble || isVar))
                    {
                        throw new FormulaFormatException("The last token of a formula must be a number, a variable, or a closing parenthesis.");
                    }
                    if(countlp != countrp)
                    {
                        throw new FormulaFormatException("The total number of opening parentheses must equal the total number of closing parentheses.");
                    }
                }
                if (count > 1 && isDorVARorRP == true)
                {

                }

            }
        }
        /// <summary>
        /// Evaluates this Formula, using the Lookup delegate to determine the values of variables.  (The
        /// delegate takes a variable name as a parameter and returns its value (if it has one) or throws
        /// an UndefinedVariableException (otherwise).  Uses the standard precedence rules when doing the evaluation.
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, its value is returned.  Otherwise, throws a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            return 0;
        }

        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of a letter followed by
        /// zero or more digits and/or letters, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
    }

    /// <summary>
    /// A Lookup method is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an UndefinedVariableException (meaning that the string is unmapped 
    /// to a value. Exactly how a Lookup method decides which strings map to doubles and which
    /// don't is up to the implementation of the method.
    /// </summary>
    public delegate double Lookup(string s);

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is the
        /// undefined variable.
        /// </summary>
        /// <param name="variable"></param>
        public UndefinedVariableException(String variable)
            : base(variable)
        {
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the parameter to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message) : base(message)
        {
        }
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message) : base(message)
        {
        }
    }
}
