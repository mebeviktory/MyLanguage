using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    internal class GoTo : Statement
    {
        public readonly string Label;


        internal GoTo(string label, Coord sc, Coord ec)
        {
            if (label == null)
            {
                throw new ArgumentNullException("Print");
            }
            Label = label;
            Start = sc;
            End = ec;
        }

        public override Values Interpret()
        {
            Memory.LabelsToStatement[Label].Interpret();
            NextStatement = Memory.LabelsToStatement[Label].NextStatement;
            return new Values();
        }
    }
}
