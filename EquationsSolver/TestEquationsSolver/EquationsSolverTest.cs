/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using EquationsSolver;

namespace TestEquationsSolver
{
    [TestFixture]
    class EquationsSolverTest
    {
        [Test]
        public void CheckForAssignment()
        {
            string programForTest = "x=5;";
            Dictionary<string, float> expectResult = new Dictionary<string, float>();
            expectResult.Add("x", 5);
            Parser.EatProgram(programForTest).Interpret();
            Assert.AreEqual(Memory.VariableValues, expectResult);
        }

        [Test]
        public void CheckForAssignments()
        {
            string programForTest = "x = 18; y = 3; z=x/y";
            Dictionary<string, float> expectResult = new Dictionary<string, float>();
            expectResult.Add("x", 18);
            expectResult.Add("y", 3);
            expectResult.Add("z", 6);
            Parser.EatProgram(programForTest).Interpret();
            Assert.AreEqual(Memory.VariableValues, expectResult);
        }
        [Test]
        public void CheckForFor()
        {
            string programForTest = "x = 1; for (i=0;i<=3;i=i+1) {x = x*2;}";
            Dictionary<string, float> expectResult = new Dictionary<string, float>();
            expectResult.Add("i", 4);
            expectResult.Add("x", 16);
            Parser.EatProgram(programForTest).Interpret();
            Assert.AreEqual(Memory.VariableValues, expectResult);
        }

        [Test]
        public void CheckForIfWithOutElse()
        {
            string programForTest = "if (3<4) { x=5;}";
            Dictionary<string, float> expectResult = new Dictionary<string, float>();
            expectResult.Add("x", 5);
            Parser.EatProgram(programForTest).Interpret();
            Assert.AreEqual(Memory.VariableValues, expectResult);
        }

        [Test]
        public void CheckForIfWithElse()
        {
            string programForTest = "if (3>4) { x = 0;} else {x=1;}";
            Dictionary<string, float> expectResult = new Dictionary<string, float>();
            expectResult.Add("x", 1);
            Parser.EatProgram(programForTest).Interpret();
            Assert.AreEqual(Memory.VariableValues, expectResult);
        }

        [Test]
        public void CheckForStatementIfWithIfBlockWithOutBracket()
        {
            string programForTest = "if (2>=2) x = 25;";
            Dictionary<string, float> expectResult = new Dictionary<string, float>();
            expectResult.Add("x", 25);
            Parser.EatProgram(programForTest).Interpret();
            Assert.AreEqual(Memory.VariableValues, expectResult);
        }

        [Test]
        public void CheckForStatementIfWithElseblockWithOutBracket()
        {
            string programForTest = "if (2>3) { x = 0} else x = 1;";
            Dictionary<string, float> expectResult = new Dictionary<string, float>();
            expectResult.Add("x", 1);
            Parser.EatProgram(programForTest).Interpret();
            Assert.AreEqual(Memory.VariableValues, expectResult);
        }
        [Test]
        public void CheckForStatementForWithOutBracket()
        {
            string programForTest = " x = 1; for (i=0; i<=2; i=i+1) x= x*2;";
            Dictionary<string, float> expectResult = new Dictionary<string, float>();
            expectResult.Add("x", 8);
            expectResult.Add("i", 3);
            Parser.EatProgram(programForTest).Interpret();
            Assert.AreEqual(Memory.VariableValues, expectResult);
        }

        [Test]
        public void CheckForLongVarWithNumbers()
        {
            string programForTest = "Var2504t = 13;";
            Dictionary<string, float> expectResult = new Dictionary<string, float>();
            expectResult.Add("Var2504t", 13);
            Parser.EatProgram(programForTest).Interpret();
            Assert.AreEqual(Memory.VariableValues, expectResult);
        }

        [Test]
        public void CheckForHardExpr()
        {
            string programForTest = "v=2*(5+(6**2-(24/8)));";
            Dictionary<string, float> expectResult = new Dictionary<string, float>();
            expectResult.Add("v", 76);
            Parser.EatProgram(programForTest).Interpret();
            Assert.AreEqual(Memory.VariableValues, expectResult);
        }

