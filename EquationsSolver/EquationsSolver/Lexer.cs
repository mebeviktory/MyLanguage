using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver
{
    internal sealed partial class Parser
    {
        public class Lexer
        {
            private const string alphabet = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM_";
            private const string delimitersCharacters = "\t\r\n ";
            private const string digits = "0123456789";
            private const char zero = '0';
            private const char point = '.';
            private const char exponent = 'E';
            private static int x = 0, y = 0;
            public static int currX = 0, currY = 0;
            public static int currPos;
            private static string expression;
            public static int position;
            public static Stack<int> OffSets = new Stack<int>();

            public Lexer(string Expr)
            {
                expression = Expr;
                position = 0;
                OffSets = new Stack<int>();
                currX = 0; currY = 0;
                x = 0; y = 0;
            }

            public struct Lexem
            {
                public readonly LexType LexType;
                public readonly float Value;
                public readonly string NameVariable;
                public readonly Nodes.Node.Coord LexStart, LexEnd;
                public Lexem(LexType newLexem, float value, string nameVar, Nodes.Node.Coord lexStart, Nodes.Node.Coord lexEnd)
                {
                    this.LexType = newLexem;
                    this.Value = value;
                    this.NameVariable = nameVar;
                    this.LexStart = lexStart;
                    this.LexEnd = lexEnd;
                }
            }

            public enum LexType
            {
                Number,
                OpenBracket,
                CloseBracket,
                OpenBracketOfBlock,
                CloseBracketOfBlock,
                Add,
                Multiply,
                Minus,
                Degree,
                Division,
                EOF,
                Var,
                Semicolon,
                Equal,
                Print,
                String,
                If,
                Else,
                Less,
                Greater,
                GreaterOrEqual,
                LessOrEqual,
                DoubleEqual,
                NotEqual,
                For,
                While,
                MinusOne,
                PlusOne,
                Colon,
                Comma,
                Label,
                GoTo,
                Str,
                Bool,
                Float,
                OpenSquareBracket,
                CloseSquareBracket,
                Dot,
                Length
            }

            //for exp 
            private enum States
            {
                IntegerPart, Point, Exponent, FractionalPart, NumberAfterExponent
            }

            private static int SelectionNumber(int curPosition)
            {
                curPosition--;
                States state = States.IntegerPart;
                while (true)
                {
                    curPosition++;
                    switch (state)
                    {
                        case States.IntegerPart:
                            if (curPosition >= expression.Length)
                            {
                                return curPosition;
                            }
                            char curSymbol = expression[curPosition];
                            if (digits.Contains(expression[curPosition]))
                            {
                                curSymbol = zero;
                            }
                            switch (curSymbol)
                            {
                                case point:
                                    state = States.Point;
                                    continue;
                                case exponent:
                                    state = States.Exponent;
                                    continue;
                                case zero:
                                    continue;
                                default:
                                    return curPosition;
                            }
                        case States.Point:
                            if (curPosition >= expression.Length)
                            {
                                x = curPosition + 1 - position;
                                throw new LexerException(ParserException.ErrorType.IncorrectNumber, new Nodes.Node.Coord(currX + curPosition - position - 1, currY + y));
                            }
                            if (digits.Contains(expression[curPosition]))
                            {
                                state = States.FractionalPart;
                                continue;
                            }
                            x = curPosition + 1 - position;
                            throw new LexerException(ParserException.ErrorType.IncorrectNumber, new Nodes.Node.Coord(currX + curPosition - position - 1, currY + y));
                        case States.Exponent:
                            if (curPosition >= expression.Length)
                            {
                                x = curPosition + 1 - position;
                                throw new LexerException(ParserException.ErrorType.IncorrectNumber, new Nodes.Node.Coord(currX + curPosition - position - 1, currY + y));
                            }
                            curSymbol = expression[curPosition];
                            if (digits.Contains(expression[curPosition]))
                            {
                                curSymbol = zero;
                            }
                            switch (curSymbol)
                            {
                                case zero:
                                    state = States.NumberAfterExponent;
                                    continue;
                                case '+':
                                    curPosition++;
                                    state = States.NumberAfterExponent;
                                    continue;
                                case '-':
                                    curPosition++;
                                    state = States.NumberAfterExponent;
                                    continue;
                                default:
                                    x = curPosition + 1 - position;
                                    throw new LexerException(ParserException.ErrorType.IncorrectNumber, new Nodes.Node.Coord(currX + curPosition - position - 1, currY + y));
                            }
                        case States.FractionalPart:
                            if (curPosition >= expression.Length)
                            {
                                return curPosition;
                            }
                            curSymbol = expression[curPosition];
                            if (digits.Contains(expression[curPosition]))
                            {
                                curSymbol = zero;
                            }
                            switch (curSymbol)
                            {
                                case exponent:
                                    state = States.Exponent;
                                    continue;
                                case zero:
                                    continue;
                                default:
                                    return curPosition;
                            }
                        case States.NumberAfterExponent:
                            if (curPosition >= expression.Length)
                            {
                                return curPosition;
                            }
                            if (digits.Contains(expression[curPosition]))
                            {
                                continue;
                            }
                            return curPosition;
                    }
                }
            }

            public static Lexem CreateNewLexem(out int offSet)
            {

                if (currPos >= expression.Length)
                {
                    offSet = 0;
                    return new Lexem(LexType.EOF, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                }

                if (delimitersCharacters.Contains(expression[currPos]))
                {
                    if (expression[currPos] == '\n')
                    {
                        currY++;
                        currX = 0;
                        x = 0;
                        y = 0;
                    }
                    else
                    {
                        currX++;
                        x = 0;
                    }
                    currPos++;
                    CreateNewLexem(out offSet);
                }

                // check is it number;
                int number;
                int i = 0;
                string stringForNumber = String.Empty;
                while (currPos + i < expression.Length && int.TryParse(expression[currPos + i].ToString(), out number))
                {
                    stringForNumber += expression[currPos + i];
                    i++;
                }
                if (stringForNumber != String.Empty)
                {
                    offSet = i;
                    int.TryParse(stringForNumber, out number);
                    x = x + i - 1;
                    return new Lexem(LexType.Number, number, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                }

                //check is it variable;
                int j = 0;
                string nameVariable = String.Empty;
                while (currPos + j < expression.Length && (digits.Contains(expression[currPos + j]) || alphabet.Contains(expression[currPos + j])))
                {
                    nameVariable += expression[currPos + j];
                    j++;
                }

                if (nameVariable != String.Empty)
                {
                    offSet = j;
                    x = x + j - 1;
                    switch (nameVariable)
                    {
                        case "if":
                            {
                                return new Lexem(LexType.If, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                            }
                        case "else":
                            {
                                return new Lexem(LexType.Else, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                            }
                        case "print":
                            {
                                return new Lexem(LexType.Print, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                            }
                        case "for":
                            {
                                return new Lexem(LexType.For, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                            }
                        case "while":
                            {
                                return new Lexem(LexType.While, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                            }
                        case "goto":
                            {
                                return new Lexem(LexType.GoTo, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                            }
                        case "string":
                            {
                                return new Lexem(LexType.Str, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                            }
                        case "bool":
                            {
                                return new Lexem(LexType.Bool, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                            }
                        case "float":
                            {
                                return new Lexem(LexType.Float, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                            }
                        case "length":
                            {
                                return new Lexem(LexType.Length, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                            }
                        default:
                            {
                                if (expression[currPos + 1] == ':')
                                {
                                    return new Lexem(LexType.Label, 0, nameVariable, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                                }
                                return new Lexem(LexType.Var, 0, nameVariable, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                            }
                    }
                }

                if (currPos >= expression.Length)
                {
                    offSet = 0;
                    x = offSet;
                    return new Lexem(LexType.EOF, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                }

                offSet = 1;
                x = 1;
                switch (expression[currPos])
                {
                    case '*':
                        if (expression[currPos + 1] == '*')
                        {
                            offSet = 2;
                            x++;
                            return new Lexem(LexType.Degree, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                        }
                        return new Lexem(LexType.Multiply, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));

                    case '/':
                        return new Lexem(LexType.Division, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));

                    case ',':
                        return new Lexem(LexType.Comma, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));

                    case '.':
                        return new Lexem(LexType.Dot, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));

                    case '-':
                        if (expression[currPos + 1] == '-')
                        {
                            offSet = 2;
                            x++;
                            return new Lexem(LexType.MinusOne, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                        }
                        return new Lexem(LexType.Minus, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));

                    case '+':
                        if (expression[currPos + 1] == '+')
                        {
                            offSet = 2;
                            x++;
                            return new Lexem(LexType.PlusOne, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                        }
                        return new Lexem(LexType.Add, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));

                    case '(':
                        return new Lexem(LexType.OpenBracket, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));

                    case ')':
                        return new Lexem(LexType.CloseBracket, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));

                    case '{':
                        return new Lexem(LexType.OpenBracketOfBlock, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));

                    case '}':
                        return new Lexem(LexType.CloseBracketOfBlock, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));

                    case ';':
                        return new Lexem(LexType.Semicolon, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));

                    //case ':':
                        //return new Lexem(LexType.Colon, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                    case '[':
                        return new Lexem(LexType.OpenSquareBracket, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                    case ']':
                        return new Lexem(LexType.CloseSquareBracket, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                    case '=':
                        if (expression[currPos + 1] == '=')
                        {
                            offSet = 2;
                            x++;
                            return new Lexem(LexType.DoubleEqual, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                        }
                        return new Lexem(LexType.Equal, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));

                    case '"':
                        string stringToPrint = String.Empty;
                        if (currPos + offSet >= expression.Length)
                        {
                            throw new LexerException(ParserException.ErrorType.IncorrectLexem, new Nodes.Node.Coord(currX, currY));
                        }
                        while (expression[currPos + offSet] != '"')
                        {
                            if (currPos + offSet + 1 >= expression.Length)
                            {
                                throw new LexerException(ParserException.ErrorType.IncorrectLexem, new Nodes.Node.Coord(currX, currY));
                            }
                            else
                            {
                                stringToPrint += expression[currPos + offSet];
                            }
                            offSet++;
                            x++;
                        }
                        offSet++;
                        x++;
                        return new Lexem(LexType.String, 0, stringToPrint, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));

                    case '>':
                        if (expression[currPos + 1] == '=')
                        {
                            offSet = 2;
                            x++;
                            return new Lexem(LexType.GreaterOrEqual, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                        }
                        return new Lexem(LexType.Greater, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));

                    case '<':
                        if (expression[currPos + 1] == '=')
                        {
                            offSet = 2;
                            x++;
                            return new Lexem(LexType.LessOrEqual, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                        }
                        return new Lexem(LexType.Less, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));

                    case '!':
                        if (expression[currPos + 1] == '=')
                        {
                            offSet = 2;
                            x++;
                            return new Lexem(LexType.NotEqual, 0, String.Empty, new Nodes.Node.Coord(currX, currY), new Nodes.Node.Coord(x, y));
                        }
                        throw new LexerException(ParserException.ErrorType.IncorrectLexem, new Nodes.Node.Coord(currX, currY));

                    default:
                        currPos += offSet;
                        x++;
                        offSet = 0;
                        return CreateNewLexem(out offSet);
                }
            }

            public static Lexem GetNextToken()
            {
                x = 0;
                y = 0;
                int offSet;
                Lexem lexem = CreateNewLexem(out offSet);
                currPos += offSet;
                currX += x;
                currY += y;
                return lexem;
            }

            public static Lexem LookAHead()
            {
                x = 0;
                y = 0;
                int offSet;
                Lexem lexem = CreateNewLexem(out offSet);
                return lexem;
            }
        }
    }
}
