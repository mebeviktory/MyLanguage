using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    internal class If : CompoundStatement
    {
        public readonly Condition Condition;
        public readonly Block BlockIf, BlockElse;

        public If(Condition currCondition, Block currBlockIf, Block currBlockElse, Coord sc, Coord ec)
        {
            if (currCondition == null)
            {
                throw new ArgumentNullException("cant find Condition"); ;
            }
            if (currBlockIf == null)
            {
                throw new ArgumentNullException("cant find BlockIf");
            }
            Condition = currCondition;
            BlockIf = currBlockIf;
            BlockElse = currBlockElse;
            Start = sc;
            End = ec;
        }

        public override Values Interpret()
        {
            if (Condition.Interpret().ValueOfFloat == Condition.True.ValueOfFloat)
            {
                return BlockIf.Interpret();
            }
            if (BlockElse != null)
            {
                return BlockElse.Interpret();
            }
            return new Values();
        }
    }
}
