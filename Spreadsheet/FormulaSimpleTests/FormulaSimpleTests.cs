﻿// Written by Joe Zachary for CS 3500, January 2017.
// Tests added by Lingxi Zhong U0770136 1/26/17

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;
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
        public void constructorWithUnderScore()
        {
            Formula f = new Formula("_");
        }

        /// <summary>
        /// This is another syntax error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorWithDoubleOperator()
        {
            Formula f = new Formula("2++3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorWithDoubleOperands()
        {
            Formula f = new Formula("2 3");
        }

        [TestMethod]
        public void ConstructorTestWithValidSimpleEquation()
        {
            Formula f = new Formula("2+3");
        }

        [TestMethod]
        public void ConstructorTestWithParenthesis()
        {
            Formula f = new Formula("(2+3)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructTestWithSeveralInvalidTokens()
        {
            Formula f = new Formula("_$_");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTestWithNoTokens()
        {
            Formula f = new Formula("");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTestWithBackWardsParenthesis()
        {
            Formula f = new Formula(")2+3(");
        }

        [TestMethod]
        public void ConstructorTestWithMultipleParenthesis()
        {
            Formula f = new Formula("6+4/(2-3)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTestWithMultipleIncompleteParenthesis()
        {
            Formula f = new Formula("(2+3");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTestWithMultipleIncompleteParenthFlipped()
        {
            Formula f = new Formula("2+3)");
        }


        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTestWithStartingOperator()
        {
            Formula f = new Formula("-3");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTestWithEndingOperator()
        {
            Formula f = new Formula("2+3-");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTestWithOutOfPlaceOperands()
        {
            Formula f = new Formula("36+4 6");
        }

        [TestMethod]
        public void ConstructorTestMakingSureLongOperandsWork()
        {
            Formula f = new Formula("(253/5345)+(242352)-(34534)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTestWithOperandParenthOperandFunc()
        {
            Formula f = new Formula("23(34)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTestWithOperandParenthFuncReversedShouldFail()
        {
            Formula f = new Formula("(234)(435)(435)");
        }

        [TestMethod]
        public void ConstructorTestWithOperandParenthShouldPass()
        {
            Formula f = new Formula("(234) + (435) - (435)");
        }

        [TestMethod]
        public void ConstructorTestWithVariables()
        {
            Formula f = new Formula("g+3+s+y");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTestWithBadTokenMultiple()
        {
            Formula f = new Formula("6+3-(4/2)+$");
        }

        [TestMethod]
        public void ConstructorTestWithPlusandVar()
        {
            Formula f = new Formula("x + y");
        }

        [TestMethod]
        public void ConsturtorTestWithADecimal()
        {
            Formula f = new Formula("2.3 + 4");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorWithParenthNegStart()
        {
            Formula f = new Formula("(-)2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorWithParenthNegEnd()
        {
            Formula f = new Formula("2(-)");
        }

        [TestMethod]
        public void emptyParameterTest()
        {
            Formula f = new Formula();
        }
        /// <summary>
        /// Makes sure that "2+3" evaluates to 5.  Since the Formula
        /// contains no variables, the delegate passed in as the
        /// parameter doesn't matter.  We are passing in one that
        /// maps all variables to zero.
        /// </summary>
        [TestMethod]
        public void SimpleAddEvaluate()
        {
            Formula f = new Formula("2+3");
            Assert.AreEqual(5.0, f.Evaluate(v => 0), 1e-6);
        }

        /// <summary>
        /// The Formula consists of a single variable (x5).  The value of
        /// the Formula depends on the value of x5, which is determined by
        /// the delegate passed to Evaluate.  Since this delegate maps all
        /// variables to 22.5, the return value should be 22.5.
        /// </summary>
        [TestMethod]
        public void SimpleDelegateFunctionEvaluation()
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
        public void VariableDoesntExistEvaluate()
        {
            Formula f = new Formula("x + y");
            f.Evaluate(v => { throw new UndefinedVariableException(v); });
        }

        /// <summary>
        /// The delegate passed to Evaluate is defined below.  We check
        /// that evaluating the formula returns in 10.
        /// </summary>
        [TestMethod]
        public void VariableDelegateSimpleFunction()
        {
            Formula f = new Formula("x + y");
            Assert.AreEqual(f.Evaluate(Lookup4), 10.0, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        [TestMethod]
        public void ComplexDelegateVariableEval()
        {
            Formula f = new Formula("(x + y) * (z / x) * 1.0");
            Assert.AreEqual(f.Evaluate(Lookup4), 20.0, 1e-6);
        }

        [TestMethod]
        public void ComplexFunctionWithNoParenth()
        {
            Formula f = new Formula("2+6*3-2+4/2");
            Assert.AreEqual(f.Evaluate(v => 0), 20, 1e-6);
        }

        [TestMethod]
        public void SimpleSubtractionEval()
        {
            Formula f = new Formula("6-3");
            Assert.AreEqual(f.Evaluate(v => 0), 3, 1e-6);
        }

        [TestMethod]
        public void SimpleSubtractionWithParenthEval()
        {
            Formula f = new Formula("(6-3)");
            Assert.AreEqual(f.Evaluate(v => 0), 3, 1e-6);
        }

        [TestMethod]
        public void SimpleDivideWithParenthEval()
        {
            Formula f = new Formula("(6/3)");
            Assert.AreEqual(f.Evaluate(v => 0), 2, 1e-6);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void DivideByZeroEval()
        {
            Formula f = new Formula("6/0");
            Assert.AreEqual(f.Evaluate(v => 0), 0, 1e-6);
        }

        [TestMethod]
        public void MultipleDivideEval()
        {
            Formula f = new Formula("10/5/2");
            Assert.AreEqual(f.Evaluate(v => 0), 1, 1e-6);

        }

        [TestMethod]
        public void ComplexMultiDivEval()
        {
            Formula f = new Formula("((20/4)/(25/5))");
            Assert.AreEqual(f.Evaluate(v => 0), 1, 1e-6);
        }


        //////////////////////////////////////////////////////////////////////////////

        [TestMethod]
        public void NewConstructorTest()
        {
            Formula f = new Formula("x+y+z", x => x.ToUpper(), t => true);

            Assert.IsFalse(f.ToString().Contains("x"));
            Assert.IsTrue(f.ToString().Contains("X"));
        }

        [TestMethod]
        public void getVariablesTest()
        {
            Formula f = new Formula("x+y+z");
            ISet<string> variables = (HashSet<String>)f.GetVariables();
            Assert.AreEqual(true, variables.Contains("x"));
            Assert.AreEqual(true, variables.Contains("y"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void nullConstructor()
        {
            Formula f = new Formula(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void nullEvaluate()
        {
            Formula f = new Formula("2+2");
            f.Evaluate(null);
        }

        [TestMethod]
        public void ZeroArgumentConstructor()
        {
            Formula f = new Formula();
            double result = f.Evaluate(v => 0);
            Assert.AreEqual(0, result, .001);
        }

        [TestMethod]
        public void ZeroArgumentConstructorToStringAndVariableGet()
        {
            Formula f = new Formula();
            Assert.AreEqual("0", f.ToString());
            ISet<String> test = f.GetVariables();
            string result = "";
            foreach(string s in test)
            {
                result = result + s;
            }
            Assert.AreEqual("", result);
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
    }
}
