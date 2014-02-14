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

        public While(Condition currCondition, Block currBlock)
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
