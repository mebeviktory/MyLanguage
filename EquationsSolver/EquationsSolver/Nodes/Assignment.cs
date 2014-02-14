using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    internal class Assignment : Statement
    {
        public readonly string VarName;
        public readonly Expression Expr;
        public readonly Expression Count;
        public readonly string Str;
        public readonly Statement Statement;
        public readonly float diff;
        Parser.Lexer.LexType VarType;

        public Assignment(string nameVariable, Expression currExpr)
        {
            Expr = currExpr;
            VarName = nameVariable;
        }

        public Assignment(string nameVariable)
        {
            VarName = nameVariable;
        }

        public Assignment(string nameOfArray, Expression counter, Expression currExpr)
        {
            VarName = nameOfArray;
            Count = counter;
            Expr = currExpr;

        }

        public Assignment(string nameVariable, string currStr)
        {
            Str = currStr;
            VarName = nameVariable;
            VarType = Parser.Lexer.LexType.Str;

        }

        public Assignment(string nameVariable, float one)
        {
            diff = one;
            VarName = nameVariable;
        }

        public Assignment(string nameLabel, Statement currStatement)
        {
            Statement = currStatement;
            VarName = nameLabel;
        }

        public override Values Interpret()
        {
            if (diff != 0.0)
            {
                if (!Memory.VariableValues.ContainsKey(VarName))
                {
                    throw new InterpretException(InterpretException.TypeException.NonInitializedVar);
                }
                Memory.VariableValues[VarName] = new Values(null, Memory.VariableValues[VarName].ValueOfFloat + diff, Variable.VariableType.Float);
            }
            else if (Count != null)
            {
                Values count = Count.Interpret();
                Values expr = Expr.Interpret();
                string name = VarName + '[' + count.ValueOfFloat.ToString() + ']';
                if (!Memory.VariableValues.ContainsKey(name))
                {
                    Memory.VariableValues.Add(name, expr);
                }
                else
                {
                    Memory.VariableValues[name] = expr;
                }
            }
            /*else if (Statement != null)
            {
                if (!Memory.VariableValues.ContainsKey(VarName))
                {
                    Memory.VariableValues.Add(VarName,);
                }
                Memory.LabelsToStatement[VarName] = Statement;
                Statement.NextStatement = this.NextStatement;
            }*/
            else if (Str != null)
            {
                if (!Memory.VariableValues.ContainsKey(VarName))
                {
                    throw new InterpretException(InterpretException.TypeException.NonInitializedVar);
                }
                Memory.VariableValues[VarName] = new Values(Str, 0, Variable.VariableType.String);
            }
            else if (Expr != null)
            {
                if (!Memory.VariableValues.ContainsKey(VarName))
                {
                    throw new InterpretException(InterpretException.TypeException.NonInitializedVar);
                }
                Memory.VariableValues[VarName] = new Values(null, Expr.Interpret().ValueOfFloat, Variable.VariableType.Float);
            }
            

            return new Values();

        }
    }
}
