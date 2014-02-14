using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    internal class Statement : Node
    {
        public Statement NextStatement{get; set;}

        public override Values Interpret()
        {
            return new Values();
        }
    }
}
