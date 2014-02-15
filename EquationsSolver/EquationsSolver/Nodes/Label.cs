using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    internal class Label : Statement
    {
        public readonly string LabelName;
        public readonly Statement Statement;

        public Label(string nameLabel, Statement currStatement, Coord sc, Coord ec)
        {
            Statement = currStatement;
            LabelName = nameLabel;
            Start = sc;
            End = ec;
        }

        public override Values Interpret()
        {
            Memory.LabelsToStatement [LabelName] = Statement;
            NextStatement = Memory.LabelsToStatement[LabelName].NextStatement;
            return new Values();
        }
    }
}
