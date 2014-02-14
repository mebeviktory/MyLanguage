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

        public Label(string nameLabel, Statement currStatement)
        {
            Statement = currStatement;
            LabelName = nameLabel;
        }

        public override Values Interpret()
        {
            Memory.LabelsToStatement [LabelName] = Statement;
            NextStatement = Memory.LabelsToStatement[LabelName].NextStatement;
            return new Values();
        }
    }
}
