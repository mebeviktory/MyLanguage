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
            switch (Operation)
            {
                case BinaryOperationType.Plus:
                    return new Values(null, LeftNode.Interpret().ValueOfFloat + RightNode.Interpret().ValueOfFloat, Variable.VariableType.Float);

                case BinaryOperationType.Minus:
                    return new Values(null, LeftNode.Interpret().ValueOfFloat - RightNode.Interpret().ValueOfFloat, Variable.VariableType.Float);

                case BinaryOperationType.Multiply:
                    return new Values(null, LeftNode.Interpret().ValueOfFloat * RightNode.Interpret().ValueOfFloat, Variable.VariableType.Float);

                case BinaryOperationType.Division:
                    float rightNode = RightNode.Interpret().ValueOfFloat;
                    if (rightNode == 0)
                    {
                        throw new InterpretException(InterpretException.TypeException.DivisionByZero);
                    }
                    return new Values(null, LeftNode.Interpret().ValueOfFloat / RightNode.Interpret().ValueOfFloat, Variable.VariableType.Float);

                case BinaryOperationType.Degree:
                    return new Values(null, (float)Math.Pow(LeftNode.Interpret().ValueOfFloat, RightNode.Interpret().ValueOfFloat), Variable.VariableType.Float);

                default:
                    throw new InterpretException(InterpretException.TypeException.Error) ;
            }
        }
    }
}
