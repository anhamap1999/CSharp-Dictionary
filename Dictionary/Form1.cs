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
using System.Net;

namespace Dictionary
{
    public partial class Dictionary : Form
    {
        public Dictionary()
        {
            InitializeComponent();
            PicOfWordPb.ImageLocation = "https://cdn.britannica.com/22/206222-131-E921E1FB/Domestic-feline-tabby-cat.jpg";
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
            Load_Dictionary();
            Generate_Letter_Buttons();
        }

        DataTable tableWord = new DataTable();
        DataTable tableDefinition = new DataTable();

        Dictionary<char, Button> letterButtons = new Dictionary<char, Button>();
        char selectedChar = Char.Parse("a");

        string searchText = "";

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
                    //word = null;
                    //type = null;
                    //definition = null;
                    //spelling = null;
                    //synonym = null;
                    //explain = null;
                    //examples.Clear();
                }
                else
                {
                    if (str.StartsWith("@"))
                    {
                        if (word != null)
                        {
                            tableDefinition.Rows.Add(word, type, definition, synonym, explain, examples);
                            word = null;
                            type = null;
                            definition = null;
                            spelling = null;
                            synonym = null;
                            explain = null;
                            examples.Clear();
                        }
                        if (str.Contains(" /"))
                        {
                            word = str.Substring(1, str.IndexOf(" /") - 1);
                        }
                        else
                        {
                            word = str.Substring(1);
                        }
                        if (str.Contains(" /"))
                        {
                            spelling = str.Substring(str.IndexOf(" /") + 1);
                        }
                        else
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
                    }
                    else if (str.StartsWith("!"))
                    {
                        synonym = str.Remove(0, 1).Trim();
                    }
                    else if (str.StartsWith("+"))
                    {
                        explain = str.Remove(0, 1).Trim();
                    }
                    else if (str.StartsWith("="))
                    {
                        if (str.Contains("+"))
                        {
                            example["example"] = str.Remove(0, 1).Substring(0, str.IndexOf("+") - 1).Trim();
                            example["explain"] = str.Substring(str.IndexOf("+") + 1).Trim();
                        }
                        else
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

        void Load_Dictionary()
        {
            Random rnd = new Random();
            int _rnd = rnd.Next(tableWord.Rows.Count);
            string word = tableWord.Rows[_rnd][0].ToString();
            while (!Check_Url_Valid("https://ssl.gstatic.com/dictionary/static/sounds/oxford/" + word + "--_gb_1.mp3"))
            {
                _rnd = rnd.Next(tableWord.Rows.Count);
                word = tableWord.Rows[_rnd][0].ToString();
            }
            WoDlb.Text = word;
            WoDSpellingLb.Text = tableWord.Rows[_rnd][1].ToString();
            DataRow r = tableDefinition.Select("word = '" + word + "'").FirstOrDefault();
            //DataRow[] r = tableDefinition.Select("word LIKE 'alumna'");
            //foreach (DataRow row in r)
            //{
            //    Console.WriteLine(row[0]);

            //}
            Console.WriteLine(r[0].ToString());
            WoDTypeLb.Text = "(" + r[1].ToString() + ")";
            WoDMeaningLb.Text = r[2].ToString();

        }

        void Generate_Letter_Buttons()
        {
            letterButtons.Add(Char.Parse("a"), ABtn);
            letterButtons.Add(Char.Parse("b"), BBtn);
            letterButtons.Add(Char.Parse("c"), CBtn);
            letterButtons.Add(Char.Parse("d"), DBtn);
            letterButtons.Add(Char.Parse("e"), EBtn);
            letterButtons.Add(Char.Parse("f"), FBtn);
            letterButtons.Add(Char.Parse("g"), GBtn);
            letterButtons.Add(Char.Parse("h"), HBtn);
            letterButtons.Add(Char.Parse("i"), IBtn);
            letterButtons.Add(Char.Parse("j"), JBtn);
            letterButtons.Add(Char.Parse("k"), KBtn);
            letterButtons.Add(Char.Parse("l"), LBtn);
            letterButtons.Add(Char.Parse("m"), MBtn);
            letterButtons.Add(Char.Parse("n"), NBtn);
            letterButtons.Add(Char.Parse("o"), OBtn);
            letterButtons.Add(Char.Parse("p"), PBtn);
            letterButtons.Add(Char.Parse("q"), QBtn);
            letterButtons.Add(Char.Parse("r"), RBtn);
            letterButtons.Add(Char.Parse("s"), SBtn);
            letterButtons.Add(Char.Parse("t"), TBtn);
            letterButtons.Add(Char.Parse("u"), UBtn);
            letterButtons.Add(Char.Parse("v"), VBtn);
            letterButtons.Add(Char.Parse("w"), WBtn);
            letterButtons.Add(Char.Parse("x"), XBtn);
            letterButtons.Add(Char.Parse("y"), YBtn);
            letterButtons.Add(Char.Parse("z"), ZBtn);

            foreach (var item in letterButtons)
            {
                item.Value.Click += handle_Letter_Click;
                item.Value.MouseMove += handle_Letter_Mouse_Move;
                item.Value.MouseLeave += handle_Letter_Mouse_Leave;
            }
            Render_Word_List_By_Letter(selectedChar.ToString());
            letterButtons[selectedChar].BackColor = ColorTranslator.FromHtml("#8E83A6");
        }

        void Render_Word_List_By_Letter(string s)
        {
            //Loading loading = new Loading();
            //loading.Location = new Point((this.Width - loading.Width) / 2, (this.Height - loading.Height) / 2);
            //loading.Show();
            char c = s[0];
            if (letterButtons.FirstOrDefault(x => x.Key == c).Value == null)
            {
                return;
            }
            selectedChar = c;
            lb_SelectedLetter.Text = "#" + c.ToString().ToUpper();

            DataRow[] wordList = tableWord.Select("word LIKE '" + s + "%'");
            int count = 0;
            flow_WordsByLetter.Controls.Clear();
            foreach (DataRow row in wordList)
            {
                Label label = new Label();
                label.Text = row["word"].ToString();
                label.ForeColor = Color.White;
                label.BackColor = Color.FromArgb(64, 75, 113);
                label.Font = new Font("Microsoft Sans Serif", 32);
                label.Size = new Size(flow_WordsByLetter.Width - 30, 70);
                label.Cursor = Cursors.Hand;
                label.MouseMove += handle_Word_Mouse_Move;
                label.MouseLeave += handle_Word_Mouse_Leave;
                label.Click += handle_Word_Click;
                flow_WordsByLetter.Controls.Add(label);
                count++;
                if (count > 300)
                {
                    return;
                }
            }
            //loading.Close();
            //loading.Dispose();
        }

        private bool Check_Url_Valid(string url)
        {
            try
            {
                WebClient client = new WebClient();
                client.DownloadString(url);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SoundPlayer.URL = "https://ssl.gstatic.com/dictionary/static/sounds/oxford/" + WoDlb.Text + "--_gb_1.mp3";
        }

        void handle_Letter_Event(object sender, string type)
        {
            Button button = (Button)sender;
            char c = letterButtons.FirstOrDefault(x => x.Value == button).Key;
            switch (type)
            {
                case "mouse move":
                    if (c != selectedChar)
                    {
                        letterButtons[c].BackColor = ColorTranslator.FromHtml("#8E83A6");
                    }
                    break;

                case "mouse leave":
                    if (c != selectedChar)
                    {
                        letterButtons[c].BackColor = ColorTranslator.FromHtml("#303A63");
                    }
                    break;
                case "click":
                    if (c != selectedChar)
                    {
                        letterButtons[selectedChar].BackColor = ColorTranslator.FromHtml("#303A63");
                        letterButtons[c].BackColor = ColorTranslator.FromHtml("#8E83A6");

                        Render_Word_List_By_Letter(c.ToString());
                        selectedChar = c;
                    }
                    break;
                default:
                    break;
            }
        }

        void handle_Letter_Mouse_Move(object sender, MouseEventArgs e)
        {
            handle_Letter_Event(sender, "mouse move");
        }
        void handle_Letter_Mouse_Leave(object sender, EventArgs e)
        {
            handle_Letter_Event(sender, "mouse leave");
        }
        void handle_Letter_Click(object sender, EventArgs e)
        {
            handle_Letter_Event(sender, "click");
        }

        private async void SearchBox_TextChanged(object sender, EventArgs e)
        {
            HomePn.BringToFront();
            async Task<bool> UserKeepsTyping()
            {
                string txt = SearchBox.Text;
                await Task.Delay(300);
                return txt != SearchBox.Text;
            }
            if (await UserKeepsTyping()) return;
            if (searchText != SearchBox.Text && searchText != "Search some word...")
            {
                if (!string.IsNullOrWhiteSpace(SearchBox.Text))
                {
                    Render_Word_List_By_Letter(SearchBox.Text);
                }
                else
                {
                    Render_Word_List_By_Letter(selectedChar.ToString());
                }
            }
            searchText = SearchBox.Text;
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            if (searchText != SearchBox.Text && searchText != "Search some word...")
            {
                if (!string.IsNullOrWhiteSpace(SearchBox.Text))
                {
                    Render_Word_List_By_Letter(SearchBox.Text);
                }
                else
                {
                    Render_Word_List_By_Letter(selectedChar.ToString());
                }
            }
            searchText = SearchBox.Text;
        }

        private void SearchBox_Enter(object sender, EventArgs e)
        {
            if (SearchBox.Text == "Search some word...")
            {
                SearchBox.Text = "";
                SearchBox.ForeColor = Color.Black;
            }
            if (searchText != SearchBox.Text && searchText != "Search some word...")
            {
                if (!string.IsNullOrWhiteSpace(SearchBox.Text))
                {
                    Render_Word_List_By_Letter(SearchBox.Text);
                }
                else
                {
                    Render_Word_List_By_Letter(selectedChar.ToString());
                }
            }
            searchText = SearchBox.Text;
        }

        void handle_Word_Event(object sender, string type)
        {
            Label label = (Label)sender;
            switch (type)
            {
                case "mouse move":
                    label.Font = new Font("Microsoft Sans Serif", 34, FontStyle.Bold);
                    label.BackColor = Color.FromArgb(93, 105, 145);
                    break;

                case "mouse leave":
                    label.BackColor = Color.FromArgb(64, 75, 113);
                    label.Font = new Font("Microsoft Sans Serif", 32, FontStyle.Regular);
                    break;
                case "click":
                    //
                    //

                    break;
                default:
                    break;
            }
        }

        void handle_Word_Mouse_Move(object sender, MouseEventArgs e)
        {
            handle_Word_Event(sender, "mouse move");
        }
        void handle_Word_Mouse_Leave(object sender, EventArgs e)
        {
            handle_Word_Event(sender, "mouse leave");
        }
        void handle_Word_Click(object sender, EventArgs e)
        {
            handle_Word_Event(sender, "click");
        }
    }
}
