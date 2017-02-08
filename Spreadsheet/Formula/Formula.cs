// Skeleton written by Joe Zachary for CS 3500, January 2017
// Implementation by Lingxi Zhong U0770136

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Formulas
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>
    public struct Formula
    {
        /// <summary>
        /// Variable to store constructor input to give to Evalute Method later.
        /// </summary>
        private string formulaPass;
        /// <summary>
        /// stores variables while constructing formula so they can be returned later in GetVariables()
        /// </summary>
        private HashSet<string> variables;
        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// </summary>
        public Formula(String formula) : this(formula, n=>n, v=>true)
        {
        }

        public Formula(String formula, Normalizer normalize, Validator validator)
        {
            if(formula == null || normalize == null || validator == null)
            {
                throw new ArgumentNullException();
            }
            formulaPass = normalize(formula);
            formula = normalize(formula);
            variables = new HashSet<string>();
            int length = 0;
            Stack<String> stringStack = new Stack<string>();
            int openParenth = 0;
            int closeParenth = 0;
            foreach (string inputString in GetTokens(formula))
            {
                // Console.Write(inputString); // For Testing only
                if (inputString.Equals("(") || inputString.Equals(")"))
                {
                    if (inputString.Equals("("))
                    {
                        openParenth++;
                        if (length == 0)
                        {
                            stringStack.Push(inputString);
                        }
                        length++;

                    }
                    if (inputString.Equals(")"))
                    {
                        closeParenth++;
                    }
                    if (closeParenth > openParenth)
                    {
                        throw new FormulaFormatException("Syntax Error: Parenthesis in wrong order");
                    }
                    continue;
                }
                if (inputString.Equals("+") || inputString.Equals("-") || inputString.Equals("*") || inputString.Equals("/"))
                {
                    if (length == 0)
                    {
                        throw new FormulaFormatException("Syntax Error: Cannot start formula with operator");
                    }
                    if (length == 1)
                    {
                        string prevString = stringStack.Pop();
                        if (prevString.Equals("("))
                        {
                            throw new FormulaFormatException("Syntax Error: Cannot start formula with operator");
                        }
                    }
                    if (length > 1)
                    {
                        string previousString = stringStack.Peek();
                        if (previousString.Equals("+") || previousString.Equals("-") || previousString.Equals("*") || previousString.Equals("/"))
                        {
                            throw new FormulaFormatException("Syntax Error: Cannot have 2 operators in sequence");
                        }
                    }
                    stringStack.Push(inputString);
                    length++;
                }
                else
                {

                    if (length == 0)
                    {
                        if (!Regex.IsMatch(inputString, @"[0-9a-zA-Z][0-9a-zA-Z]*"))
                        {
                            throw new FormulaFormatException("Invalid Token Received");
                        }
                        stringStack.Push(inputString);
                        length++;
                        continue;
                    }
                    string previousString = stringStack.Peek();
                    if (Regex.IsMatch(previousString, @"[0-9a-zA-Z][0-9a-zA-Z]*"))
                    {
                        throw new FormulaFormatException("Syntax Error: 2 operands in sequence");
                    }

                    if (Regex.IsMatch(inputString, @"[0-9a-zA-Z][0-9a-zA-Z]*"))
                    {
                        if (Regex.IsMatch(inputString, @"[a-zA-Z][0-9a-zA-Z]*")) // if inputString(token) is a variable: (PS4a)
                        {
                            if(validator(inputString) == false)
                            {
                                throw new FormulaFormatException("Validation failed");
                            }
                            variables.Add(inputString);
                        }
                        stringStack.Push(inputString);
                        length++;
                        continue;
                    }
                    throw new FormulaFormatException("Invalid Token Received");

                }
            }
            if (closeParenth != openParenth)
            {
                throw new FormulaFormatException("Invalid parenthesis placement");
            }
            if (length == 0)
            {
                throw new FormulaFormatException("Must have atleast one token");
            }
            string lastString = stringStack.Peek();
            if (lastString.Equals("+") || lastString.Equals("-") || lastString.Equals("*") || lastString.Equals("/"))
            {
                throw new FormulaFormatException("Syntax Error: Cannot end with operator");
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
            if(lookup == null)
            {
                throw new ArgumentNullException();
            }
            Stack<String> operatorStack = new Stack<string>();
            Stack<double> operandStack = new Stack<double>();
            double result = 0;
            foreach (string inputStringE in GetTokens(formulaPass))
            {
                if (inputStringE.Equals("+") || inputStringE.Equals("-"))
                {
                    // Execution if t is add "+" or subtract "-"
                    if (operatorStack.Count != 0)
                    {
                        if (operatorStack.Peek().Equals("+") || operatorStack.Peek().Equals("-"))
                        {
                            double opValue1 = operandStack.Pop();
                            double opValue2 = operandStack.Pop();
                            string opFunc = operatorStack.Pop();
                            if (opFunc.Equals("+"))
                            {
                                operandStack.Push(opValue1 + opValue2);
                            }
                            if (opFunc.Equals("-"))
                            {
                                operandStack.Push(opValue2 - opValue1);
                            }
                        }
                    }
                    operatorStack.Push(inputStringE);
                    continue;
                }

                if (inputStringE.Equals("*") || inputStringE.Equals("/"))
                {
                    // Execution if t is multiply "*" or divide "/"
                    operatorStack.Push(inputStringE);
                    continue;
                }

                if (inputStringE.Equals("("))
                {
                    // Execution if t is "(" open parenthesis
                    operatorStack.Push(inputStringE);
                    continue;
                }

                if (inputStringE.Equals(")"))
                {
                    // Execution if t is close parenthesis ")"
                    if (operatorStack.Peek().Equals("+") || operatorStack.Peek().Equals("-"))
                    {
                        double opValue1 = operandStack.Pop();
                        double opValue2 = operandStack.Pop();
                        string opFunc = operatorStack.Pop();
                        if (opFunc.Equals("+"))
                        {
                            operandStack.Push(opValue1 + opValue2);
                        }
                        if (opFunc.Equals("-"))
                        {
                            operandStack.Push(opValue2 - opValue1);
                        }
                    }
                    if (operatorStack.Peek().Equals("("))
                    {
                        operatorStack.Pop();
                    }
                    if (operatorStack.Count != 0)
                    {
                        if (operatorStack.Peek().Equals("*") || operatorStack.Peek().Equals("/"))
                        {
                            double opValue1 = operandStack.Pop();
                            double opValue2 = operandStack.Pop();
                            string opFunc = operatorStack.Pop();
                            if (opFunc.Equals("*"))
                            {
                                operandStack.Push(opValue1 * opValue2);
                                continue;
                            }
                            if (opFunc.Equals("/"))
                            {
                                if (opValue1 == 0)
                                {
                                    throw new FormulaEvaluationException("Cannot Divide by Zero");
                                }
                                operandStack.Push(opValue2 / opValue1);
                                continue;
                            }
                        }
                    }
                    continue;
                }
                // If t is a number or variable:
                double inputValue = 0;
                Boolean flag = Double.TryParse(inputStringE, out inputValue);
                if (Regex.IsMatch(inputStringE, @"[a-zA-Z][0-9a-zA-Z]*") && flag == false)
                {
                    // If t is a variable:
                    try
                    {
                        inputValue = lookup(inputStringE);
                    }
                    catch(UndefinedVariableException)
                    {
                        throw new FormulaEvaluationException("Variable is not defined");
                    }
                }
                if (operatorStack.Count != 0)
                {
                    if (operatorStack.Peek().Equals("*") || operatorStack.Peek().Equals("/"))
                    {
                        double tempValue = operandStack.Pop();
                        string operatorS = operatorStack.Pop();
                        if (operatorS.Equals("*"))
                        {
                            operandStack.Push(inputValue * tempValue);
                            continue;
                        }
                        else
                        {
                            if (inputValue == 0)
                            {
                                throw new FormulaEvaluationException("Cannot divide by zero");
                            }
                            operandStack.Push(tempValue / inputValue);
                            continue;
                        }
                    }
                }
                operandStack.Push(inputValue);
                
            }

            if (operatorStack.Count == 1)
            {
                double opValue1 = operandStack.Pop();
                double opValue2 = operandStack.Pop();
                string opFunc = operatorStack.Pop();
                if (opFunc.Equals("+"))
                {
                    operandStack.Push(opValue1 + opValue2);
                }
                if (opFunc.Equals("-"))
                {
                    operandStack.Push(opValue2 - opValue1);
                }
            }
            if (operatorStack.Count == 0)
            {
                result = operandStack.Pop();
            }

            return result;
        }

        public ISet<string> GetVariables()
        {
            return variables;
        }

        public string toString()
        {
            return formulaPass;
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
            // PLEASE NOTE:  I have added white space to this regex to make it more readable.
            // When the regex is used, it is necessary to include a parameter that says
            // embedded white space should be ignored.  See below for an example of this.
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern.  It contains embedded white space that must be ignored when
            // it is used.  See below for an example of this.
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            // PLEASE NOTE:  Notice the second parameter to Split, which says to ignore embedded white space
            /// in the pattern.
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
    public delegate double Lookup(string var);
    /// <summary>
    /// A normalizer delegate, normalizes things.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public delegate string Normalizer(string s);
    /// <summary>
    /// A validator delegate, validates things. 
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public delegate bool Validator(string s);

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    [Serializable]
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
    [Serializable]
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
    [Serializable]
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
