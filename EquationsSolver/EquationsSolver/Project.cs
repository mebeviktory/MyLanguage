using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace EquationsSolver
{
    internal class Project
    {
        public static Form1 form = new Form1();

        public static string savePath = "";

        [STAThread]
        public static void Main(string[] args)
        {
            Application.Run(form);
        }
    }
}