using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver
{
    class Memento
    {
        public string Input;
        public string Output;
        public string Watches;
        public string Errors;

        public Memento(string input, string output, string watches, string errors)
        {
            Input = input;
            Output = output;
            Watches = watches;
            Errors = errors;
        }
    }
}
