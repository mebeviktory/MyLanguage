﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace EquationsSolver
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
            openFileDialog1.InitialDirectory = "c:\\";
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
            saveFileDialog1.InitialDirectory = "c:\\";
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
        }

        private void runToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Nodes.Program program = Parser.EatProgram(Input.Text);
            if (program == null)
            {
                ResultBox.Text = "Was entered an incorrect code";

                for (int i = 0; i < Parser.Errors.Count; i++)
                {
                    ErrorsBox.Text =
                        Parser.Errors[i].TypeError.ToString()
                        + ' ' + Parser.Errors[i].Coords.Y.ToString()
                        + ' ' + Parser.Errors[i].Coords.X.ToString() + ';';
                }
            }
            else
            {
                ErrorsBox.Text = "";
                try
                {
                    program.Interpret();
                    for (int i = 0; i < Memory.VariableValues.Count; i++)
                    {
                        string val = "";
                        Nodes.Variable.VariableType type = Memory.VariableValues.Values.ElementAt(i).Type;
                        if (type == Nodes.Variable.VariableType.Float) val = Memory.VariableValues.Values.ElementAt(i).ValueOfFloat.ToString();
                        if (type == Nodes.Variable.VariableType.String) val = '"' + Memory.VariableValues.Values.ElementAt(i).ValueOfString + '"';

                        WatchesBox.Text =
                            WatchesBox.Text + " "
                            + type.ToString() + " "
                            + Memory.VariableValues.Keys.ElementAt(i).ToString()
                            + "=" + val + ";" + "\t";

                    }
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

        private void ResultBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void WatchesBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}