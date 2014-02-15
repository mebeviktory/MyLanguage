using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    class While : CompoundStatement
    {
        public readonly Condition Condition;
        public readonly Block Block;

        public While(Condition currCondition, Block currBlock, Coord sc, Coord ec)
        {
            if (currCondition == null)
            {
                throw new ArgumentNullException("cant find Condition");
            }
            if (currBlock == null)
            {
                throw new ArgumentNullException("cant find Block");
            }
            Condition = currCondition;
            Block = currBlock;
            Start = sc;
            End = ec;
        }

        public override Values Interpret()
        {
            while (Condition.Interpret().ValueOfFloat == Condition.True.ValueOfFloat)
            {
                Block.Interpret();
            }
            return new Values();
        }
    }
}
