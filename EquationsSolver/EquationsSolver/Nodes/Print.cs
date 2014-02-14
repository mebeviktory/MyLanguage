using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EquationsSolver.Nodes
{
    internal class Print : Statement
    {
        public readonly string StrForPrint;
        public readonly Expression Expr;
        public readonly IPrint PrintInterface;
        public readonly List<Expression> Arguments;
        public readonly MatchCollection Matches;

        public Print(Expression expr,  IPrint printer)
        {
            Expr = expr;
            PrintInterface = printer;
        }

        public Print(string str, IPrint printer)
        {
            StrForPrint = str;
            PrintInterface = printer;
        }

        public Print(string str, MatchCollection mtch, List <Nodes.Expression> arg, IPrint printer)
        {
            StrForPrint = str;
            Matches = mtch;
            Arguments = arg;
            PrintInterface = printer;

        }

        public override Values Interpret()
        {
           if (StrForPrint != null)
            {
                if (Matches != null || Arguments != null)
                {
                    string currString = StrForPrint;
                    foreach (Match match in Matches)
                    {
                        Match currMatch = Regex.Match(match.Value, "[0-9]+");
                        int result;
                        int.TryParse(currMatch.Value, out result);
                        if (result > Arguments.Count() - 1)
                        {
                            throw new InterpretException(InterpretException.TypeException.Error);
                        }
                        currString = currString.Replace(match.Value, Arguments[result].Interpret().ValueOfFloat.ToString());
                    }
                    Project.form.ResultText = Project.form.ResultText + currString + " ";
                    return new Values();
                }
                Project.form.ResultText = Project.form.ResultText + StrForPrint + " ";
                return new Values();
            }
           else if (Expr.Interpret().Type == Variable.VariableType.String)
           {
               Project.form.ResultText = Project.form.ResultText + Expr.Interpret().ValueOfString + " ";
               return new Values();
           }
           else
           {
            Project.form.ResultText = Project.form.ResultText + Expr.Interpret().ValueOfFloat.ToString() + " ";
            return new Values();
           }
        }
    }
}
