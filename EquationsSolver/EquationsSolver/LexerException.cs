using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver
{
    class LexerException : Exception
    {
        public readonly ParserException.ErrorType Exception;
        public readonly Nodes.Node.Coord ErrorCoord;

        public LexerException(ParserException.ErrorType exception, Nodes.Node.Coord errorCoord)
        {
            Exception = exception;
            ErrorCoord = errorCoord;
        }
    }
}
