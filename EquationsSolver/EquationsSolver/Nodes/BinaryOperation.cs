using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    internal class BinaryOperation : Expression
    {
        internal enum BinaryOperationType
        {
            Plus = '+',
            Minus = '-',
            Multiply = '*',
            Division = '/',
            Degree = '^'
        }

        public readonly BinaryOperationType Operation;
        public readonly Expression LeftNode;
        public readonly Expression RightNode;

        public BinaryOperation(Expression leftNode, Expression rightNode, BinaryOperationType operation, Coord nodeStart)
        {
            if (leftNode == null)
            {
                throw new ArgumentNullException("little arguments of ariphmetic operation");
            }
            if (rightNode == null)
            {
                throw new ArgumentNullException("little arguments of ariphmetic operation");
            }
            LeftNode = leftNode;
            RightNode = rightNode;
            Operation = operation;
            Start = nodeStart;
        }

        public override Values Interpret()
        {
            Variable.VariableType ftype = Variable.VariableType.Float;
            switch (Operation)
            {
                
                case BinaryOperationType.Plus:
                {
                    float val = LeftNode.Interpret().ValueOfFloat + RightNode.Interpret().ValueOfFloat;
                    return new Values(null, val, ftype);
                }
                case BinaryOperationType.Minus:
                {
                    float val = LeftNode.Interpret().ValueOfFloat - RightNode.Interpret().ValueOfFloat;
                    return new Values(null, val, ftype);
                }

                case BinaryOperationType.Multiply:
                {
                    float val = LeftNode.Interpret().ValueOfFloat * RightNode.Interpret().ValueOfFloat;
                    return new Values(null, val, ftype);
                }

                case BinaryOperationType.Division:
                {
                    float rightNode = RightNode.Interpret().ValueOfFloat;
                    if (rightNode == 0)
                    {
                        throw new InterpretException(InterpretException.TypeException.DivisionByZero);
                    }
                    else
                    {
                        float val = LeftNode.Interpret().ValueOfFloat / rightNode;
                        return new Values(null, val, ftype);
                    }
                }

                case BinaryOperationType.Degree:
                {
                    float val = (float)Math.Pow(LeftNode.Interpret().ValueOfFloat,RightNode.Interpret().ValueOfFloat);
                    return new Values(null, val, ftype);
                }

                default:
                    throw new InterpretException(InterpretException.TypeException.Error) ;
            }
        }
    }
}
