using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    internal class CompoundStatement : Statement
    {
        public override Values Interpret()
        {
            return new Values();
        }
    }
}
