using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EquationsSolver
{
    class Caretaker
    {
        public Memento currMemento;
        public string Path;

        public Caretaker()
        {
            currMemento = new Memento("","","","");
            Path = "C://mem/Memento.txt";
        }

        public void ChangeMemento(Memento memento)
        {
            currMemento = memento;
        }
        public void Save()
        {
            Console.WriteLine(currMemento.Input);
            StreamWriter SW = new StreamWriter(new FileStream(Path, FileMode.Create, FileAccess.Write));
            SW.Write(currMemento.Input + "|" + currMemento.Output + "|" + currMemento.Errors + "|" + currMemento.Watches + "|");
            SW.Close();
        }
        public Memento Load()
        {
            string load = File.ReadAllText("C://mem/Memento.txt");
            string[] split = load.Split(new Char[] { '|' });

            this.ChangeMemento(new Memento(split[0], split[1], split[2], split[3]));
            return currMemento;
        }
    }
}
