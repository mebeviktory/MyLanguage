using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver
{
    internal sealed class InterpretException : Exception
    {
        public readonly TypeException Exception;
        public enum TypeException
        {
            DivisionByZero,
            NonInitializedVar,
            Error
        }

        public InterpretException(TypeException exception)
        {
            Exception = exception;
        }
    }
}
