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
            if (Statements.Count == 0)
            {
                return new Values();
            }
            Statement CurrStatement = Statements[0];
            while (CurrStatement.NextStatement != null)
            {
                //Console.WriteLine(CurrStatement.Start.Y);
                if (Memory.Breakpoints.Contains(CurrStatement.Start.Y))
                {
                    return new Values();
                }
                CurrStatement.Interpret();
                CurrStatement = CurrStatement.NextStatement;
            }
            if (CurrStatement != null && !Memory.Breakpoints.Contains(CurrStatement.Start.Y))
            {
                CurrStatement.Interpret();
            }
            return new Values();
        }
    }
}