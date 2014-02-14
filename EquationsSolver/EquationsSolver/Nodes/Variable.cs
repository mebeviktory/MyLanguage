using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    internal class Variable : Expression
    {
        public readonly string VarName;
        public enum VariableType
        {
            String,
            Float
        }

        internal Variable(string variableName, Coord nodeStart)
        {
            if (variableName == null)
            {
                throw new ArgumentNullException("variableName");
            }
            VarName = variableName;
            Start = nodeStart;
        }

        public override Values Interpret()
        {
            if (!Memory.VariableValues.ContainsKey(VarName))
            {
                throw new InterpretException(InterpretException.TypeException.NonInitializedVar);
            }
            return Memory.VariableValues[VarName];
        }
    }
}