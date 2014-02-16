using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver
{

    internal class Memory
    {

        public static List<int> Breakpoints
        {
            get;
            set;
        }

        public static Dictionary<string, Nodes.Node.Values> VariableValues
        {
            get;
            set;
        }
        public static Dictionary<string, Nodes.Statement> LabelsToStatement
        {
            get;
            set;
        }
    }
}
