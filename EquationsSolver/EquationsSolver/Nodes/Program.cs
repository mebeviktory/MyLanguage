using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    internal class Program : Node
    {
        public readonly StatementList ListOfStatements;

        public Program(StatementList listOfStatements)
        {
            if (listOfStatements == null)
            {
                throw new ArgumentNullException(" Program doesn't found");
            }
            ListOfStatements = listOfStatements;
        }


        public override Values Interpret()
        {
            ListOfStatements.Interpret();
            return new Values();
        }
    }
}
