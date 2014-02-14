using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    internal class GoTo : Statement
    {
        public readonly string Label;

        internal GoTo(string label, Coord nodeStart)
        {
            if (label == null)
            {
                throw new ArgumentNullException("Print");
            }
            Label = label;
            Start = nodeStart;
        }

        public override Values Interpret()
        {
            Memory.LabelsToStatement[Label].Interpret();
            NextStatement = Memory.LabelsToStatement[Label].NextStatement;
            return new Values();
        }
    }
}
