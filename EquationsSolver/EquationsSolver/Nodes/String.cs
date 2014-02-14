using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    internal class String : Node
    {
        public readonly string Value;

        internal String(string value, Coord nodeStart)
        {
            Value = value;
            Start = nodeStart;
        }

        public override Values Interpret()
        {
            return new Values(Value, 0, Variable.VariableType.String);
        }
    }
}
