using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver
{
    class ParserException : Exception
    {
        public ErrorType Exception;
        public readonly Nodes.Node.Coord ErrorCoord;
        public enum ErrorType
        {
            AbsenceSemiclon,
            IncorrectSemicolon,
            AbsenceEqual,
            AbsenceOpenBracket,
            AbsenceCloseBracket,
            AbsenceCloseBracketOfSlice,
            IncorrectCondition,
            ErrorWithBrackets,
            ErrorWithBracketsOfBlock,
            ErrorWithOperands,
            IncorrectStatement,
            IncorrectFor,
            IncorrectWhile,
            IncorrectIf,
            IncorrectAssignment,
            IncorrectPrint,
            IncorrectBlock,
            IncorrectLexem,
            IncorrectNumber,
            IncorrectIndex,
            IncorrectInitialization,
            SecondInitialization,
            IncorrectSymbol,
            ErrorWithFormatString,
            ErrorWithLabel,
            AbsenceOpenBracketOfLen,
            AbsenceCloseBracketOfLen,
            AbsenceOpenBracketOfSlice
        }

        public ParserException(ErrorType exception, Nodes.Node.Coord errorCoord)
        {
            Exception = exception;
            ErrorCoord = errorCoord;
        }
    }
}
