using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    internal class Slice : Expression
    {
        public readonly string VarName;
        public readonly Expression Counter;

        internal Slice(string variableName, Expression count)
        {
            if (variableName == null)
            {
                throw new ArgumentNullException("variableName");
            }
            VarName = variableName;
            Counter = count;
        }

        public override Values Interpret()
        {
            string name = VarName + "[" + Counter.Interpret().ValueOfFloat.ToString() + "]";
            if (!Memory.VariableValues.ContainsKey(name))
            {
                throw new InterpretException(InterpretException.TypeException.NonInitializedVar);
            }
            return Memory.VariableValues[name];
        }
    }
}
