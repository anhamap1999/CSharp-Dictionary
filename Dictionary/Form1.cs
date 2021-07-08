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
using AxWMPLib;
using System.Drawing;
using System.Media;


namespace Dictionary
{
    public partial class Dictionary : Form
    {
        public Dictionary()
        {
            InitializeComponent();
            //PicOfWordPb.ImageLocation = "https://cdn.britannica.com/22/206222-131-E921E1FB/Domestic-feline-tabby-cat.jpg";
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
            Initialize_Game_Screen();
            PnToeicWords_Load();
            //Async_Load();
        }

        void Async_Load()
        {
            Task loadDictionary = new Task(Load_Dictionary);
            loadDictionary.Start();
            Task generateLetterButtons = new Task(Generate_Letter_Buttons);
            generateLetterButtons.Start();
            Task initializeGame = new Task(Initialize_Game_Screen);
            initializeGame.Start();
            Task loadToeic = new Task(PnToeicWords_Load);
            loadToeic.Start();
        }

        DataTable tableWord = new DataTable();
        DataTable tableDefinition = new DataTable();

        Dictionary<char, Button> letterButtons = new Dictionary<char, Button>();
        Dictionary<char, DataRow[]> letterWordList = new Dictionary<char, DataRow[]>();
        char selectedChar = Char.Parse("a");

        string searchText = "";

        List<string> savedWords = new List<string>() { "nightfall", "vaccine", "outbreak" };
        DataTable tableConnectWord = new DataTable();

        DataTable tableLevel = new DataTable();
        int currentLevel = -1;
        List<string> crossedWords = new List<string>();
        bool crossing = false;
        Panel startLetter = new Panel();

        List<string> ToeicWordsList = new List<string>();

        bool gameMusic = true;

        string srcLang = "en";
        string desLang = "vi";
        string srcAudioUrl = "";
        string desAudioUrl = "";

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
            string contents = File.ReadAllText(@"D:\Dictionary\toeic_words.txt");
            ToeicWordsList = contents.Split(' ').OrderBy(q => q).ToList();
        }

