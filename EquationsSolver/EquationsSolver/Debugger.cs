using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationsSolver
{
    class Debugger
    {
        public Nodes.Node.Coord StartCoord;
        public Nodes.Node.Coord EndCoord;
        public int length;
        public int start;
        public Nodes.Statement currStatement;
        private Nodes.Program currProgram;

        public Debugger(Nodes.Program program)
        {
            currProgram = program;
            currStatement = program.ListOfStatements.Statements[0];
        }

        public void OneStep()
        {
            StartCoord = currStatement.Start;
            EndCoord = currStatement.End;
            start = StartCoord.X;
            length = EndCoord.X - StartCoord.X;

            Console.WriteLine(EndCoord.X);
            Console.WriteLine(EndCoord.X);
            Console.WriteLine(StartCoord.Y);
            Console.WriteLine(StartCoord.Y);

            currStatement.Interpret();
            currStatement = currStatement.NextStatement;
            Console.WriteLine("step");
        }
    }
}
