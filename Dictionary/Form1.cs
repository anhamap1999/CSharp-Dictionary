using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace Dictionary
{
    public partial class Dictionary : Form
    {
        public Dictionary()
        {
            InitializeComponent();
            WoDMeaningLb.Text = "acvc.\n ákdk";
        }

        private void SearchBox_Enter(object sender, EventArgs e)
        {
            if (SearchBox.Text == "Search some word...")
            {
                SearchBox.Text = "";
                SearchBox.ForeColor = Color.Black;
            }
        }

        private void SearchBox_Leave(object sender, EventArgs e)
        {
            if (SearchBox.Text == "")
            {
                SearchBox.Text = "Search some word...";
                SearchBox.ForeColor = Color.Silver;
            }
        }

        private void Dictionary_Load(object sender, EventArgs e)
        {
            Load_Data();
        }

        DataTable tableWord = new DataTable();
        DataTable tableDefinition = new DataTable();


        void Load_Data()
        {
            tableWord.Columns.Add("word", typeof(string));
            tableWord.Columns.Add("spelling", typeof(string));
            StreamReader sr = new StreamReader("D:\\Dictionary\\anhviet109K.txt");
            string str;
            string word = null;
            string definition = null;
            string type = null;
            string spelling = null;
            string synonym = null;
            string explain = null;
            tableDefinition.Columns.Add("word", typeof(string));
            tableDefinition.Columns.Add("type", typeof(string));
            tableDefinition.Columns.Add("definition", typeof(string));
            tableDefinition.Columns.Add("synonym", typeof(string));
            tableDefinition.Columns.Add("explain", typeof(string));
            tableDefinition.Columns.Add("examples", typeof(List<Dictionary<string, string>>));
            Dictionary<string, string> example = new Dictionary<string, string>();
            example["example"] = "";
            example["explain"] = "";
            List<Dictionary<string, string>> examples = new List<Dictionary<string, string>>();
            while ((str = sr.ReadLine()) != null)
            {
                if (str == "")
                {
                    word = null;
                    type = null;
                    definition = null;
                    spelling = null;
                    synonym = null;
                    explain = null;
                    examples.Clear();
                }
                else
                {
                    if (str.StartsWith("@"))
                    {
                        if (word != null)
                        {
                            tableDefinition.Rows.Add(word, type, definition, synonym, explain, examples);
                            examples.Clear();
                        }
                        if (str.Contains(" /"))
                        {
                            word = str.Substring(1, str.IndexOf(" /") - 1);
                        } else
                        {
                            word = str.Substring(1);
                        }
                        if (str.Contains(" /"))
                        {
                            spelling = str.Substring(str.IndexOf(" /") + 1);
                        } else
                        {
                            spelling = "";
                        }
                        tableWord.Rows.Add(word, spelling);
                    }
                    else if (str.StartsWith("*"))
                    {
                        if (type != null)
                        {
                            tableDefinition.Rows.Add(word, type, definition, synonym, explain, examples);
                            examples.Clear();
                        }
                        type = str.Remove(0, 1).Trim();
                    }
                    else if (str.StartsWith("-"))
                    {
                        if (definition != null)
                        {
                            tableDefinition.Rows.Add(word, type, definition, synonym, explain, examples);
                            examples.Clear();
                        }
                        definition = str.Remove(0, 1).Trim();
                    } else if (str.StartsWith("!"))
                    {
                        synonym = str.Remove(0, 1).Trim();
                    } else if (str.StartsWith("+"))
                    {
                        explain = str.Remove(0, 1).Trim();
                    }
                    else if (str.StartsWith("="))
                    {
                        if (str.Contains("+"))
                        {
                        example["example"] = str.Remove(0, 1).Substring(0, str.IndexOf("+") - 1).Trim();
                        example["explain"] = str.Substring(str.IndexOf("+") + 1).Trim();
                        } else
                        {
                            example["example"] = str.Remove(0, 1).Trim();
                            example["explain"] = "";
                        }
                        examples.Add(example);
                    }
                }
            }
            sr.Close();
        }
    }
}
