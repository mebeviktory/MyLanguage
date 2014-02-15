using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EquationsSolver
{
    internal sealed partial class Parser
    {
        public struct Error
        {
            public readonly Nodes.Node.Coord Coords;
            public readonly ParserException.ErrorType TypeError;

            public Error(ParserException.ErrorType typeError, Nodes.Node.Coord coords)
            {
                this.TypeError = typeError;
                this.Coords = coords;
            }
        }

        private static List<string> UnUseLabeles { get; set; }
        private static bool Flag = true;
        public static List<Error> Errors { get; private set; }
        public static List<Nodes.Expression> Arguments { get; private set; }

        private static void SkipToSemicolon()
        {
            while (Lexer.LookAHead().LexType != Lexer.LexType.EOF && Lexer.LookAHead().LexType != Lexer.LexType.Semicolon)
            {
                Lexer.GetNextToken();
            }
            Lexer.GetNextToken();
        }

        public static Nodes.Program EatProgram(string expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(expression);
            }

            Memory.VariableValues = new Dictionary<string, Nodes.Node.Values>();
            Lexer lexer = new Lexer(expression);
            Errors = new List<Error>();
            UnUseLabeles = new List<string>();
            Nodes.StatementList ListExpr = new Nodes.StatementList();
            Memory.LabelsToStatement = new Dictionary<string, Nodes.Statement>();
            ListExpr = EatStatementList();
            Nodes.Program program = new Nodes.Program(ListExpr);
            foreach (string name in UnUseLabeles)
            {
                if (!Memory.LabelsToStatement.ContainsKey(name))
                {
                    throw new ParserException(ParserException.ErrorType.IncorrectStatement, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                }
            }
            if (Flag)
            {
                return program;
            }
            return null;
        }

        private static Nodes.StatementList EatStatementList()
        {
            Nodes.StatementList ListExpr = new Nodes.StatementList();
            ListExpr.Statements = new List<Nodes.Statement>();
            Nodes.Statement PrevStatement = new Nodes.Statement();
            while (Lexer.LookAHead().LexType != Lexer.LexType.EOF && Lexer.LookAHead().LexType != Lexer.LexType.CloseBracketOfBlock)
            {
                try
                {
                    Nodes.Statement statement = EatStatement();
                    ListExpr.Statements.Add(statement);
                    if (PrevStatement == null)
                    {
                        PrevStatement = statement;
                    }
                    else
                    {
                        PrevStatement.NextStatement = statement;
                        PrevStatement = statement;
                    }
                }
                catch (ParserException exception)
                {
                    Errors.Add(new Error(exception.Exception, exception.ErrorCoord));
                    Flag = false;
                    SkipToSemicolon();
                }
            }
            PrevStatement.NextStatement = null;
            return ListExpr;
        }

        private static Nodes.Statement EatStatement()
        {
            Nodes.Statement statement = EatPrint();
            if (statement != null) return statement;

            statement = EatIf();
            if (statement != null) return statement;

            statement = EatFor();
            if (statement != null) return statement;

            statement = EatWhile();
            if (statement != null) return statement;

            statement = EatGoTo();
            if (statement != null) return statement;

            statement = EatLabel();
            if (statement != null) return statement;

            statement = EatDeclaration();
            if (statement != null)
            {
                if (Lexer.LookAHead().LexType == Lexer.LexType.Semicolon)
                {
                    Lexer.GetNextToken();
                }
                return statement;
            }

            statement = EatAssignment();
            if (statement != null)
            {
                if (Lexer.LookAHead().LexType == Lexer.LexType.Semicolon)
                {
                    Lexer.GetNextToken();
                }
                return statement;
            }
            throw new ParserException(ParserException.ErrorType.IncorrectStatement, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
        }

        private static Nodes.Print EatPrint()
        {
            if (Lexer.LookAHead().LexType == Lexer.LexType.Print)
            {
                Lexer.GetNextToken();
                if (Lexer.GetNextToken().LexType != Lexer.LexType.OpenBracket)
                {
                    throw new ParserException(ParserException.ErrorType.AbsenceOpenBracket, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                }

                string stringToPrint = String.Empty;
                Nodes.Expression expr = null;
                bool flag = false;
                Arguments = new List<Nodes.Expression>();
                MatchCollection matches = null;
                Lexer.Lexem NextLexem = Lexer.LookAHead();

                if (Lexer.LookAHead().LexType == Lexer.LexType.String)
                {
                    string currString = Lexer.GetNextToken().NameVariable;
                    MatchCollection currMatches = null;
                    List<Nodes.Expression> currArguments = null;
                    if (Lexer.LookAHead().LexType == Lexer.LexType.Comma)
                    {
                        flag = true;
                        currArguments = new List<Nodes.Expression>();
                        while (Lexer.LookAHead().LexType != Lexer.LexType.CloseBracket)
                        {
                            Lexer.GetNextToken();
                            Nodes.Expression oneArg = EatExpr();
                            currArguments.Add(oneArg);
                        }
                        currMatches = Regex.Matches(currString, "{[0-9]+}");
                    }
                    stringToPrint = currString;
                    matches = currMatches;
                    Arguments = currArguments;
                }
                else
                {
                    expr = EatExpr();
                    stringToPrint = null;
                }

                if (Lexer.GetNextToken().LexType != Lexer.LexType.CloseBracket)
                {
                    throw new ParserException(ParserException.ErrorType.AbsenceCloseBracket, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                }

                if (Lexer.GetNextToken().LexType != Lexer.LexType.Semicolon)
                {
                    throw new ParserException(ParserException.ErrorType.AbsenceSemiclon, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                }

                if (expr != null)
                {
                    return new Nodes.Print(expr, new Print());
                }

                if (stringToPrint != null)
                {
                    if (flag)
                    {
                        return new Nodes.Print(stringToPrint, matches, Arguments, new Print());
                    }
                    return new Nodes.Print(stringToPrint, new Print());
                }

            }
            return null;
        }

        private static Nodes.If EatIf()
        {
            if (Lexer.LookAHead().LexType == Lexer.LexType.If)
            {
                Lexer.GetNextToken();
                if (Lexer.GetNextToken().LexType != Lexer.LexType.OpenBracket)
                {
                    throw new ParserException(ParserException.ErrorType.AbsenceOpenBracket, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                }

                Nodes.Condition currCondition = EatCondition();
                if (Lexer.GetNextToken().LexType != Lexer.LexType.CloseBracket)
                {
                    throw new ParserException(ParserException.ErrorType.AbsenceCloseBracket, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                }

                Nodes.Block ifBlock = EatBlock();
                Nodes.Block elseBlock = null;

                if (Lexer.GetNextToken().LexType == Lexer.LexType.Else)
                {
                    elseBlock = EatBlock();
                }
                return new Nodes.If(currCondition, ifBlock, elseBlock);
            }
            return null;
        }

        private static Nodes.Condition EatCondition()
        {
            Nodes.Expression left = EatExpr();
            Lexer.Lexem currLexem = Lexer.GetNextToken();
            Nodes.Expression right = EatExpr();

            switch (currLexem.LexType)
            {
                case Lexer.LexType.Less:
                    return new Nodes.Condition(left, right, Nodes.Condition.Comparison.Less);
                case Lexer.LexType.LessOrEqual:
                    return new Nodes.Condition(left, right, Nodes.Condition.Comparison.LessOrEqual);
                case Lexer.LexType.Greater:
                    return new Nodes.Condition(left, right, Nodes.Condition.Comparison.Greater);
                case Lexer.LexType.GreaterOrEqual:
                    return new Nodes.Condition(left, right, Nodes.Condition.Comparison.GreaterOrEqual);
                case Lexer.LexType.DoubleEqual:
                    return new Nodes.Condition(left, right, Nodes.Condition.Comparison.Equal);
                case Lexer.LexType.NotEqual:
                    return new Nodes.Condition(left, right, Nodes.Condition.Comparison.NotEqual);
                default:
                    throw new ParserException(ParserException.ErrorType.IncorrectLexem, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
            }
        }

        private static Nodes.Block EatBlock()
        {
            Nodes.Block block = new Nodes.Block(new Nodes.StatementList(), new Nodes.Node.Coord(), new Nodes.Node.Coord());
            block.ListOfStatements.Statements = new List<Nodes.Statement>();
            Nodes.Statement PrevStatement = new Nodes.Statement();
            if (Lexer.LookAHead().LexType == Lexer.LexType.OpenBracketOfBlock)
            {
                Lexer.GetNextToken();
                while (Lexer.LookAHead().LexType != Lexer.LexType.CloseBracketOfBlock)
                {
                    Nodes.Statement statement = EatStatement();
                    block.ListOfStatements.Statements.Add(statement);
                    if (PrevStatement == null)
                    {
                        PrevStatement = statement;
                    }
                    else
                    {
                        PrevStatement.NextStatement = statement;
                        PrevStatement = statement;
                    }

                }
                Lexer.GetNextToken();
            }
            else
            {
                Nodes.Statement statement = EatStatement();
                block.ListOfStatements.Statements.Add(statement);
                PrevStatement = statement;
            }

            block.LastStatementInBlock = PrevStatement;
            return block;
        }

        private static Nodes.For EatFor()
        {
            if (Lexer.LookAHead().LexType == Lexer.LexType.For)
            {
                Lexer.GetNextToken();
                if (Lexer.GetNextToken().LexType != Lexer.LexType.OpenBracket)
                {
                    throw new ParserException(ParserException.ErrorType.AbsenceOpenBracket, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                }
                Nodes.Assignment currAssignment = EatAssignment();
                if (Lexer.GetNextToken().LexType != Lexer.LexType.Semicolon)
                {
                    throw new ParserException(ParserException.ErrorType.AbsenceSemiclon, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                }

                Nodes.Condition currCondition = EatCondition();
                if (Lexer.GetNextToken().LexType != Lexer.LexType.Semicolon)
                {
                    throw new ParserException(ParserException.ErrorType.AbsenceSemiclon, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                }

                Nodes.Assignment delta = EatAssignment();
                if (Lexer.GetNextToken().LexType != Lexer.LexType.CloseBracket)
                {
                    throw new ParserException(ParserException.ErrorType.AbsenceCloseBracket, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                }

                Nodes.Block block = EatBlock();
                return new Nodes.For(currCondition, block, currAssignment, delta);
            }
            return null;
        }

        private static Nodes.While EatWhile()
        {
            if (Lexer.LookAHead().LexType == Lexer.LexType.While)
            {
                Lexer.GetNextToken();
                if (Lexer.GetNextToken().LexType != Lexer.LexType.OpenBracket)
                {
                    throw new ParserException(ParserException.ErrorType.AbsenceOpenBracket, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                }
                Nodes.Condition currCondition = EatCondition();
                if (Lexer.GetNextToken().LexType != Lexer.LexType.CloseBracket)
                {
                    throw new ParserException(ParserException.ErrorType.AbsenceCloseBracket, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                }
                Nodes.Block whileBlock = EatBlock();
                return new Nodes.While(currCondition, whileBlock);
            }
            return null;
        }

        private static Nodes.GoTo EatGoTo()
        {
            if (Lexer.LookAHead().LexType == Lexer.LexType.GoTo)
            {
                Lexer.GetNextToken();
                if (Lexer.LookAHead().LexType == Lexer.LexType.Var)
                {
                    Lexer.Lexem currLexem = Lexer.GetNextToken();
                    Lexer.GetNextToken();

                    if (Memory.LabelsToStatement.ContainsKey(currLexem.NameVariable))
                    {
                        return new Nodes.GoTo(currLexem.NameVariable, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                    }
                    else
                    {
                        UnUseLabeles.Add(currLexem.NameVariable);
                        return new Nodes.GoTo(currLexem.NameVariable, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                    }

                }
                else
                {
                    throw new ParserException(ParserException.ErrorType.ErrorWithLabel, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                }
            }
            return null;
        }

        private static string EatString()
        {
            if (Lexer.LookAHead().LexType == Lexer.LexType.String)
            {
                Lexer.Lexem str = Lexer.GetNextToken();
                return str.NameVariable;
            }
            else
            {
                throw new ParserException(ParserException.ErrorType.IncorrectSymbol, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
            }
        }

        private static Nodes.Declaration EatDeclaration()
        {
            if ((Lexer.LookAHead().LexType == Lexer.LexType.Float) ||
                (Lexer.LookAHead().LexType == Lexer.LexType.Str))
            {
                Lexer.Lexem type = Lexer.GetNextToken();
                if (Lexer.LookAHead().LexType == Lexer.LexType.Var)
                {
                    Lexer.Lexem variable = Lexer.GetNextToken();

                    Nodes.Expression expr = new Nodes.Expression();
                    switch (type.LexType)
                    {
                        case Lexer.LexType.Str:
                            if (Lexer.GetNextToken().LexType == Lexer.LexType.Equal)
                            {
                                string str = EatString();
                                if (str != null)
                                {
                                    return new Nodes.Declaration(variable.NameVariable, str, str.Length);
                                }
                            }
                            else
                            {
                                throw new ParserException(ParserException.ErrorType.IncorrectAssignment, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                            }
                            break;

                        case Lexer.LexType.Float:
                            if (Lexer.GetNextToken().LexType == Lexer.LexType.Equal)
                            {
                                expr = EatExpr();
                                return new Nodes.Declaration(variable.NameVariable, expr);
                            }
                            else
                            {
                                throw new ParserException(ParserException.ErrorType.IncorrectAssignment, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                            }
                    }
                }
                else
                {
                    throw new ParserException(ParserException.ErrorType.IncorrectInitialization, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                }
            }
            return null;
        }

        private static Nodes.Statement EatLabel()
        {
            if (Lexer.LookAHead().LexType == Lexer.LexType.Label)
            {
                Lexer.Lexem variable = Lexer.GetNextToken();
                Nodes.Statement currStatement = EatStatement();
                if (Memory.LabelsToStatement.ContainsKey(variable.NameVariable))
                {
                    throw new ParserException(ParserException.ErrorType.SecondInitialization, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                }
                Memory.LabelsToStatement[variable.NameVariable] = currStatement;
                return currStatement;
            }
            return null;
        }

        private static Nodes.Assignment EatAssignment()
        {
            if (Lexer.LookAHead().LexType == Lexer.LexType.Var)
            {
                Lexer.Lexem variable = Lexer.GetNextToken();
                if ((Lexer.LookAHead().LexType == Lexer.LexType.PlusOne) || (Lexer.LookAHead().LexType == Lexer.LexType.MinusOne)) //plus and minus 1
                {

                    if (Lexer.LookAHead().LexType == Lexer.LexType.PlusOne)
                    {
                        Lexer.GetNextToken();
                        return new Nodes.Assignment(variable.NameVariable, 1);
                    }
                    if (Lexer.LookAHead().LexType == Lexer.LexType.MinusOne)
                    {
                        Lexer.GetNextToken();
                        return new Nodes.Assignment(variable.NameVariable, -1);
                    }
                }
                else if (Lexer.LookAHead().LexType == Lexer.LexType.OpenSquareBracket)
                {
                    Lexer.GetNextToken();
                    Nodes.Expression count = EatExpr();
                    if (Lexer.GetNextToken().LexType == Lexer.LexType.CloseSquareBracket && Lexer.GetNextToken().LexType == Lexer.LexType.Equal)
                    {
                        Nodes.Expression expr = EatExpr();
                        return new Nodes.Assignment(variable.NameVariable, count, expr);
                    }
                    else
                    {
                        throw new ParserException(ParserException.ErrorType.IncorrectSymbol, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                    }
                }
                else if (Lexer.LookAHead().LexType == Lexer.LexType.Dot)
                {
                    Lexer.GetNextToken();
                    if (Lexer.GetNextToken().LexType == Lexer.LexType.Length)
                    {
                        return new Nodes.Assignment(variable.NameVariable);
                    }
                    else
                    {
                        throw new ParserException(ParserException.ErrorType.IncorrectLexem, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                    }
                }
                else
                {
                    if (Lexer.GetNextToken().LexType == Lexer.LexType.Equal)
                    {
                        Nodes.Expression expr = EatExpr();
                        return new Nodes.Assignment(variable.NameVariable, expr);
                    }
                    else
                    {
                        throw new ParserException(ParserException.ErrorType.IncorrectAssignment, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                    }
                }
            }
            return null;
        }

        private static Nodes.Expression EatExpr()
        {
            Nodes.Expression leftNode = EatTerm();

            while (Lexer.LookAHead().LexType == Lexer.LexType.Add || Lexer.LookAHead().LexType == Lexer.LexType.Minus)
            {
                if (Lexer.LookAHead().LexType == Lexer.LexType.Add)
                {
                    Lexer.GetNextToken();
                    Nodes.Expression rightNode = EatTerm();
                    leftNode = new Nodes.BinaryOperation(leftNode, rightNode, Nodes.BinaryOperation.BinaryOperationType.Plus, rightNode.Start);
                    continue;
                }
                if (Lexer.LookAHead().LexType == Lexer.LexType.Minus)
                {
                    Lexer.GetNextToken();
                    Nodes.Expression rightNode = EatTerm();
                    leftNode = new Nodes.BinaryOperation(leftNode, rightNode, Nodes.BinaryOperation.BinaryOperationType.Minus, rightNode.Start);
                    continue;
                }
                break;
            }
            return leftNode;
        }

        private static Nodes.Expression EatTerm()
        {
            Nodes.Expression leftNode = EatDeg();
            while (Lexer.LookAHead().LexType == Lexer.LexType.Multiply || Lexer.LookAHead().LexType == Lexer.LexType.Division)
            {
                if (Lexer.LookAHead().LexType == Lexer.LexType.Multiply)
                {
                    Lexer.GetNextToken();
                    Nodes.Expression rightNode = EatDeg();
                    leftNode = new Nodes.BinaryOperation(leftNode, rightNode, Nodes.BinaryOperation.BinaryOperationType.Multiply, rightNode.Start);
                    continue;
                }
                if (Lexer.LookAHead().LexType == Lexer.LexType.Division)
                {
                    Lexer.GetNextToken();
                    Nodes.Expression rightNode = EatDeg();
                    leftNode = new Nodes.BinaryOperation(leftNode, rightNode, Nodes.BinaryOperation.BinaryOperationType.Division, rightNode.Start);
                    continue;
                }
                break;
            }
            return leftNode;
        }

        private static Nodes.Expression EatDeg()
        {
            Nodes.Expression leftNode = EatFactor();
            if (Lexer.LookAHead().LexType == Lexer.LexType.Degree)
            {
                Lexer.GetNextToken();
                Nodes.Expression rightNode = EatDeg();
                leftNode = new Nodes.BinaryOperation(leftNode, rightNode, Nodes.BinaryOperation.BinaryOperationType.Degree, rightNode.Start);
            }
            return leftNode;
        }

        private static Nodes.Expression EatFactor()
        {
            Nodes.Expression element = new Nodes.Expression();
            if (Lexer.LookAHead().LexType == Lexer.LexType.Number)
            {
                return new Nodes.Number(Lexer.LookAHead().Value, Lexer.GetNextToken().LexStart);
            }

            if (Lexer.LookAHead().LexType == Lexer.LexType.Var)
            {
                Lexer.Lexem variable = Lexer.GetNextToken();
                if (Lexer.LookAHead().LexType == Lexer.LexType.OpenSquareBracket)
                {
                    Lexer.GetNextToken();
                    Nodes.Expression count = EatExpr();
                    if (Lexer.GetNextToken().LexType != Lexer.LexType.CloseSquareBracket)
                    {
                        throw new ParserException(ParserException.ErrorType.AbsenceCloseBracketOfSlice, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                    }
                    return new Nodes.Slice(variable.NameVariable, count);
                }
                if (Lexer.LookAHead().LexType == Lexer.LexType.Dot)
                {
                    Lexer.GetNextToken();
                    if (Lexer.GetNextToken().LexType != Lexer.LexType.Length)
                    {
                        throw new ParserException(ParserException.ErrorType.IncorrectLexem, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                    }
                    return new Nodes.Length(variable.NameVariable);
                }
                return new Nodes.Variable(variable.NameVariable, variable.LexStart);
            }

            if (Lexer.LookAHead().LexType == Lexer.LexType.OpenBracket)
            {
                Lexer.GetNextToken();
                element = EatExpr();
                if (Lexer.GetNextToken().LexType != Lexer.LexType.CloseBracket)
                {
                    throw new ParserException(ParserException.ErrorType.AbsenceCloseBracket, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
                }
                return element;
            }
            throw new ParserException(ParserException.ErrorType.IncorrectLexem, new Nodes.Node.Coord(Lexer.currX, Lexer.currY));
        }
    }
}
