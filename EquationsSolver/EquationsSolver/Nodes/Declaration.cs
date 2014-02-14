using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver.Nodes
{
    internal class Declaration : Statement
    {
        public readonly string VarName;
        public readonly Expression Expr;
        public readonly string Str;
        public readonly Statement Statement;
        public readonly float Length;

        public Declaration(string nameVariable, Expression currExpr)
        {
            Expr = currExpr;
            VarName = nameVariable;
        }

        public Declaration(string nameVariable, string currStr, float length)
        {
            Str = currStr;
            VarName = nameVariable;
            Length = length;
        }

        public override Values Interpret()
        {
            if (Memory.VariableValues.ContainsKey(VarName))
            {
                throw new InterpretException(InterpretException.TypeException.Error);//change exception;
            }
            if (Str != null)
            {
                for (int i = 0; i <= Str.Length - 1; i++)
                {
                    string name = VarName+ '[' + i.ToString() + ']';
                    Memory.VariableValues[name] = new Nodes.Node.Values(Str[i].ToString(), Str[i].GetHashCode(), Nodes.Variable.VariableType.String);
                }
                Memory.VariableValues[VarName] = new Values(Str,0, Variable.VariableType.String);
            }
            else
            {
                Memory.VariableValues[VarName] = new Values(null, Expr.Interpret().ValueOfFloat, Variable.VariableType.Float);
            }
            return new Values();
        }
    }
}