        void Load_Dictionary()
        {
            Random rnd = new Random();
            int _rnd = rnd.Next(tableWord.Rows.Count);
            string word = tableWord.Rows[_rnd][0].ToString();
            DataRow r = tableDefinition.Select("word = '" + word + "'").FirstOrDefault();

            while (r[1].ToString() == "" || (tableWord.Rows[_rnd]["spelling"].ToString() == ""))
            {
                _rnd = rnd.Next(tableWord.Rows.Count);
                word = tableWord.Rows[_rnd][0].ToString();
                r = tableDefinition.Select("word = '" + word + "'").FirstOrDefault();
            }
            //while (!Check_Url_Valid("https://ssl.gstatic.com/dictionary/static/sounds/oxford/" + word + "--_gb_1.mp3"))
            //{
            //    _rnd = rnd.Next(tableWord.Rows.Count);
            //    word = tableWord.Rows[_rnd][0].ToString();
            //}
            NWLb.Text = word;
            NWDPhoneticLb.Text = tableWord.Rows[_rnd][1].ToString();
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
            DataRow wordRow = tableWord.Select("word = '" + savedWords[_rnd] + "'").FirstOrDefault();
            DataRow row = tableDefinition.Select("word = '" + savedWords[_rnd] + "'").FirstOrDefault();
            while (!Check_Url_Valid("https://ssl.gstatic.com/dictionary/static/sounds/oxford/" + savedWords[_rnd] + "--_gb_1.mp3") || row[1].ToString() == "" || (wordRow["spelling"].ToString() == ""))
            {
                _rnd = rnd.Next(savedWords.Count);
                wordRow = tableWord.Select("word = '" + savedWords[_rnd] + "'").FirstOrDefault();
                row = tableDefinition.Select("word = '" + savedWords[_rnd] + "'").FirstOrDefault();
            }
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

            this.KeyPreview = false;
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
            //if (SearchBox.Text != "Search some word...")
            //{
            //    if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            //    {
            //        if (selectedChar != SearchBox.Text.ToLower()[0])
            //        {
            //            if (letterButtons.FirstOrDefault(x => x.Key == selectedChar).Value != null)
            //            {
            //                letterButtons[selectedChar].BackColor = ColorTranslator.FromHtml("#303A63");
            //            }
            //            if (letterButtons.FirstOrDefault(x => x.Key == SearchBox.Text.ToLower()[0]).Value != null)
            //            {
            //                letterButtons[SearchBox.Text.ToLower()[0]].BackColor = ColorTranslator.FromHtml("#8E83A6");
            //            }

            //            Render_Word_List_By_Letter(SearchBox.Text.ToLower());
            //            //selectedChar = SearchBox.Text.ToLower()[0];
            //        }

            //        panel_WordDefinition.BringToFront();
            //        Render_Word_Click(SearchBox.Text.ToLower());
            //    }
            //    else
            //    {
            //        Render_Word_List_By_Letter(selectedChar.ToString());
            //    }
            //}
            //searchText = SearchBox.Text;
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

        private void GameBtn_Click(object sender, EventArgs e)
        {
            PnConnectWord.BringToFront();
            this.KeyPreview = true;

            //Timer levelTimer = new Timer();
            //levelTimer.Interval = 10;
            //levelTimer.Tick += levelTimer_Tick;
            //levelTimer.Start();
        }

        void Game_Update_Score()
        {
            if (currentLevel >= 0 && crossedWords.Count > 0)
            {
                int score = 0;
                foreach (string word in crossedWords)
                {
                    score += word.Length;
                }
                float rate = (float)score / (int)tableLevel.Rows[currentLevel - 1]["maxScore"];
                if (rate == 1)
                {
                    score = 3;
                } else if (rate >= 0.6666)
                {
                    score = 2;
                } else if (rate >= 0.3333)
                {
                    score = 1;
                } else
                {
                    score = 0;
                }

                tableLevel.Rows[currentLevel - 1]["score"] = score;
            }


            currentLevel = -1;
            crossedWords.Clear();

            foreach (object item in panel_Level.Controls)
            {
                if (item is Panel)
                {
                    Panel panel = (Panel)item;
                    if (panel.Name.Contains("star_Level"))
                    {
                        int level = Convert.ToInt32(panel.Name.Replace("star_Level", ""));

                        switch ((int)tableLevel.Rows[level - 1]["score"])
                        {
                            case 0:
                                panel.BackgroundImage = global::Dictionary.Properties.Resources.star_0;
                                break;
                            case 1:
                                panel.BackgroundImage = global::Dictionary.Properties.Resources.star_1;
                                break;
                            case 2:
                                panel.BackgroundImage = global::Dictionary.Properties.Resources.star_2;
                                break;
                            case 3:
                                panel.BackgroundImage = global::Dictionary.Properties.Resources.star_3;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        void Initialize_Game_Screen()
        {
            tableLevel.Columns.Add("level", typeof(string));
            tableLevel.Columns.Add("letters", typeof(string));
            tableLevel.Columns.Add("words", typeof(string[]));
            tableLevel.Columns.Add("score", typeof(int));
            tableLevel.Columns.Add("maxScore", typeof(int));
            StreamReader sr = new StreamReader("D:\\Dictionary\\ConnectWord.txt");
            string str;
            int level = 1;
            while ((str = sr.ReadLine()) != null)
            {
                string letters = str.Substring(0, str.IndexOf(":"));
                string[] words = str.Substring(str.IndexOf(":") + 1).Split(',');
                int maxScore = 0;
                foreach (string word in words)
                {
                    maxScore += word.Length;
                }
                tableLevel.Rows.Add(level, letters, words, 0, maxScore);
                level++;
            }
            sr.Close();

            foreach (object item in panel_Level.Controls)
            {
                if (item is Panel)
                {
                    Panel panel = (Panel)item;
                    if (panel.Name.Contains("btn_Level"))
                    {
                        panel.MouseMove += handle_Level_Mouse_Move;
                        panel.MouseLeave += handle_Level_Mouse_Leave;
                        panel.Click += handle_Level_Click;
                        Label label = (Label)panel.Controls[0];
                        if (label.Name.Contains("label_Level"))
                        {
                            label.MouseMove += handle_Level_Mouse_Move;
                            label.MouseLeave += handle_Level_Mouse_Leave;
                            label.Click += handle_Level_Click;
                        }
                    }
                }
            }

            foreach (object item in PnLetterHolder.Controls)
            {
                if (item is Panel)
                {
                    Panel panel = (Panel)item;
                    Label label = (Label)panel.Controls[0];
                    label.MouseMove += handle_Letter_Cross_Mouse_Move;
                    label.MouseLeave += handle_Letter_Cross_Mouse_Leave;
                    //label.MouseDown += handle_Letter_Cross_Mouse_Down;
                    //label.MouseUp += handle_Letter_Cross_Mouse_Up;
                }                
            }

            BtnExit.MouseMove += delegate
            {
                if (BtnExit.Width == 55)
                {
                    BtnExit.Width = 63;
                    BtnExit.Height = 63;
                    BtnExit.Location = new Point(BtnExit.Location.X - 4, BtnExit.Location.Y - 4);
                }
            };
            BtnExit.MouseLeave += delegate
            {
                if (BtnExit.Width == 63)
                {
                    BtnExit.Width = 55;
                    BtnExit.Height = 55;
                    BtnExit.Location = new Point(BtnExit.Location.X + 4, BtnExit.Location.Y + 4);
                }
            };
            BtnHint.MouseMove += delegate
            {
                if (BtnHint.Width == 55)
                {
                    BtnHint.Width = 63;
                    BtnHint.Height = 63;
                    BtnHint.Location = new Point(BtnHint.Location.X - 4, BtnHint.Location.Y - 4);
                }
            };
            BtnHint.MouseLeave += delegate
            {
                if (BtnHint.Width == 63)
                {
                    BtnHint.Width = 55;
                    BtnHint.Height = 55;
                    BtnHint.Location = new Point(BtnHint.Location.X + 4, BtnHint.Location.Y + 4);
                }
            };
            BtnMusic.MouseMove += delegate
            {
                if (BtnMusic.Width == 55)
                {
                    BtnMusic.Width = 63;
                    BtnMusic.Height = 63;
                    BtnMusic.Location = new Point(BtnMusic.Location.X - 4, BtnMusic.Location.Y - 4);
                }
            };
            BtnMusic.MouseLeave += delegate
            {
                if (BtnMusic.Width == 63)
                {
                    BtnMusic.Width = 55;
                    BtnMusic.Height = 55;
                    BtnMusic.Location = new Point(BtnMusic.Location.X + 4, BtnMusic.Location.Y + 4);
                }
            };

            PnLetterHolder.Visible = false;
            BtnExit.Visible = false;
            BtnHint.Visible = false;
            panel_WordResult.Visible = false;
            panel_LevelTitle.Visible = false;
            panel_CrossingWord.Visible = false;
            label_GameScore.Visible = false;
            panel_LevelSuccess.Visible = false;
            panel_ScoreCoin.Visible = false;

            panel_Level.Visible = true;
        }

        void handle_Level_Event(object sender, string type)
        {
            Label label = null;
            Panel panel = null;
            int level = 0;
            if (sender is Label)
            {
                label = (Label)sender;
                level = Convert.ToInt32(label.Text);
                panel = (Panel)panel_Level.Controls.Find("btn_Level" + level, true)[0];
            } else
            {
                panel = (Panel)sender;
                level = Convert.ToInt32(panel.Name.Replace("btn_Level", ""));
                label = (Label)panel_Level.Controls.Find("label_Level" + level, true)[0];
            }
            switch (type)
            {
                case "mouse move":
                    {
                        if (panel.Width == 70)
                        {
                            panel.Width = 78;
                            panel.Height = 78;
                            label.Width = 78;
                            label.Height = 78;
                            panel.Location = new Point(panel.Location.X - 4, panel.Location.Y - 4);
                        }
                        //label.Font = new Font("Roboto", 30);
                        //label.BackColor = Color.FromArgb(93, 105, 145);

                        break;

                    }

                case "mouse leave":
                    if (panel.Width == 78)
                    {
                        panel.Width = 70;
                        panel.Height = 70;
                        label.Width = 70;
                        label.Height = 70;
                        panel.Location = new Point(panel.Location.X + 4, panel.Location.Y + 4);
                    }
                    //label.BackColor = Color.FromArgb(64, 75, 113);
                    //label.Font = new Font("Roboto Light", 28);
                    break;
                case "click":
                    {
                        if (gameMusic)
                        {
                            Stream str = global::Dictionary.Properties.Resources.sound_button_click;
                            SoundPlayer snd = new SoundPlayer(str);
                            snd.Play();
                        }
                        Render_Game_Level(level);

                        break;
                    }
                default:
                    break;
            }
        }

        void Render_Game_Level(int level)
        {
            panel_Level.Visible = false;

            PnLetterHolder.Visible = true;
            PnLetterHolder.Visible = true;
            BtnExit.Visible = true;
            BtnHint.Visible = true;
            panel_WordResult.Visible = true;
            panel_LevelTitle.Visible = true;
            panel_CrossingWord.Visible = true;
            label_GameScore.Visible = true;
            panel_ScoreCoin.Visible = true;

            Game_Update_Score();

            List<Panel> removePanel = new List<Panel>();
            foreach (object item in panel_WordResult.Controls)
            {
                if (item is Panel)
                {
                    Panel panel = (Panel)item;
                    if (panel.Name.Contains("btn_RightLetter"))
                    {
                        removePanel.Add(panel);
                    }
                }
            }
            foreach (Panel item in removePanel)
            {
                panel_WordResult.Controls.Remove(item);
            }

            star_1.BackgroundImage = global::Dictionary.Properties.Resources.sao_nhat_2;
            star_2.BackgroundImage = global::Dictionary.Properties.Resources.sao_nhat_2;
            star_3.BackgroundImage = global::Dictionary.Properties.Resources.sao_nhat_2;

            DataRow row = tableLevel.Rows[level - 1];
            string letters = row["letters"].ToString();
            string[] words = (string[])row["words"];
            if (letters.Length == 3)
            {
                panel_Letter4.Visible = false;
                panel_Letter3.Location = new Point((panel_Letter1.Location.X + panel_Letter2.Location.X) / 2, panel_Letter3.Location.Y);
            }
            else
            {
                panel_Letter4.Visible = true;
                panel_Letter3.Location = new Point(panel_Letter1.Location.X, panel_Letter3.Location.Y);
            }

            label_LevelTitle.Text = "LEVEL " + level.ToString();
            label_GameScore.Text = "0";
            label_CrossingWord.Text = "";

            currentLevel = level;
            crossedWords.Clear();

            label_Letter1.Text = letters.ToUpper()[0].ToString();
            label_Letter2.Text = letters.ToUpper()[1].ToString();
            label_Letter3.Text = letters.ToUpper()[2].ToString();
            if (letters.Length == 4)
            {
                label_Letter4.Text = letters.ToUpper()[3].ToString();
            }

            foreach (object item in panel_WordResult.Controls)
            {
                if (item is Panel)
                {
                    Panel letterPanel = (Panel)item;
                    if (letterPanel.Name.Contains("btn_Letter"))
                    {
                        string indexString = letterPanel.Name.Replace("btn_Letter", "");
                        int rIndex = Convert.ToInt32(indexString[0].ToString());
                        int cIndex = Convert.ToInt32(indexString[2].ToString());
                        if (rIndex <= words.Length && cIndex <= words[rIndex - 1].Length)
                        {
                            letterPanel.Visible = true;
                        }
                        else
                        {
                            letterPanel.Visible = false;
                        }
                    }
                }
            }
            panel_WordResult.Height = 17 + 75 * words.Length;
            panel_WordResult.Location = new Point(panel_WordResult.Location.X, (PnConnectWord.Height - panel_WordResult.Height) / 2);
        }

        void handle_Level_Mouse_Move(object sender, MouseEventArgs e)
        {
            handle_Level_Event(sender, "mouse move");
        }
        void handle_Level_Mouse_Leave(object sender, EventArgs e)
        {
            handle_Level_Event(sender, "mouse leave");
        }
        void handle_Level_Click(object sender, EventArgs e)
        {
            handle_Level_Event(sender, "click");
        }

        private void levelTimer_Tick(object sender, EventArgs e)
        {
            //panel_Level.Location = new Point(panel_Level.Location.X, panel_Level.Location.Y - 2);
            //if (panel_Level.Location.Y <= 100)
            //{
            //    Timer timer = (Timer)sender;
            //    timer.Stop();
            //}
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
            Title.Text = "Danh sách từ đã lưu";
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
            if (savedWords.Count() == 0)
            {
                Label Message = new Label();
                Message.Location = new Point(110, 114);
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
            this.KeyPreview = false;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (gameMusic)
            {
                Stream str = global::Dictionary.Properties.Resources.sound_button_click;
                SoundPlayer snd = new SoundPlayer(str);
                snd.Play();
            }
            Render_Game_Home();
        }

        void Render_Game_Home()
        {
            //PnConnectWord.BringToFront();
            PnLetterHolder.Visible = false;
            BtnExit.Visible = false;
            BtnHint.Visible = false;
            panel_WordResult.Visible = false;
            panel_LevelTitle.Visible = false;
            panel_CrossingWord.Visible = false;
            label_GameScore.Visible = false;
            panel_LevelSuccess.Visible = false;
            panel_ScoreCoin.Visible = false;

            panel_Level.Visible = true;

            Game_Update_Score();

            List<Panel> removePanel = new List<Panel>();
            foreach (object item in panel_WordResult.Controls)
            {
                if (item is Panel)
                {
                    Panel panel = (Panel)item;
                    if (panel.Name.Contains("btn_RightLetter"))
                    {
                        removePanel.Add(panel);
                    }
                }
            }
            foreach (Panel item in removePanel)
            {
                panel_WordResult.Controls.Remove(item);
            }

            star_1.BackgroundImage = global::Dictionary.Properties.Resources.sao_nhat_2;
            star_2.BackgroundImage = global::Dictionary.Properties.Resources.sao_nhat_2;
            star_3.BackgroundImage = global::Dictionary.Properties.Resources.sao_nhat_2;
        }

        int getXByLineAndAngle(int x, int angle, int dist)
        {
            return Convert.ToInt32(x + dist * Math.Cos((Math.PI / 180) * angle));
        }
        int getYByLineAndAngle(int x, int angle, int dist)
        {
            return Convert.ToInt32(x + dist * Math.Sin((Math.PI / 180) * angle));
        }

        void handle_Letter_Cross_Event(object sender, string type, MouseEventArgs e = null)
        {
            Label label = (Label)sender;
            string indexString = label.Name.Replace("label_Letter", "");
            Panel panel = null;
            foreach (object item in PnLetterHolder.Controls)
            {
                if (item is Panel)
                {
                    Panel p = (Panel)item;
                    if (p.Name == ("panel_Letter" + indexString))
                    {
                        panel = p;
                        break;
                    }
                }
            }
            switch (type)
            {
                case "mouse move":
                case "mouse enter":
                    {
                        //if (panel.Width == 90)
                        //{
                        //    panel.Width = 98;
                        //    panel.Height = 98;
                        //    label.Width = 98;
                        //    label.Height = 88;
                        //    panel.Location = new Point(panel.Location.X - 4, panel.Location.Y - 4);
                        //}
                        if (crossing)
                        {
                            if (label_CrossingWord.Text.Contains(label.Text))
                            {
                                //if (label_CrossingWord.Text.EndsWith(label.Text))
                                //{
                                //    label_CrossingWord.Text = label_CrossingWord.Text.Substring(0, label_CrossingWord.Text.Length - 1);
                                //    if (panel.Name == "panel_Letter4" || panel_Letter4.Name == "panel_Letter3")
                                //    {
                                //        panel.BackgroundImage = global::Dictionary.Properties.Resources.tron_cam_dam;
                                //    } else
                                //    {
                                //        panel.BackgroundImage = global::Dictionary.Properties.Resources.tron_cam_nhat;
                                //    }
                                //}
                            }
                            else
                            {
                                Graphics g = PnLetterHolder.CreateGraphics();
                                if (gameMusic)
                                {
                                    Stream str = global::Dictionary.Properties.Resources.sound_cross;
                                    SoundPlayer snd = new SoundPlayer(str);
                                    snd.Play();
                                }
                                //TextureBrush texttureBrush = new TextureBrush(global::Dictionary.Properties.Resources.texture_cross_2, System.Drawing.Drawing2D.WrapMode.TileFlipY);
                                Pen pen = new Pen(Color.FromArgb(0, 115, 100), 10);
                                if (startLetter != null && startLetter != panel)
                                {
                                    int aX = startLetter.Location.X + 45;
                                    int aY = startLetter.Location.Y + 40;
                                    int bX = panel.Location.X + 45;
                                    int bY = panel.Location.Y + 40;
                                    g.DrawLine(pen, aX, aY, bX, bY);
                                    //if (aX == bX) {
                                    //    g.FillPolygon(texttureBrush, new Point[] { new Point(aX - 5, aY), new Point(aX + 5, aY), new Point(bX + 5, bY), new Point(bX - 5, bY), new Point(aX - 5, aY) });
                                    //} else if (aY == bY)
                                    //{
                                    //    g.FillPolygon(texttureBrush, new Point[] { new Point(aX, aY - 5), new Point(aX, aY + 5), new Point(bX, bY + 5), new Point(bX, bY - 5), new Point(aX, aY - 5) });
                                    //} else if (aX < bX) 
                                    //{ 
                                    //    g.FillPolygon(texttureBrush, new Point[] { new Point(getXByLineAndAngle(aX, 270, 10), getYByLineAndAngle(aY, 270, 10)), new Point(getXByLineAndAngle(aX, 90, 10), getYByLineAndAngle(aY, 90, 10)), new Point(getXByLineAndAngle(bX, 90, 10), getYByLineAndAngle(bY, 90, 10)), new Point(getXByLineAndAngle(bX, 270, 10), getYByLineAndAngle(bY, 270, 10)), new Point(getXByLineAndAngle(aX, 270, 10), getYByLineAndAngle(aY, 270, 10)) });
                                    //} else if (aX > bX)
                                    //{
                                    //    g.FillPolygon(texttureBrush, new Point[] { new Point(getXByLineAndAngle(aX, 90, 10), getYByLineAndAngle(aY, 90, 10)), new Point(getXByLineAndAngle(aX, 270, 10), getYByLineAndAngle(aY, 270, 10)), new Point(getXByLineAndAngle(bX, 270, 10), getYByLineAndAngle(bY, 270, 10)), new Point(getXByLineAndAngle(bX, 90, 10), getYByLineAndAngle(bY, 90, 10)), new Point(getXByLineAndAngle(aX, 90, 10), getYByLineAndAngle(aY, 90, 10)) });
                                    //}
                                }
                                //g.FillPolygon(brush, new Point[] { new Point(0, 10), new Point(10, 0), new Point(100,100), new Point(90, 110), new Point(0, 10) });
                                //System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath()
                                //g.FillPath()
                                //g.DrawImage()
                                //g.DrawImage(global::Dictionary.Properties.Resources.cross_line, new Point[] { new Point(0, 1), new Point(1, 0), new Point(50, 50), new Point(49, 51) });

                                startLetter = panel;
                                label_CrossingWord.Text += label.Text;
                                panel.BackgroundImage = global::Dictionary.Properties.Resources.tron_xanh_dam;
                            }
                        }
                        break;

                    }

                case "mouse up":
                    {
                        crossing = false;
                        DataRow row = tableLevel.Rows[currentLevel - 1];
                        string[] words = (string[])row["words"];
                        int foundIndex = words.ToList().FindIndex(x => x == label_CrossingWord.Text);
                        if (foundIndex >= 0)
                        {
                            crossedWords.Add(label_CrossingWord.Text);
                            int score = Convert.ToInt32(label_GameScore.Text);
                            int range = score + label_CrossingWord.Text.Length * 10;
                            Timer timer = new Timer();
                            timer.Interval = 1;
                            timer.Tick += delegate
                            {
                                if (score < range)
                                {
                                    score += 2;
                                } else
                                {
                                    timer.Stop();
                                }
                            };
                            timer.Start();
                        }
                        panel_Letter3.BackgroundImage = global::Dictionary.Properties.Resources.tron_cam_dam;
                        panel_Letter4.BackgroundImage = global::Dictionary.Properties.Resources.tron_cam_dam;
                        panel_Letter1.BackgroundImage = global::Dictionary.Properties.Resources.tron_cam_nhat;
                        panel_Letter2.BackgroundImage = global::Dictionary.Properties.Resources.tron_cam_nhat;



                        label_CrossingWord.Text = "";
                        break;
                    }
                case "mouse down":
                    {
                        crossing = true;
                        startLetter = panel;
                        label_CrossingWord.Text += label.Text;
                        panel.BackgroundImage = global::Dictionary.Properties.Resources.tron_xanh_dam;
                        break;
                    }
                case "mouse leave":
                    {
                        //if (panel.Width == 98)
                        //{
                        //    panel.Width = 90;
                        //    panel.Height = 90;
                        //    label.Width = 90;
                        //    label.Height = 80;
                        //    panel.Location = new Point(panel.Location.X + 4, panel.Location.Y + 4);
                        //}
                        break;
                    }
                default:
                    break;
            }
        }

        void handle_Letter_Cross_Mouse_Move(object sender, MouseEventArgs e)
        {
            handle_Letter_Cross_Event(sender, "mouse move", e);
        }
        void handle_Letter_Cross_Mouse_Leave(object sender, EventArgs e)
        {
            handle_Letter_Cross_Event(sender, "mouse leave");
        }
        void handle_Letter_Cross_Mouse_Down(object sender, EventArgs e)
        {
            handle_Letter_Cross_Event(sender, "mouse down");
        }
        void handle_Letter_Cross_Mouse_Up(object sender, EventArgs e)
        {
            handle_Letter_Cross_Event(sender, "mouse up");
        }

        private void Dictionary_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && currentLevel > 0)
            {
                crossing = true;
            }
        }

        private void Dictionary_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && currentLevel > 0)
            {
                crossing = false;
                startLetter = null;
                PnLetterHolder.Invalidate();
                DataRow row = tableLevel.Rows[currentLevel - 1];
                string[] words = (string[])row["words"];
                int foundIndex = words.ToList().FindIndex(x => x == label_CrossingWord.Text);
                int found = crossedWords.FindIndex(x => x == label_CrossingWord.Text);
                if (foundIndex >= 0)
                {
                    if (found < 0)
                    {
                        crossedWords.Add(label_CrossingWord.Text);
                        int score = Convert.ToInt32(label_GameScore.Text);
                        int range = score + label_CrossingWord.Text.Length * 10;
                        Timer timer = new Timer();
                        timer.Interval = 100;
                        timer.Start();
                        timer.Tick += delegate
                        {
                            if (score < range)
                            {
                                score += 2;
                                label_GameScore.Text = score.ToString();
                            }
                            else
                            {
                                timer.Stop();
                            }
                        };

                        List<Panel> newPanels = new List<Panel>();
                        foreach (object item in panel_WordResult.Controls)
                        {
                            if (item is Panel)
                            {
                                Panel letterPanel = (Panel)item;
                                if (letterPanel.Name.Contains("btn_Letter"))
                                {
                                    string indexString = letterPanel.Name.Replace("btn_Letter", "");
                                    int rIndex = Convert.ToInt32(indexString[0].ToString());
                                    int cIndex = Convert.ToInt32(indexString[2].ToString());
                                    if (rIndex == foundIndex + 1 && cIndex <= label_CrossingWord.Text.Length)
                                    {
                                        Panel panel = new Panel();
                                        panel.BackColor = Color.Transparent;
                                        panel.BackgroundImageLayout = ImageLayout.Zoom;
                                        panel.Size = new Size(70, 70);
                                        panel.Name = "btn_RightLetter" + rIndex + "x" + cIndex;
                                        if (rIndex % 2 == 0)
                                        {
                                            panel.BackgroundImage = global::Dictionary.Properties.Resources.vuong_xanh_dam;
                                        }
                                        else
                                        {
                                            panel.BackgroundImage = global::Dictionary.Properties.Resources.vuong_xanh_nhat;
                                        }
                                        panel.Location = new Point(letterPanel.Location.X + 15, letterPanel.Location.Y + 15);

                                        Label label = new Label();
                                        label.BackColor = Color.Transparent;
                                        label.AutoSize = false;
                                        label.Size = new Size(70, 60);
                                        label.Location = new Point(0, 0);
                                        label.ForeColor = Color.White;
                                        label.Font = new Font("Somatic Rounded", 32, FontStyle.Bold);
                                        label.Text = label_CrossingWord.Text[cIndex - 1].ToString();
                                        label.TextAlign = ContentAlignment.MiddleCenter;
                                        panel.Controls.Add(label);
                                        label.BringToFront();
                                        newPanels.Add(panel);
                                    }

                                }
                            }
                        }
                        foreach (Panel item in newPanels)
                        {
                            panel_WordResult.Controls.Add(item);
                            item.BringToFront();
                        }
                        newPanels.Sort((x, y) => x.Name.CompareTo(y.Name));
                        int i = 0;
                        Timer flyTimer = new Timer();
                        flyTimer.Interval = 50;
                        int delay = 0;
                        int loopCount = 0;
                        if (gameMusic)
                        {
                            Stream str = global::Dictionary.Properties.Resources.sound_score_plus;
                            SoundPlayer snd = new SoundPlayer(str);
                            snd.Play();
                        }
                        Stream strFly = global::Dictionary.Properties.Resources.sound_word_fly;
                        SoundPlayer sndFly = new SoundPlayer(strFly);
                        flyTimer.Tick += delegate
                        {
                            if (delay >= 10)
                            {
                                if (i<newPanels.Count)
                                {
                                    if (loopCount < 5)
                                    {
                                        newPanels[i].Location = new Point(newPanels[i].Location.X - 3, newPanels[i].Location.Y - 3);
                                        loopCount++;
                                    } else
                                    {
                                        //sndFly.Play();
                                        loopCount = 0;
                                        i++;
                                    }
                                    //newPanels[i].Location = new Point(newPanels[i].Location.X, newPanels[i].Location.Y - 15);
                                } else
                                {
                                    flyTimer.Stop();

                                    if (words.Length == crossedWords.Count)
                                    {
                                        panel_LevelSuccess.Visible = true;
                                        //label_LevelSuccessScore.Text = row["maxScore"].ToString();
                                        Timer starTimer = new Timer();
                                        starTimer.Interval = 300;
                                        int stars = 1;
                                        int starDelay = 0;
                                        if (gameMusic)
                                        {
                                            Stream strFinish = global::Dictionary.Properties.Resources.sound_level_finish;
                                            SoundPlayer sndFinish = new SoundPlayer(strFinish);
                                            sndFinish.Play();
                                        }
                                        starTimer.Tick += delegate
                                        {
                                            if (starDelay < 3)
                                            {
                                                if (stars == 1)
                                                {
                                                    star_1.BackgroundImage = global::Dictionary.Properties.Resources.sao_dam_2;
                                                }
                                                else if (stars == 2)
                                                {
                                                    star_2.BackgroundImage = global::Dictionary.Properties.Resources.sao_dam_2;
                                                }
                                                else if (stars == 3)
                                                {
                                                    star_3.BackgroundImage = global::Dictionary.Properties.Resources.sao_dam_2;
                                                }
                                                else
                                                {
                                                    starTimer.Stop();
                                                }
                                                stars++;
                                            }
                                            starDelay++;
                                        };
                                        starTimer.Start();

                                        Timer scoreTimer = new Timer();
                                        scoreTimer.Interval = 100;
                                        int maxScore = 0;
                                        scoreTimer.Tick += delegate
                                        {
                                            if (maxScore <= (int)row["maxScore"])
                                            {
                                                label_LevelSuccessScore.Text = (maxScore * 10).ToString();
                                                maxScore++;
                                            }
                                            else
                                            {
                                                scoreTimer.Stop();
                                            }
                                        };
                                        scoreTimer.Start();
                                    }
                                }
                                //i++;

                            }
                            delay++;
                        };
                        flyTimer.Start();
                    }

                    label_CrossingWord.Text = "";

                    
                } else
                {
                    Timer timer = new Timer();
                    timer.Interval = 100;
                    int count = 3;
                    bool isRed = false;
                    if (gameMusic)
                    {
                        Stream str = global::Dictionary.Properties.Resources.incorrect_sound_effect;
                        SoundPlayer snd = new SoundPlayer(str);
                        snd.Play();
                    }
                    timer.Tick += delegate
                    {
                        if (count > 0)
                        {
                            if (isRed)
                            {
                                label_CrossingWord.ForeColor = Color.White;
                                isRed = false;
                                count--;
                            } else
                            {
                                label_CrossingWord.ForeColor = Color.Red;
                                isRed = true;
                            }
                        } else
                        {
                            timer.Stop();
                            label_CrossingWord.Text = "";
                            label_CrossingWord.ForeColor = Color.FromArgb(191, 35, 57);
                        }
                    };
                    timer.Start();
                }
                panel_Letter3.BackgroundImage = global::Dictionary.Properties.Resources.tron_cam_dam;
                panel_Letter4.BackgroundImage = global::Dictionary.Properties.Resources.tron_cam_dam;
                panel_Letter1.BackgroundImage = global::Dictionary.Properties.Resources.tron_cam_nhat;
                panel_Letter2.BackgroundImage = global::Dictionary.Properties.Resources.tron_cam_nhat;



            }
        }

        private void BtnHint_Click(object sender, EventArgs e)
        {
            if (gameMusic)
            {
                Stream str = global::Dictionary.Properties.Resources.sound_button_click;
                SoundPlayer snd = new SoundPlayer(str);
                snd.Play();
            }
            DataRow row = tableLevel.Rows[currentLevel - 1];
            string[] words = (string[])row["words"];
            if (words.Length > crossedWords.Count)
            {
                Random rnd = new Random();
                string word = words[rnd.Next(words.Length)];
                while (crossedWords.FindIndex(x => x == word) >= 0)
                {
                    word = words[rnd.Next(words.Length)];
                }

                int foundIndex = words.ToList().FindIndex(x => x == word);

                List<Panel> newPanels = new List<Panel>();
                foreach (object item in panel_WordResult.Controls)
                {
                    if (item is Panel)
                    {
                        Panel letterPanel = (Panel)item;
                        if (letterPanel.Name.Contains("btn_Letter"))
                        {
                            string indexString = letterPanel.Name.Replace("btn_Letter", "");
                            int rIndex = Convert.ToInt32(indexString[0].ToString());
                            int cIndex = Convert.ToInt32(indexString[2].ToString());
                            if (rIndex == foundIndex + 1 && cIndex <= word.Length)
                            {
                                Panel panel = new Panel();
                                panel.BackColor = Color.Transparent;
                                panel.BackgroundImageLayout = ImageLayout.Zoom;
                                panel.Size = new Size(70, 70);
                                panel.Name = "btn_HintLetter" + rIndex + "x" + cIndex;
                                if (rIndex % 2 == 0)
                                {
                                    panel.BackgroundImage = global::Dictionary.Properties.Resources.vuong_xanh_dam;
                                }
                                else
                                {
                                    panel.BackgroundImage = global::Dictionary.Properties.Resources.vuong_xanh_nhat;
                                }
                                panel.Location = new Point(letterPanel.Location.X, letterPanel.Location.Y);

                                Label label = new Label();
                                label.BackColor = Color.Transparent;
                                label.AutoSize = false;
                                label.Size = new Size(70, 60);
                                label.Location = new Point(0, 0);
                                label.ForeColor = Color.White;
                                label.Font = new Font("Vinhan", 28, FontStyle.Bold);
                                label.Text = word[cIndex - 1].ToString();
                                label.TextAlign = ContentAlignment.MiddleCenter;
                                panel.Controls.Add(label);
                                label.BringToFront();
                                newPanels.Add(panel);
                            }

                        }
                    }
                }
                foreach (Panel item in newPanels)
                {
                    panel_WordResult.Controls.Add(item);
                    item.BringToFront();
                }
                newPanels.Sort((x, y) => x.Name.CompareTo(y.Name));
                int i = 0;
                Timer timer = new Timer();
                timer.Interval = 100;
                int count = 3;
                bool isShow = false;
                timer.Tick += delegate
                {
                    if (count > 0)
                    {
                        if (isShow)
                        {
                            foreach (Panel item in newPanels)
                            {
                                item.Visible = false;
                            }
                            isShow = false;
                            count--;
                        }
                        else
                        {
                            foreach (Panel item in newPanels)
                            {
                                item.Visible = true;
                            }
                            isShow = true;
                        }
                    }
                    else
                    {
                        timer.Stop();
                        List<Panel> removePanel = new List<Panel>();
                        foreach (object item in panel_WordResult.Controls)
                        {
                            if (item is Panel)
                            {
                                Panel panel = (Panel)item;
                                if (panel.Name.Contains("btn_HintLetter"))
                                {
                                    removePanel.Add(panel);
                                }
                            }
                        }
                        foreach (Panel item in removePanel)
                        {
                            panel_WordResult.Controls.Remove(item);
                        }
                    }
                };
                timer.Start();
            }
        }

        private void btn_LevelHome_Click(object sender, EventArgs e)
        {
            if (gameMusic)
            {
                Stream str = global::Dictionary.Properties.Resources.sound_button_click;
                SoundPlayer snd = new SoundPlayer(str);
                snd.Play();
            }
            Render_Game_Home();
        }

        private void btn_LevelAgain_Click(object sender, EventArgs e)
        {
            if (gameMusic)
            {
                Stream str = global::Dictionary.Properties.Resources.sound_button_click;
                SoundPlayer snd = new SoundPlayer(str);
                snd.Play();
            }
            Render_Game_Level(currentLevel);
            panel_LevelSuccess.Visible = false;
        }

        private void btn_LevelNext_Click(object sender, EventArgs e)
        {
            if (gameMusic)
            {
                Stream str = global::Dictionary.Properties.Resources.sound_button_click;
                SoundPlayer snd = new SoundPlayer(str);
                snd.Play();
            }
            if (currentLevel == 11)
            {
                Render_Game_Home();
            }
            else
            {
                Render_Game_Level(currentLevel + 1);
                panel_LevelSuccess.Visible = false;
            }
        }



        private void WordListBtn_Click(object sender, EventArgs e)
        {
            PnToeicWords.BringToFront();
            PnToeicGame.Visible = false;
            PnToeicWords.AutoScroll = true;
            this.KeyPreview = false;
        }

        void PnToeicWords_Load()
        {

            flowToeicWords.Controls.Clear();
            for (char letter = 'a'; letter <= 'z'; letter++)
            {
                Panel Container = new Panel();
                Container.AutoSize = true;
                flowToeicWords.Controls.Add(Container);
                Label Title = new Label();
                Title.AutoSize = true;
                Title.Text = letter.ToString().ToUpper();
                Title.Margin = new Padding(3);
                Title.Font = new Font("Roboto", 22);
                Title.ForeColor = Color.FromArgb(15, 23, 59);
                Title.Location = new Point(3, 1);
                Container.Controls.Add(Title);
                FlowLayoutPanel WordsContainer = new FlowLayoutPanel();
                WordsContainer.AutoSize = true;
                //WordsContainer.Size = new Size(990, 100);
                WordsContainer.MaximumSize = new Size(996, int.MaxValue);
                WordsContainer.Location = new Point(2, 38);
                Container.Controls.Add(WordsContainer);
                foreach (string w in ToeicWordsList)
                {
                    if (w.StartsWith(letter.ToString()))
                    {
                        Label TWords = new Label();
                        TWords.AutoSize = true;
                        TWords.Cursor = Cursors.Hand;
                        TWords.Text = w;
                        TWords.Click += Handle_Word_Click;
                        TWords.MouseMove += delegate
                        {
                            TWords.Font = new Font("Roboto Light", 16, FontStyle.Underline);
                        };
                        TWords.MouseLeave += delegate
                        {
                            TWords.Font = new Font("Roboto Light", 16, FontStyle.Regular);
                        };
                        TWords.Margin = new Padding(5);
                        TWords.Font = new Font("Roboto Light", 16);
                        TWords.ForeColor = Color.FromArgb(15, 23, 59);
                        WordsContainer.Controls.Add(TWords);
                    }
                }
            }
        }

        private void DictionaryBtn_Click(object sender, EventArgs e)
        {
            HomePn.BringToFront();
            this.KeyPreview = false;
        }

        private void BtnToeicGame_Click(object sender, EventArgs e)
        {
            PnToeicGame.Visible = true;
            PnToeicGame.BringToFront();
            PnToeicWords.AutoScroll = false;
            QuestCount = 0;
            CorrectCount = 0;
            LbQuestCount.Text = "Đã trả lời 0 câu";
            LbCorrectCount.Text = "Số câu đúng: ";
            LbIncorrectCount.Text = "Số câu sai: ";
            ToeicGame_CreateQuestion();
        }

        int QuestCount = 0;
        int CorrectCount = 0;

        void ToeicGame_CreateQuestion()
        {
            Random rnd = new Random();
            string[] words = new string[4];
            for (int j = 0; j < 4; j++)
            {
                int v;
                do
                {
                    v = rnd.Next(ToeicWordsList.Count);
                } while (ToeicWordsList[v] == words[j]);
                words[j] = ToeicWordsList[v];
            }
            PnToeicQuest.Controls.Clear();
            FlowAnswerContainer.Controls.Clear();
            Label QuestWord = new Label();
            QuestWord.Text = words[0].ToUpper();
            QuestWord.Font = new Font("Roboto Condensed", 30);
            QuestWord.AutoSize = false;
            QuestWord.Dock = DockStyle.Fill;
            QuestWord.TextAlign = ContentAlignment.MiddleCenter;
            PnToeicQuest.Controls.Add(QuestWord);
            words = words.OrderBy(x => rnd.Next()).ToArray();
            int CorrectAnswer = Array.FindIndex(words, x => x.Contains(QuestWord.Text.ToLower()));
            for (int j = 0; j < 4; j++)
            {
                DataRow row = tableDefinition.Select("word ='" + words[j] + "'").FirstOrDefault();
                if (row == null)
                {
                    ToeicGame_CreateQuestion();
                    return;
                }
                Panel PnAnswer = new Panel();
                PnAnswer.Margin = new Padding(0, 3, 0, 10);
                PnAnswer.BackColor = Color.PowderBlue;
                PnAnswer.Size = new Size(720, 85);
                if (j == CorrectAnswer)
                {
                    PnAnswer.Tag = "true";
                }
                else PnAnswer.Tag = "false";
                FlowAnswerContainer.Controls.Add(PnAnswer);
                Label AnswerText = new Label();
                char OrderLetter = (char)(j + 65);
                AnswerText.Text = OrderLetter + ".   " + row["definition"].ToString();
                AnswerText.AutoSize = false;
                AnswerText.Dock = DockStyle.Fill;
                AnswerText.Font = new Font("Roboto Light", 18);
                AnswerText.ForeColor = Color.FromArgb(15, 23, 59);
                AnswerText.TextAlign = ContentAlignment.MiddleLeft;
                AnswerText.Padding = new Padding(30, 5, 5, 5);
                if (j == CorrectAnswer)
                {
                    AnswerText.Tag = "true";
                }
                else AnswerText.Tag = "false";
                AnswerText.Click += Handle_Answer_Click;
                PnAnswer.Controls.Add(AnswerText);
            }
        }
        void Handle_Answer_Click(object sender, EventArgs e)
        {
            QuestCount++;
            Label label = (Label)sender;
            if (label.Tag.ToString() == "true")
            {
                CorrectCount++;
                if (PbMute.Tag.ToString() == "Unmute")
                {
                    Stream str = global::Dictionary.Properties.Resources.Ding_Sound_Effect;
                    SoundPlayer snd = new SoundPlayer(str);
                    snd.Play();
                }
                label.Parent.BackColor = Color.FromArgb(0, 139, 41);
                label.ForeColor = Color.White;

                foreach (Panel c in FlowAnswerContainer.Controls.OfType<Panel>())
                {
                    Label L = c.Controls.OfType<Label>().First();
                    L.Click -= Handle_Answer_Click;
                }
            }
            else
            {
                label.Parent.BackColor = Color.FromArgb(209, 26, 42);
                label.ForeColor = Color.White;
                if (PbMute.Tag.ToString() == "Unmute")
                {
                    Stream str = global::Dictionary.Properties.Resources.incorrect_sound_effect;
                    SoundPlayer snd = new SoundPlayer(str);
                    snd.Play();
                }
                foreach (Panel c in FlowAnswerContainer.Controls.OfType<Panel>())
                {
                    if (c.Tag.ToString() == "true")
                    {
                        c.BackColor = Color.FromArgb(0, 139, 41);
                        Label lb = c.Controls.OfType<Label>().First();
                        lb.ForeColor = Color.White;
                    }
                    Label L = c.Controls.OfType<Label>().First();
                    L.Click -= Handle_Answer_Click;
                }
            }
            LbQuestCount.Text = "Đã trả lời " + QuestCount.ToString() + " câu";
            LbCorrectCount.Text = "Số câu đúng: " + CorrectCount.ToString();
            LbIncorrectCount.Text = "Số câu sai: " + (QuestCount - CorrectCount).ToString();
        }
        private void BtnNextQuest_Click(object sender, EventArgs e)
        {
            ToeicGame_CreateQuestion();
        }

        private void BtnToeicExit_Click(object sender, EventArgs e)
        {
            PnToeicGame.Visible = false;
            QuestCount = 0;
            CorrectCount = 0;
            PnToeicWords.BringToFront();
            PnToeicWords.AutoScroll = true;
        }

        private void PbMute_Click(object sender, EventArgs e)
        {
            if (PbMute.Tag.ToString() == "Mute")
            {
                PbMute.Tag = "Unmute";
                PbMute.Image = global::Dictionary.Properties.Resources.audio__1_;
            }
            else
            {
                PbMute.Tag = "Mute";
                PbMute.Image = global::Dictionary.Properties.Resources.mute;
            }
        }

        public string Translate(string text, string fromLanguage = "en", string toLanguage = "vi")
        {
            var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLanguage}&tl={toLanguage}&dt=t&q={text}";
            var webClient = new WebClient
            {
                Encoding = System.Text.Encoding.UTF8
            };
            var result = webClient.DownloadString(url);
            try
            {
                result = result.Substring(4, result.IndexOf("\"", 4, StringComparison.Ordinal) - 4);
                return result;
            }
            catch
            {
                return "Error";
            }
        }

        //voice: en-US | vi-VN
        string Get_Audio_Link(string text, string voice = "vi-VN")
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.soundoftext.com/sounds");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var data = new Dictionary<string, string>
            {
                { "text", text },
                { "voice", voice}
            };
                var values = new Dictionary<string, string>
            {
                { "data", data.ToString() },
                { "engine", "Google" }
            };
                string json = "{\"data\":{\"text\":\"" + text + "\"," +
                  "\"voice\":\"" + voice + "\"}," +
                  "\"engine\":\"Google\"}";

                streamWriter.Write(json);
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    string link = "https://storage.soundoftext.com/" + result.Substring(result.IndexOf("id")).Replace("id\":\"", "").Replace("\"}", "") + ".mp3";
                    return link;
                }
            }
            catch (Exception)
            {

                return "";
                throw;
            }
        }

        private void BtnMusic_Click(object sender, EventArgs e)
        {
            if (gameMusic)
            {
                Stream str = global::Dictionary.Properties.Resources.sound_button_click;
                SoundPlayer snd = new SoundPlayer(str);
                snd.Play();
            }
            if (gameMusic)
            {
                BtnMusic.BackgroundImage = global::Dictionary.Properties.Resources.music_off;
            } else
            {
                BtnMusic.BackgroundImage = global::Dictionary.Properties.Resources.music_on;
            }
            gameMusic = !gameMusic;
        }

        private void TranslateBtn_Click(object sender, EventArgs e)
        {
            PnTranslateText.BringToFront();
            this.KeyPreview = false;
        }



        private async void btn_TranslateSrcText_TextChanged(object sender, EventArgs e)
      {
            async Task<bool> UserKeepsTyping()
            {
                string txt = txt_TranslateSrc.Text;
                if (!string.IsNullOrWhiteSpace(txt))
                {
                    await Task.Delay(1000);
                }
                else
                {
                    await Task.Delay(0);
                }
                return txt != txt_TranslateSrc.Text;
            }
            if (await UserKeepsTyping()) return;
            if (!string.IsNullOrWhiteSpace(txt_TranslateSrc.Text) && txt_TranslateSrc.Text != "Nhập văn bản")
            {
                pic_TranslateClear.Visible = true;
                string translatedText = Translate(txt_TranslateSrc.Text, srcLang, desLang);
                srcAudioUrl = Get_Audio_Link(txt_TranslateSrc.Text, srcLang == "vi" ? "vi-VN" : "en-US");
                desAudioUrl = Get_Audio_Link(translatedText, desLang == "vi" ? "vi-VN" : "en-US");
                txt_TranslateDes.Text = translatedText;
            } else
            {
                txt_TranslateDes.Text = "Bản dịch";
                srcAudioUrl = "";
                desAudioUrl = "";
                pic_TranslateClear.Visible = false;
            }
        }

        private void btn_TranslateSrcText_Leave(object sender, EventArgs e)
        {
            if (txt_TranslateSrc.Text == "")
            {
                txt_TranslateSrc.Text = "Nhập văn bản";
            }
        }

        private void txt_TranslateSrc_Click(object sender, EventArgs e)
        {
            if (txt_TranslateSrc.Text == "Nhập văn bản")
            {
                txt_TranslateSrc.Text = "";
            }
        }

        private void pic_TranslateClear_Click(object sender, EventArgs e)
        {
            txt_TranslateSrc.Text = "Nhập văn bản";
            txt_TranslateDes.Text = "Bản dịch";
            srcAudioUrl = "";
            desAudioUrl = "";
            pic_TranslateClear.Visible = false;
        }

        private void btn_TranslateSrcAudio_Click(object sender, EventArgs e)
        {
            SoundPlayer.URL = srcAudioUrl;
        }

        private void btn_TranslateDesAudio_Click(object sender, EventArgs e)
        {
            SoundPlayer.URL = desAudioUrl;
        }

        private async void btn_TranslateSrcCopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_TranslateSrc.Text) && txt_TranslateSrc.Text != "Nhập văn bản")
            {
                System.Windows.Forms.Clipboard.SetText(txt_TranslateSrc.Text);
                label_MessageCopied.Visible = true;
                await Task.Delay(1000);
                label_MessageCopied.Visible = false;
            }
        }

        private async void btn_TranslateDesCopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_TranslateDes.Text) && txt_TranslateDes.Text != "Bản dịch")
            {
                System.Windows.Forms.Clipboard.SetText(txt_TranslateDes.Text);
                label_MessageCopied.Visible = true;
                await Task.Delay(1000);
                label_MessageCopied.Visible = false;
            }
        }

        private void btn_TranslateSwitch_Click(object sender, EventArgs e)
        {
            string tmp = "";

            tmp = label_TranslateSrcLang.Text;
            label_TranslateSrcLang.Text = label_TranslateDesLang.Text;
            label_TranslateDesLang.Text = tmp;

            tmp = srcLang;
            srcLang = desLang;
            desLang = tmp;

            if (!string.IsNullOrWhiteSpace(txt_TranslateSrc.Text) && txt_TranslateSrc.Text!= "Nhập văn bản")
            {
                txt_TranslateSrc.Text = txt_TranslateDes.Text;
            }
        }
    }
}
