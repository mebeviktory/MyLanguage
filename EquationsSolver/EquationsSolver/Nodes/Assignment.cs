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
        public readonly float Diff;
        

        //float
        public Assignment(string nameVariable, Expression currExpr, Coord sc, Coord ec)
        {
            Expr = currExpr;
            VarName = nameVariable;
            Start = sc;
            End = ec;

        }

        // for length
        public Assignment(string nameVariable, Coord sc, Coord ec)
        {
            VarName = nameVariable;
            Start = sc;
            End = ec;
        }

        //array
        public Assignment(string nameOfArray, Expression counter, Expression currExpr, Coord sc, Coord ec)
        {
            VarName = nameOfArray;
            Count = counter;
            Expr = currExpr;
            Start = sc;
            End = ec;
        }

        //string
        public Assignment(string nameVariable, string currStr, Coord sc, Coord ec)
        {
            Str = currStr;
            VarName = nameVariable;
            Start = sc;
            End = ec;
        }

        //inc
        public Assignment(string nameVariable, float one, Coord sc, Coord ec)
        {
            Diff = one;
            VarName = nameVariable;
            Start = sc;
            End = ec;
        }

        public override Values Interpret()
        {

            Variable.VariableType sType = Variable.VariableType.Float;
            Variable.VariableType fType = Variable.VariableType.Float;

            //i++
            if (Diff != 0.0)
            {
                
                if (!Memory.VariableValues.ContainsKey(VarName))
                {
                    throw new InterpretException(InterpretException.TypeException.NonInitializedVar);
                }
                float valCurrVariable = Memory.VariableValues[VarName].ValueOfFloat;
                Memory.VariableValues[VarName] = new Values(null, valCurrVariable + Diff, fType);
            }

            // array assignment
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

            //string assignment
            else if (Str != null)
            {
                if (!Memory.VariableValues.ContainsKey(VarName))
                {
                    throw new InterpretException(InterpretException.TypeException.NonInitializedVar);
                }
                Memory.VariableValues[VarName] = new Values(Str, 0, sType);
            }

            // float assignment
            else if (Expr != null)
            {
                if (!Memory.VariableValues.ContainsKey(VarName))
                {
                    throw new InterpretException(InterpretException.TypeException.NonInitializedVar);
                }
                Memory.VariableValues[VarName] = new Values(null, Expr.Interpret().ValueOfFloat, fType);
            }
            return new Values();

        }
    }
}
