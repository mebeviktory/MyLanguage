using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    internal class StatementList : Node
    {
        public List<Statement> Statements{get; set;}

        public override Values Interpret()
        {
            Statement CurrStatement = Statements[0];
            while (CurrStatement.NextStatement != null)
            {
                CurrStatement.Interpret();
                CurrStatement = CurrStatement.NextStatement;
            }
            CurrStatement.Interpret();
            return new Values();
        }
    }
}
