using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    internal sealed class Condition : Statement
    {
        internal enum Comparison
        {
            Greater,
            GreaterOrEqual,
            Less,
            LessOrEqual,
            Equal,
            NotEqual
        }

        public Values True = new Values(null, 1, Variable.VariableType.Float);
        public Values False = new Values(null, 0, Variable.VariableType.Float);
        public readonly Comparison Operation;
        public readonly Expression LeftNode;
        public readonly Expression RightNode;

        public Condition(Expression leftNode, Expression rightNode, Comparison operation, Coord sc, Coord ec)
        {
            if (leftNode == null)
            {
                throw new ArgumentNullException("little args of compare operation");
            }
            if (rightNode == null)
            {
                throw new ArgumentNullException("little args of compare operation");
            }
            LeftNode = leftNode;
            RightNode = rightNode;
            Operation = operation;
            Start = sc;
            End = ec;
        }

        public override Values Interpret()
        {
            switch (Operation)
            {
                case Comparison.Greater:
                    if (LeftNode.Interpret().ValueOfFloat > RightNode.Interpret().ValueOfFloat)
                    {
                        return True;
                    }
                    return False;

                case Comparison.GreaterOrEqual:
                    if (LeftNode.Interpret().ValueOfFloat >= RightNode.Interpret().ValueOfFloat)
                    {
                        return True;
                    }
                    return False;

                case Comparison.Less:
                    if (LeftNode.Interpret().ValueOfFloat < RightNode.Interpret().ValueOfFloat)
                    {
                        return True;
                    }
                    return False;

                case Comparison.LessOrEqual:
                    if (LeftNode.Interpret().ValueOfFloat <= RightNode.Interpret().ValueOfFloat)
                    {
                        return True;
                    }
                    return False;

                case Comparison.Equal:
                    if (LeftNode.Interpret().ValueOfFloat == RightNode.Interpret().ValueOfFloat)
                    {
                        return True;
                    }
                    return False;

                case Comparison.NotEqual:
                    if (LeftNode.Interpret().ValueOfFloat != RightNode.Interpret().ValueOfFloat)
                    {
                        return True;
                    }
                    return False;

                default: return new Values();
            }
        }
    }
}
