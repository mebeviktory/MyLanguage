using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    class For : CompoundStatement
    {
        public readonly Statement Assignment, Delta;
        public readonly Condition Condition;
        public readonly Block Block;


        public For(Condition currCondition, Block currBlock, Assignment currAssignment, Assignment currDelta, Coord sc, Coord ec)
        {
            if (currCondition == null)
            {
                throw new ArgumentNullException("cant find condition");
            }
            if (currBlock == null)
            {
                throw new ArgumentNullException("cant find Block");
            } 
            if (currAssignment == null)
            {
                throw new ArgumentNullException("cant find Assignment");
            }
            if (currDelta == null)
            {
                throw new ArgumentNullException("cant find Delta");
            }
            Condition = currCondition;
            Block = currBlock;
            Assignment = currAssignment;
            Delta = currDelta;
            Start = sc;
            End = ec;
        }

        public override Values Interpret()
        {
            Assignment.Interpret();
            while (Condition.Interpret().ValueOfFloat == Condition.True.ValueOfFloat)
            {
                Block.Interpret();
                Delta.Interpret();
            }
            return new Values();
        }
    }
}
