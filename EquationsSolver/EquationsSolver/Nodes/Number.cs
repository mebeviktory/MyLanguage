using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    internal class Number : Expression
    {
        public readonly float Value;

        internal Number(float value, Coord nodeStart)
        {
            Value = value;
            Start = nodeStart;
        }

        public override Values Interpret()
        {
            return new Values(null, Value, Variable.VariableType.Float);
        }
    }
}
