using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    internal class Block : Statement
    {
        public readonly StatementList ListOfStatements;

        public Statement LastStatementInBlock { get; set; }

        public Block(StatementList listStatement, Coord sc, Coord ec)
        {
            ListOfStatements = listStatement;
            End = sc;
            Start = ec;
        }

        public override Values Interpret()
        {
            ListOfStatements.Interpret();
            return new Values();
        }
    }
}
