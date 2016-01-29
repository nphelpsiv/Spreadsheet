// Skeleton and some comments written by Joe Zachary for CS 3500, January 2015
// Revised by Joe Zachary, January 2016
// JLZ Repaired pair of mistakes, January 23, 2016
//Formula and Evaluate Methods completed and filled by Neal Phelps U0669056 CS 3500, Jan. 2016

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
        private IEnumerable<String> tokens;
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
            //set variables that should not be reset with each new token
            int count = 0;
            int countlp = 0;
            int countrp = 0;
            bool isDorVARorRP = false;
            tokens = GetTokens(formula);
            //Check if there is at least one token, and is not null
            if (formula == null ||formula.Equals("") || tokens.Count() == 0)
            {
                throw new FormulaFormatException("Formula must have at least one token");
            }
            foreach (String token in tokens)
            {
                //set and reset variables for each new token in tokens
                bool isVar = false;
                bool isOper = false;
                bool islp = false;
                bool isrp = false;
                bool isDouble = false;
                double n;
                //Do checks to see what type the token is and set booleans accordingly
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
                    //Check if we have too many right parenthesis
                    if(countrp > countlp)
                    {
                        throw new FormulaFormatException("Too many close paranthesis");
                    }
                }
                //Check if it's a double, if so store in n
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
                //If its not one of the valid types throw the exception
                else
                {
                    throw new FormulaFormatException("There is a character that is not valid");
                }
                //Check if first variable is valid
                if(count == 0 && !(islp || isDouble || isVar))
                {
                    throw new FormulaFormatException("The first token of a formula must be a number, a variable, or an opening parenthesis.");
                }
                //If we are on the last one check if its the incorrect token
                if (count == formula.Length)
                {
                    if (!(isrp || isDouble || isVar))
                    {
                        throw new FormulaFormatException("The last token of a formula must be a number, a variable, or a closing parenthesis.");
                    }
                    //Check if paranthesis are equal at end
                    if(countlp != countrp)
                    {
                        throw new FormulaFormatException("The total number of opening parentheses must equal the total number of closing parentheses.");
                    }
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
            //Create stacks and final result
            Stack<String> strStack = new Stack<String>();
            Stack<Double> dStack = new Stack<Double>();
            Double result = 0;
            foreach (String token in tokens)
            {
                //Create variables that reset after each new token
                bool isVar = false;
                bool isOper = false;
                bool islp = false;
                bool isrp = false;
                bool isDouble = false;
                double d = 0;
                //make checks to see what type the token is
                if (Regex.IsMatch(token, @"[a-zA-Z][0-9a-zA-Z]*"))
                {
                    isVar = true;

                }
                else if (Regex.IsMatch(token, @"[\+\-*/]"))
                {

                    isOper = true;

                }
                else if (Regex.IsMatch(token, @"^[(]+$"))
                {
                    islp = true;
                }
                else if (Regex.IsMatch(token, @"^[)]+$"))
                {
                    isrp = true;
                }
                //Check if it is a double, if so store in d
                else if (Double.TryParse(token, out d))
                {
                    isDouble = true;
                }
                //if it is a variable call lookup method to see if it correlates to value, store in that double value, otherwise throw exception
                if (isVar)
                {
                    try
                    {
                        d = lookup(token);
                        isDouble = true;
                    }
                    catch(UndefinedVariableException e)
                    {
                        throw new FormulaEvaluationException(token + " is not a defined variable.");
                    }
                }
                //If * or / is at the top of the operator stack, pop the value stack, pop the operator stack, 
                //and apply the popped operator to t and the popped number. Push the result onto the value stack. 
                // Otherwise, push t onto the value stack
                //Any variable has already been converted to double at this point and will work accordingly
                if (isDouble)
                {
                    if(strStack.Count != 0 && (strStack.Peek().Equals("*") || (strStack.Peek().Equals("/")))){
                        Double temp;
                        String strOper = strStack.Pop();
                        Double value = dStack.Pop();
                        if (strOper.Equals("*"))
                        {
                            temp = d * value;
                        }
                        else
                        {
                            if(d != 0)
                            {
                                temp = value / d;
                            }
                            else
                            {
                                throw new FormulaEvaluationException("Cannot divide by zero");
                            }
                        }
                        dStack.Push(temp);
                    }
                    else
                    {
                        dStack.Push(d);
                    }
                }
                //Check if operator
                else if (isOper)
                {
                    //If + or - is at the top of the operator stack, pop the value stack twice and the operator stack once.  
                    //Apply the popped operator to the popped numbers. Push the result onto the value stack.
                    //Whether or not you did the first step, push t onto the operator stack
                    if (token.Equals("+") || token.Equals("-"))
                    {
                        if(strStack.Count != 0 && (strStack.Peek().Equals("+") || strStack.Peek().Equals("-")))
                        {
                            Double value1 = dStack.Pop();
                            Double value2 = dStack.Pop();
                            String oper = strStack.Pop();
                            Double temp;
                            if (oper.Equals("+"))
                            {
                                temp = value1 + value2;
                            }
                            else
                            {
                                temp = value2 - value1;
                            }
                            dStack.Push(temp);
                        }
                        strStack.Push(token);
                    }
                    //If operator is * or / Push t onto the operator stack
                    else if (token.Equals("*") || token.Equals("/"))
                    {
                        strStack.Push(token);
                    }
                }
                //If left paranthesis push on stack
                else if (islp)
                {
                    strStack.Push(token);
                }
                //If + or - is at the top of the operator stack, pop the value stack twice and the operator stack once. 
                //Apply the popped operator to the popped numbers. Push the result onto the value stack.
                //Whether or not you did the first step, the top of the operator stack will be a (.Pop it.
                //After you have completed the previous step, if *or / is at the top of the operator stack, 
                //pop the value stack twice and the operator stack once. Apply the popped operator to the popped numbers. 
                //Push the result onto the value stack.
                else if (isrp)
                {

                    if (strStack.Count != 0 && (strStack.Peek().Equals("+") || strStack.Peek().Equals("-")))
                    {
                        Double value1 = dStack.Pop();
                        Double value2 = dStack.Pop();
                        String oper = strStack.Pop();
                        Double temp;
                        if (oper.Equals("+"))
                        {
                            temp = value1 + value2;
                        }
                        else
                        {
                            temp = value2 - value1;
                        }
                        dStack.Push(temp);
                    }
                    String rp = strStack.Pop();
                    if (strStack.Count != 0 && (strStack.Peek().Equals("*") || strStack.Peek().Equals("/")))
                    {
                        Double value1 = dStack.Pop();
                        Double value2 = dStack.Pop();
                        String oper = strStack.Pop();
                        Double temp;
                        if (oper.Equals("*"))
                        {
                            temp = value1 * value2;
                        }
                        else
                        {
                            if(value1 != 0)
                            {
                                temp = value2 / value1;
                            }
                            else
                            {
                                throw new FormulaEvaluationException("Cannot divide by zero");
                            }
                        }
                        dStack.Push(temp);
                    }
                }
                
            }
            //If operator stack is empty: Value stack will contain a single number.  Pop it and report as the value of the expression
            if (strStack.Count == 0)
            {
                result = dStack.Pop();
            }
            //Otherwise: There will be exactly one operator on the operator stack, and it will be either + or -. 
            //There will be exactly two values on the value stack. 
            //Apply the operator to the two values and report the result as the value of the expression.
            else
            {
                Double value1 = dStack.Pop();
                Double value2 = dStack.Pop();
                String oper = strStack.Pop();
                if (oper.Equals("+"))
                {
                    result = value1 + value2;
                }
                else if (oper.Equals("-"))
                {
                    result = value2 - value1;
                }
            }
            return result;
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
