using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver
{
    public interface IPrint
    {
        void ConsolePrint(string tPrint);
    }

    public class Print : IPrint
    {
        public void ConsolePrint(string tPrint)
        {
            Console.Write(tPrint);
        }
    }
}
