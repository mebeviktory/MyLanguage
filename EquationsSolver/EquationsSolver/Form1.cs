using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace EquationsSolver
{
    partial class Form1 : Form
    {
        public Debugger debug = null;
        public Caretaker caret = new Caretaker();
        public bool isDebug = false; 
        
        public Form1()
        {
            InitializeComponent();
        }

        public String ResultText
        {
            get
            {
                return this.ResultBox.Text;
            }
            set
            {
                this.ResultBox.Text = value;
            }
        }

        public String ErrorsText
        {
            get
            {
                return this.ErrorsBox.Text;
            }
            set
            {
                this.ErrorsBox.Text = value;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "c:\\user";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {        
                    string path = openFileDialog1.FileName;
                    Input.Text = File.ReadAllText(path);
                    Project.savePath = openFileDialog1.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Input.Text = "";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = "C:\\user";
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string path = saveFileDialog1.FileName;

                    StreamWriter SW = new StreamWriter(new FileStream(path, FileMode.Create, FileAccess.Write));
                    SW.Write(Input.Text);
                    SW.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not write file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string path = Project.savePath;
            if (path != "")
            {
                StreamWriter SW = new StreamWriter(new FileStream(path, FileMode.Create, FileAccess.Write));
                SW.Write(Input.Text);
                SW.Close();
            }
            saveToolStripMenuItem1.BackColor = Color.Lavender;
        }

        private void runToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            RunReload();
            string inputString = Input.Text;
            Nodes.Program program = Parser.EatProgram(inputString);
            if (program == null)
            {
                ParserErrorWrite();
            }
            else
            {
                ErrorsBox.Text = "";
                try
                {
                    program.Interpret();
                    printWatches();
                }
                catch (InterpretException exception)
                {
                    ErrorsBox.Text = "Interpret Exception";
                    switch (exception.Exception)
                    {
                        case InterpretException.TypeException.DivisionByZero:
                            ErrorsBox.Text = "Division by zero";
                            break;
                        case InterpretException.TypeException.NonInitializedVar:
                            ErrorsBox.Text = "Using non initialized variable";
                            break;
                    }
                }
            }
        }

        private void printWatches()
        {
            
            int countVariables = Memory.VariableValues.Count;
            for (int i = 0; i < countVariables; i++)
            {
                string val = "";
                Nodes.Variable.VariableType fType = Nodes.Variable.VariableType.Float;
                Nodes.Variable.VariableType sType = Nodes.Variable.VariableType.String;
                Nodes.Variable.VariableType type = Memory.VariableValues.Values.ElementAt(i).Type;
                Nodes.Node.Values currElem = Memory.VariableValues.Values.ElementAt(i);

                if (type == fType) val = currElem.ValueOfFloat.ToString();
                if (type == sType) val = '"' + currElem.ValueOfString + '"';

                WatchesBox.Text =
                    WatchesBox.Text + " "
                    + type.ToString() + " "
                    + Memory.VariableValues.Keys.ElementAt(i).ToString()
                    + "=" + val + ";" + "\t";

            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case (Keys.F10):
                {
                    /*int endCoordX = debug.EndcoordOfStatement.X;
                    int startCoord = debug.StartcoordOfStatement;


                    ResultBox.Select(startCoordX, lengthOfStatement)*/
                    debug.OneStep();
                    break;
                }
                case (Keys.F5):
                {
                    RunReload();
                    isDebug = false;
                    break;
                }
            }
        }

        private void RunReload()
        {
            ErrorsBox.Text = "";
            ResultBox.Text = "";
            WatchesBox.Text = "";
            ResultBox.BackColor = Color.White;
        }

        private void ParserErrorWrite()
        {
            ResultBox.Text = "Was entered an incorrect code";

            int errorCounter = Parser.Errors.Count;
            for (int i = 0; i < errorCounter; i++)
            {
                Parser.Error currError = Parser.Errors[i];
                ErrorsBox.Text =
                    currError.TypeError.ToString()
                    + ' ' + currError.Coords.Y.ToString()
                    + ' ' + currError.Coords.X.ToString() + ';';
            }
        }

        private void debugToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RunReload();
            string inputString = Input.Text;

            Nodes.Program program = Parser.EatProgram(inputString);

            if (program == null)
            {
                ParserErrorWrite();
            }
            else if (isDebug)
            {
                string text = "Debug alredy run";
                string caption = "Error";
                MessageBoxButtons button = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;
                MessageBox.Show(text, caption, button, icon);
            }
            else
            {
                debug = new Debugger(program);
                isDebug = true;
            }
        }

        private void stopDebug_Click(object sender, EventArgs e)
        {
            RunReload();
            isDebug = false;
        }

        private void step_Click(object sender, EventArgs e)
        {
            if (!isDebug)
            {
                string text = "Please, run debug";
                string caption = "Error";
                MessageBoxButtons button = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;
                MessageBox.Show(text, caption, button, icon);
            }
            else if (debug.isEnd)
            {
                isDebug = false;
                RunReload();
            }
            else
            {
                debug.OneStep();
                WatchesBox.Text = "";
                printWatches();
                int start = debug.start;
                int length = debug.length;
                Input.SelectionBackColor = Color.White;
                Input.Select(start, length);
                Input.SelectionBackColor = Color.Yellow;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string inp = Input.Text;
            string outp = ResultBox.Text;
            string err = ErrorsBox.Text;
            string wat = WatchesBox.Text;

            Memento mem = new Memento(inp, outp, err, wat);
            caret.ChangeMemento(mem);

            caret.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Memento mem = caret.Load();
            Input.Text = mem.Input;
            ResultBox.Text = mem.Output;
            ErrorsBox.Text = mem.Errors;
            WatchesBox.Text = mem.Watches;
        }
    }
}