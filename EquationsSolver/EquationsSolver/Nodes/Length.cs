using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    internal class Length : Expression
    {
        public readonly string VarName;

        internal Length(string varName)
        {
            VarName = varName;
        }

        public override Values Interpret()
        {      
            if (!Memory.VariableValues.ContainsKey(VarName))
            {
                throw new InterpretException(InterpretException.TypeException.NonInitializedVar);
            }
            float length = Memory.VariableValues[VarName].ValueOfString.Length;
            Variable.VariableType ftype = Variable.VariableType.Float;
            return new Values(null, length, ftype);
        }         
    }
}
