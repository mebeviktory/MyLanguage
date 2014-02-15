using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GUI
{
partial class Form1 : Form
    {

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

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

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
            EquationsSolver.Nodes.Program program = EquationsSolver.Parser.EatProgram(Input.Text);
            if (program == null)
            {
                ResultBox.Text = "Was entered an incorrect code";

                for (int i = 0; i < EquationsSolver.Parser.Errors.Count; i++)
                {
                    ErrorsBox.Text =
                        EquationsSolver.Parser.Errors[i].TypeError.ToString()
                        + ' ' + EquationsSolver.Parser.Errors[i].Coords.Y.ToString()
                        + ' ' + EquationsSolver.Parser.Errors[i].Coords.X.ToString() + ';';
                }
            }
            else
            {
                ErrorsBox.Text = "";
                try
                {
                    program.Interpret();
                    for (int i = 0; i < EquationsSolver.Memory.VariableValues.Count; i++)
                    {
                        string val = "";
                        EquationsSolver.Nodes.Variable.VariableType type = EquationsSolver.Memory.VariableValues.Values.ElementAt(i).Type;
                        if (type == EquationsSolver.Nodes.Variable.VariableType.Float) val = EquationsSolver.Memory.VariableValues.Values.ElementAt(i).ValueOfFloat.ToString();
                        if (type == EquationsSolver.Nodes.Variable.VariableType.String) val = '"' + EquationsSolver.Memory.VariableValues.Values.ElementAt(i).ValueOfString + '"';

                        WatchesBox.Text =
                            WatchesBox.Text + " "
                            + type.ToString() + " "
                            + EquationsSolver.Memory.VariableValues.Keys.ElementAt(i).ToString()
                            + "=" + val + ";" + "\t";

                    }
                }
                catch (EquationsSolver.InterpretException exception)
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

        private void ResultBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void WatchesBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void debugToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
    }
}