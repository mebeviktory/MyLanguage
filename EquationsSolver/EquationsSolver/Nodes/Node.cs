using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    abstract class Node
    {
        public abstract Values Interpret();
        public Coord Start
        { get; set; }
        public Coord End
        { get; set; }

        public struct Values
        {
            public readonly string ValueOfString;
            public readonly float ValueOfFloat;
            public readonly Nodes.Variable.VariableType Type;

            public Values(string valueOfString, float valueOfFloat, Nodes.Variable.VariableType type)
            {
                this.ValueOfFloat = valueOfFloat;
                this.ValueOfString = valueOfString;
                this.Type = type;
            }
        }

        public struct Coord
        {
            public readonly int X;
            public readonly int Y;

            public Coord(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
    }
}
