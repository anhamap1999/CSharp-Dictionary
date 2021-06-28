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
using System.Text.RegularExpressions;

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
            HomePn.BringToFront();
            //WordConnet_load();
        }

        DataTable tableWord = new DataTable();
        DataTable tableDefinition = new DataTable();

        Dictionary<char, Button> letterButtons = new Dictionary<char, Button>();
        Dictionary<char, DataRow[]> letterWordList = new Dictionary<char, DataRow[]>();
        char selectedChar = Char.Parse("a");

        string searchText = "";

        List<string> savedWords = new List<string>() { "nightfall", "vaccine", "outbreak","happy","abnormal" };
        DataTable tableConnectWord = new DataTable();

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
                        if (!string.IsNullOrWhiteSpace(word))
                        {
                            List<Dictionary<string, string>> cloneArray = new List<Dictionary<string, string>>(examples);
                            tableDefinition.Rows.Add(word, type, definition, synonym, explain, cloneArray);
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
                        //if (!string.IsNullOrWhiteSpace(word))
                        //{
                        //        if (!letterWordList.ContainsKey(word.ToLower()[0]))
                        //    {
                        //        List<DataRow> rows = new List<DataRow>();
                        //        rows.Add(tableWord.Rows[tableWord.Rows.Count - 1]);
                        //        List<DataRow> cloneArr = new List<DataRow>(rows);
                        //        letterWordList.Add(word.ToLower()[0], cloneArr.ToArray());
                        //    }
                        //    else
                        //    {
                        //        List<DataRow> rows = letterWordList[word.ToLower()[0]].ToList();
                        //        rows.Add(tableWord.Rows[tableWord.Rows.Count - 1]);
                        //        List<DataRow> cloneArr = new List<DataRow>(rows);
                        //        letterWordList[word.ToLower()[0]] = cloneArr.ToArray();
                        //    }
                        //}

                        type = null;
                    }
                    else if (str.StartsWith("*"))
                    {
                        if (type != null)
                        {
                            List<Dictionary<string, string>> cloneArray = new List<Dictionary<string, string>>(examples);
                            tableDefinition.Rows.Add(word, type, definition, synonym, explain, cloneArray);
                            examples.Clear();
                        }
                        type = str.Remove(0, 1).Trim();
                        definition = null;
                        synonym = null;
                        explain = null;
                    }
                    else if (str.StartsWith("-"))
                    {
                        if (definition != null)
                        {
                            List<Dictionary<string, string>> cloneArray = new List<Dictionary<string, string>>(examples);
                            tableDefinition.Rows.Add(word, type, definition, synonym, explain, cloneArray);
                            examples.Clear();
                        }
                        definition = str.Remove(0, 1).Trim();
                        synonym = null;
                        explain = null;
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
                        Dictionary<string, string> cloneExample = new Dictionary<string, string>(example);
                        examples.Add(cloneExample);
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
            //while (!Check_Url_Valid("https://ssl.gstatic.com/dictionary/static/sounds/oxford/" + word + "--_gb_1.mp3"))
            //{
            //    _rnd = rnd.Next(tableWord.Rows.Count);
            //    word = tableWord.Rows[_rnd][0].ToString();
            //}
            NWLb.Text = word;
            NWDPhoneticLb.Text = tableWord.Rows[_rnd][1].ToString();
            DataRow r = tableDefinition.Select("word = '" + word + "'").FirstOrDefault();
            //DataRow[] r = tableDefinition.Select("word LIKE 'alumna'");
            //foreach (DataRow row in r)
            //{
            //    Console.WriteLine(row[0]);

            //}
            Console.WriteLine(r[0].ToString());
            NWWordTypeLb.Text = "(" + r[1].ToString() + ")";
            string url = Search_Google(word);
            PicOfWordPb.ImageLocation = url;

            _rnd = rnd.Next(savedWords.Count);
            while (!Check_Url_Valid("https://ssl.gstatic.com/dictionary/static/sounds/oxford/" + savedWords[_rnd] + "--_gb_1.mp3"))
            {
                _rnd = rnd.Next(savedWords.Count);
            }
            DataRow wordRow = tableWord.Select("word = '" + savedWords[_rnd] + "'").FirstOrDefault();
            DataRow row = tableDefinition.Select("word = '" + savedWords[_rnd] + "'").FirstOrDefault();
            WoDTypeLb.Text = "(" + row["type"].ToString() + ")";
            WoDSpellingLb.Text = tableWord.Rows[_rnd]["spelling"].ToString();
            WoDlb.Text = savedWords[_rnd];
            WoDMeaningLb.Text = row["definition"].ToString();
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
            if (letterButtons.FirstOrDefault(x => x.Key == c).Value != null)
            {
                //flow_WordsByLetter.Controls.Clear();
                //return;
            }
            selectedChar = c;
            lb_SelectedLetter.Text = "#" + c.ToString().ToUpper();

            //DataTable table = letterWordList[c].CopyToDataTable();
            //List<DataRow> rows = new List<DataRow>();
            //if (s.Length == 1)
            //{
            //    rows = table.Select().ToList();
            //} else
            //{
            //    rows = table.Select("word LIKE '" + s + "%'").ToList();
            //}
            DataRow[] wordList = tableWord.Select("word LIKE '" + s + "%'");
            int count = 0;
            flow_WordsByLetter.Controls.Clear();
            foreach (DataRow row in wordList)
            {
                Label label = new Label();
                label.Text = row["word"].ToString();
                label.ForeColor = Color.White;
                label.BackColor = Color.FromArgb(64, 75, 113);
                label.Font = new Font("Roboto Light", 28);
                label.Size = new Size(flow_WordsByLetter.Width - 30, 70);
                label.Cursor = Cursors.Hand;
                label.MouseMove += handle_Word_Mouse_Move;
                label.MouseLeave += handle_Word_Mouse_Leave;
                label.Click += handle_Word_Click;
                label.TextAlign = ContentAlignment.MiddleLeft;
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
                        if (letterButtons.FirstOrDefault(x => x.Key == selectedChar).Value != null)
                        {
                            letterButtons[selectedChar].BackColor = ColorTranslator.FromHtml("#303A63");
                        }
                        letterButtons[c].BackColor = ColorTranslator.FromHtml("#8E83A6");

                        HomePn.BringToFront();
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
                if (!string.IsNullOrWhiteSpace(txt))
                {
                    await Task.Delay(500);
                } else
                {
                    await Task.Delay(0);
                }
                return txt != SearchBox.Text;
            }
            if (await UserKeepsTyping()) return;
            if (searchText != SearchBox.Text && SearchBox.Text != "Search some word...")
            {
                if (!string.IsNullOrWhiteSpace(SearchBox.Text))
                {
                    if (letterButtons.FirstOrDefault(x => x.Key == selectedChar).Value != null)
                    {
                        letterButtons[selectedChar].BackColor = ColorTranslator.FromHtml("#303A63");
                    }
                    if (letterButtons.FirstOrDefault(x => x.Key == SearchBox.Text.ToLower()[0]).Value != null)
                    {
                        letterButtons[SearchBox.Text.ToLower()[0]].BackColor = ColorTranslator.FromHtml("#8E83A6");
                    }

                    HomePn.BringToFront();
                    Render_Word_List_By_Letter(SearchBox.Text.ToLower());
                    //selectedChar = SearchBox.Text.ToLower()[0];
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
            if (SearchBox.Text != "Search some word...")
            {
                if (!string.IsNullOrWhiteSpace(SearchBox.Text))
                {
                    if (selectedChar != SearchBox.Text.ToLower()[0])
                    {
                        if (letterButtons.FirstOrDefault(x => x.Key == selectedChar).Value != null)
                        {
                            letterButtons[selectedChar].BackColor = ColorTranslator.FromHtml("#303A63");
                        }
                        if (letterButtons.FirstOrDefault(x => x.Key == SearchBox.Text.ToLower()[0]).Value != null)
                        {
                            letterButtons[SearchBox.Text.ToLower()[0]].BackColor = ColorTranslator.FromHtml("#8E83A6");
                        }

                        Render_Word_List_By_Letter(SearchBox.Text.ToLower());
                        //selectedChar = SearchBox.Text.ToLower()[0];
                    }

                    panel_WordDefinition.BringToFront();
                    Render_Word_Click(SearchBox.Text.ToLower());
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
            //if (SearchBox.Text == "Search some word...")
            //{
            //    searchText = "";
            //    SearchBox.Text = "";
            //    SearchBox.ForeColor = Color.Black;
            //}
            if (SearchBox.Text != "Search some word...")
            {
                if (!string.IsNullOrWhiteSpace(SearchBox.Text))
                {
                    if (selectedChar != SearchBox.Text.ToLower()[0])
                    {
                        if (letterButtons.FirstOrDefault(x => x.Key == selectedChar).Value != null)
                        {
                            letterButtons[selectedChar].BackColor = ColorTranslator.FromHtml("#303A63");
                        }
                        if (letterButtons.FirstOrDefault(x => x.Key == SearchBox.Text.ToLower()[0]).Value != null)
                        {
                            letterButtons[SearchBox.Text.ToLower()[0]].BackColor = ColorTranslator.FromHtml("#8E83A6");
                        }

                        Render_Word_List_By_Letter(SearchBox.Text.ToLower());
                        //selectedChar = SearchBox.Text.ToLower()[0];
                    }

                    panel_WordDefinition.BringToFront();
                    Render_Word_Click(SearchBox.Text.ToLower());
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
                    label.Font = new Font("Roboto", 30);
                    label.BackColor = Color.FromArgb(93, 105, 145);
                    break;

                case "mouse leave":
                    label.BackColor = Color.FromArgb(64, 75, 113);
                    label.Font = new Font("Roboto Light", 28);
                    break;
                case "click":
                    //
                    //
                    panel_WordDefinition.BringToFront();
                    Render_Word_Click(label.Text);

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

        void Render_Word_Click(string word)
        {
            label_ViewWord.Text = "";
            label_WordSpelling.Text = "";
            DataRow[] rows = (from row in tableWord.AsEnumerable()
                             where row["word"].ToString() == word
                             select row).ToArray();
            if (rows.Length == 0)
            {
                panel_WordNotFound.BringToFront();
                return;
            } else
            {
                panel_WordNotFound.SendToBack();
            }

            label_ViewWord.Text = word;
            label_WordSpelling.Text = rows[0]["spelling"].ToString();

            if (savedWords.FindIndex(x => x == word) >= 0)
            {
                pic_WordSave.BackgroundImage = global::Dictionary.Properties.Resources.saved;
            } else
            {
                pic_WordSave.BackgroundImage = global::Dictionary.Properties.Resources.unsaved;
            }

            pic_WordAudio.Location = new Point(label_WordSpelling.Location.X + label_WordSpelling.Width + 40, pic_WordAudio.Location.Y);
            var data = from row in tableDefinition.AsEnumerable()
                       where row["word"].ToString() == word
                       group row by new { type = row["type"].ToString() } into newRow
                       select newRow;

            flow_WordType.Height = 0;
            flow_WordType.Controls.Clear();
            foreach (var item in data.ToList())
            {
                string type = item.Key.type;
                WordType wordType = new WordType(type, item.ToList(), flow_WordType.Width);
                wordType.WordClick += Handle_Word_Click;
                flow_WordType.Controls.Add(wordType);
                flow_WordType.Height += wordType.Height + 5;
            }
        }

        private void pic_WordAudio_Click(object sender, EventArgs e)
        {
            SoundPlayer.URL = "https://ssl.gstatic.com/dictionary/static/sounds/oxford/" + label_ViewWord.Text + "--_gb_1.mp3";
        }

        private void pic_WordSave_Click(object sender, EventArgs e)
        {
            int index = savedWords.FindIndex(x => x == label_ViewWord.Text);
            if (index >= 0)
            {
                savedWords.RemoveAt(index);
                pic_WordSave.BackgroundImage = global::Dictionary.Properties.Resources.unsaved;
            }
            else
            {
                savedWords.Add(label_ViewWord.Text);
                pic_WordSave.BackgroundImage = global::Dictionary.Properties.Resources.saved;
            }
        }

        private void SearchBox_Click(object sender, EventArgs e)
        {
            if (SearchBox.Text == "Search some word...")
            {
                searchText = "";
                SearchBox.Text = "";
                SearchBox.ForeColor = Color.Black;
            }
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (SearchBox.Text != "Search some word...")
                {
                    if (!string.IsNullOrWhiteSpace(SearchBox.Text))
                    {
                        if (selectedChar != SearchBox.Text.ToLower()[0])
                        {
                            if (letterButtons.FirstOrDefault(x => x.Key == selectedChar).Value != null)
                            {
                                letterButtons[selectedChar].BackColor = ColorTranslator.FromHtml("#303A63");
                            }
                            if (letterButtons.FirstOrDefault(x => x.Key == SearchBox.Text.ToLower()[0]).Value != null)
                            {
                                letterButtons[SearchBox.Text.ToLower()[0]].BackColor = ColorTranslator.FromHtml("#8E83A6");
                            }

                            Render_Word_List_By_Letter(SearchBox.Text.ToLower());
                            //selectedChar = SearchBox.Text.ToLower()[0];
                        }

                        panel_WordDefinition.BringToFront();
                        Render_Word_Click(SearchBox.Text.ToLower());
                    }
                    else
                    {
                        Render_Word_List_By_Letter(selectedChar.ToString());
                    }
                }
                searchText = SearchBox.Text;
            }
        }

        void Handle_Word_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            string word = label.Text;
            word = Regex.Replace(word, @"(?<=[\.!\?])\s+", "");
            if (!string.IsNullOrWhiteSpace(word))
            {
                //if (selectedChar != word.ToLower()[0])
                //{
                //    if (letterButtons.FirstOrDefault(x => x.Key == selectedChar).Value != null)
                //    {
                //        letterButtons[selectedChar].BackColor = ColorTranslator.FromHtml("#303A63");
                //    }
                //    if (letterButtons.FirstOrDefault(x => x.Key == word.ToLower()[0]).Value != null)
                //    {
                //        letterButtons[word.ToLower()[0]].BackColor = ColorTranslator.FromHtml("#8E83A6");
                //    }

                //    //Render_Word_List_By_Letter(word.ToLower()[0].ToString());
                //    //selectedChar = SearchBox.Text.ToLower()[0];
                //}

                panel_WordDefinition.BringToFront();
                Render_Word_Click(word.ToLower());
            }
        }

        private void pic_WordSave_MouseMove(object sender, MouseEventArgs e)
        {
            int index = savedWords.FindIndex(x => x == label_ViewWord.Text);
            if (index < 0)
            {
                pic_WordSave.BackgroundImage = global::Dictionary.Properties.Resources.saved;
            }
        }

        private void pic_WordSave_MouseLeave(object sender, EventArgs e)
        {
            int index = savedWords.FindIndex(x => x == label_ViewWord.Text);
            if (index < 0)
            {
                pic_WordSave.BackgroundImage = global::Dictionary.Properties.Resources.unsaved;
            }
        }

        private void Logolb_Click(object sender, EventArgs e)
        {
            HomePn.BringToFront();
        }

        string Search_Google(string word)
        {
            WebRequest request = WebRequest.Create("https://www.google.com/search?tbm=isch&q=" + word);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Display the status.
            Console.WriteLine(response.StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            Console.WriteLine(responseFromServer);
            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            response.Close();

            int firstIndex = responseFromServer.IndexOf("<img");
            int nextIndex = responseFromServer.IndexOf("<img", firstIndex + 1);
            int startIndex = responseFromServer.IndexOf("src=\"", nextIndex + 1);
            DateTime time = DateTime.Now;
            if (startIndex >= 0)
            {
                int endIndex = responseFromServer.IndexOf("\"", startIndex + 5);

                string url = responseFromServer.Substring(startIndex + 5, endIndex - startIndex - 5);
                return url;
                //using (WebClient client = new WebClient())
                //{
                //    //client.DownloadFile(new Uri(url), @"D:\Dictionary\" + word + "-" + time.ToString() + ".png");
                //    //client.DownloadFile(new Uri(url), "D:\\Dictionary\\" + word + ".png");
                //    // OR 
                //    client.DownloadFileAsync(new Uri(url), "D:\\Dictionary\\" + word + ".png");

                //    //var image = Directory.GetFiles("D:\\Dictionary\\" + word + ".png");
                //}
            }
            return "";
        }

        private void WoDlb_Click(object sender, EventArgs e)
        {
            Handle_Word_Click(sender, e);
        }

        private void NWLb_Click(object sender, EventArgs e)
        {
            Handle_Word_Click(sender, e);
        }



        int level = 1;
        void WordConnet_load(int level)
        {
            //string[] vowel = { "A", "E", "I", "O", "U" };
            //string[] consonant = { "B", "C", "D", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T","Y", "V", "W", "X", "Z" };
            //Random rnd = new Random();
            //string[] letters = new string[3];   
            //for(int i = 0; i<3; i++)
            //{
            //    if (i == 0)
            //    {
            //        letters[i] = vowel[rnd.Next(0, 4)];
            //        continue;
            //    }
            //    else letters[i] = consonant[rnd.Next(0, 20)];
            //    if (letters[i] == letters[i-1])
            //    {
            //        letters[i] = consonant[rnd.Next(0, 20)];
            //    }
            //}
            //Console.WriteLine(letters[0]);
            //Console.WriteLine(letters[1]);
            //Console.WriteLine(letters[2]);
            ////foreach (var v in Permutations(letters))
            ////    Console.WriteLine(string.Join(",", v)); // Print values separated by comma
            ////var combinations = Combinations(0, 2, letters);

            ////foreach (var item in combinations)
            ////    Console.WriteLine(item);

            //string[] randomArr = new string[] { "O", "E", "B", "R" };
            //List<string[]> combinations = new List<string[]>();

            //var combinations2 = Combinations(0, 2, randomArr);
            //var combinations3 = Combinations(0, 3, randomArr);

            //foreach (string item in combinations2)
            //{
            //    combinations.Add(item.Split(' '));
            //}
            //foreach (string item in combinations3)
            //{
            //    combinations.Add(item.Split(' '));
            //}
            //combinations.Add(randomArr);
            //List<string> permutationsResult = new List<string>();
            //foreach (string[] i in combinations)
            //    foreach (var v in Permutations(i))
            //        permutationsResult.Add(string.Join("", v));
            //foreach(string w in permutationsResult)
            //{
            //    DataRow row = tableConnectWord.Select("word ='" + w + "'").FirstOrDefault();
            //    if(row != null)
            //        Console.WriteLine(row[0].ToString());
            //}
            tableConnectWord.Columns.Add("letters", typeof(string));
            tableConnectWord.Columns.Add("answer", typeof(string));
            string[] lines = System.IO.File.ReadAllLines("D:\\Dictionary\\ConnectWord.txt");
            foreach (string line in lines)
            {
                string[] str = line.Split(':');
                tableConnectWord.Rows.Add(str[0], str[1]);
            }
            foreach (DataRow dataRow in tableConnectWord.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    Console.WriteLine(item);
                }
            }

        }


        //private static IEnumerable<string> Combinations(int start, int level, string[] array)
        //{
        //    for (int i = start; i < array.Length; i++)
        //        if (level == 1)
        //            yield return array[i];
        //        else
        //            foreach (string combination in Combinations(i + 1, level - 1, array))
        //                yield return String.Format("{0} {1}", array[i], combination);
        //}

        //public static IEnumerable<T[]> Permutations<T>(T[] values, int fromInd = 0)
        //{
        //    if (fromInd + 1 == values.Length)
        //        yield return values;
        //    else
        //    {
        //        foreach (var v in Permutations(values, fromInd + 1))
        //            yield return v;

        //        for (var i = fromInd + 1; i < values.Length; i++)
        //        {
        //            SwapValues(values, fromInd, i);
        //            foreach (var v in Permutations(values, fromInd + 1))
        //                yield return v;
        //            SwapValues(values, fromInd, i);
        //        }
        //    }
        //}

        //private static void SwapValues<T>(T[] values, int pos1, int pos2)
        //{
        //    if (pos1 != pos2)
        //    {
        //        T tmp = values[pos1];
        //        values[pos1] = values[pos2];
        //        values[pos2] = tmp;
        //    }
        //}

        void PnSaveWord_Load()
        {
            SavedWordPn.BringToFront();
            SavedWordPn.Controls.Clear();
            Label Title = new Label();
            Title.Text = "Danh sách từ mới";
            Title.ForeColor = Color.FromArgb(15, 23, 59);
            Title.Location = new Point(102, 50);
            Title.Font = new Font("Roboto", 28);
            Title.AutoSize = true;
            SavedWordPn.Controls.Add(Title);
            Panel Seperator = new Panel();
            Seperator.AutoSize = false;
            Seperator.Height = 2;
            Seperator.Width = 400;
            Seperator.BackColor = Color.FromArgb(15, 23, 59);
            Seperator.Location = new Point(113, 100);
            SavedWordPn.Controls.Add(Seperator);
            if(savedWords.Count() == 0)
            {
                Label Message = new Label();
                Message.Location = new Point(110,114);
                Message.Text = "Bạn chưa lưu từ nào cả!";
                Message.Font = new Font("Roboto Light", 17, FontStyle.Italic);
                Message.AutoSize = true;
                Message.ForeColor = Color.FromArgb(15, 23, 59);
                SavedWordPn.Controls.Add(Message);
            }    
            else
            {
                for (int j = 0; j < savedWords.Count; j++)
                {
                    DataRow r = (from row in tableDefinition.AsEnumerable()
                                 where row["word"].ToString() == savedWords[j]
                                 select row).FirstOrDefault();
                    SaveWordItem SWitem = new SaveWordItem(r, savedWords);
                    SWitem.Location = new Point(112, 114 + 110 * j);
                    SWitem.WordClick += handle_Word_Click;
                    SavedWordPn.Controls.Add(SWitem);
                }
            }
        }

        private void SavedWordBtn_Click(object sender, EventArgs e)
        {
            PnSaveWord_Load();
        }
    }
}