        [Test]
        public void CheckForIncorrectAssignment()
        {
            string programForTest = "a==3;";
            EquationsSolver.Parser.EatProgram(programForTest);
            List<Parser.Error> expErrors = new List<Parser.Error> ();
            expErrors.Add(new Parser.Error(ParserException.ErrorType.IncorrectAssignment, 3, 1));
            Assert.AreEqual(Parser.Errors, expErrors);
        }

        [Test]
        public void CheckForIncorrectAssignments()
        {
            string programForTest = "a==3; b = (2+3;";
            EquationsSolver.Parser.EatProgram(programForTest);
            List<Parser.Error> expErrors = new List<Parser.Error>();
            expErrors.Add(new Parser.Error(ParserException.ErrorType.IncorrectAssignment, 3, 1));
            expErrors.Add(new Parser.Error(ParserException.ErrorType.AbsenceCloseBracket, 11, 1));
            Assert.AreEqual(Parser.Errors, expErrors);
        }

        [Test]
        public void CheckForIncorrectFor()
        {
            string programForTest = "for (i=0;i<2;i=i+1;) {} ";
            EquationsSolver.Parser.EatProgram(programForTest);
            List<Parser.Error> expErrors = new List<Parser.Error>();
            expErrors.Add(new Parser.Error(ParserException.ErrorType.IncorrectSemicolon, 12, 1));
            Assert.AreEqual(Parser.Errors, expErrors);
        }

        [Test]
        public void CheckForEmptyBlockInFor()
        {
            string programForTest = "for (i=0;i<2;i=i+1) {} ";
            Dictionary<string, float> expectResult = new Dictionary<string, float>();
            expectResult.Add("i", 2);
            Parser.EatProgram(programForTest).Interpret();
            Assert.AreEqual(Memory.VariableValues, expectResult);
        }

        [Test]
        public void CheckForEmptyIfBlockInIf()
        {
            string programForTest = "x = 0;if (3<4) {} else {x=1;}";
            Dictionary<string, float> expectResult = new Dictionary<string, float>();
            expectResult.Add("x", 0);
            Parser.EatProgram(programForTest).Interpret();
            Assert.AreEqual(Memory.VariableValues, expectResult);
        }

        [Test]
        public void CheckForEmptyElseBlockInIf()
        {
            string programForTest = "x = 0; if (3>4) { x = 5;} else {}";
            Dictionary<string, float> expectResult = new Dictionary<string, float>();
            expectResult.Add("x", 0);
            Parser.EatProgram(programForTest).Interpret();
            Assert.AreEqual(Memory.VariableValues, expectResult);
        }

        [Test]
        public void CheckForPrintWithOutCloseBracket()
        {
            string programForTest = "x=0; print(x;";
            EquationsSolver.Parser.EatProgram(programForTest);
            List<Parser.Error> expErrors = new List<Parser.Error>();
            expErrors.Add(new Parser.Error(ParserException.ErrorType.AbsenceCloseBracket, 10, 1));
            Assert.AreEqual(Parser.Errors, expErrors);
        }

        [Test]
        [ExpectedException(typeof(ParserException))]
        public void CheckForDivisionbyZero()
        {
            string programForTest = "x = 5 / 0;";
            Dictionary<string, float> expectResult = new Dictionary<string, float>();
            expectResult.Add("x", 5);
            Parser.EatProgram(programForTest).Interpret();
            Assert.AreEqual(expectResult, Memory.VariableValues);
        }

        [Test]
        [ExpectedException(typeof(ParserException))]
        public void CheckForNonInitializedVar()
        {
            string programForTest = "y=x+3;";
            Dictionary<string, float> expectResult = new Dictionary<string, float>();
            expectResult.Add("y", 3);
            Parser.EatProgram(programForTest).Interpret();
            Assert.AreEqual(expectResult, Memory.VariableValues);
        }
    }
}*/
