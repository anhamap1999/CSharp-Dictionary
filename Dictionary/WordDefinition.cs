using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dictionary
{
    public partial class WordDefinition : UserControl
    {
        public WordDefinition()
        {
            InitializeComponent();
        }
        public WordDefinition(DataRow data, int width = 0)
        {
            InitializeComponent();
            if (width != 0)
            {
                this.Width = width;
            }

            int height = flow_Definition.Location.Y;

            string definition = data["definition"].ToString();
            string synonym = data["synonym"].ToString();
            string explain = data["explain"].ToString();
            List<Dictionary<string, string>> examples = (List<Dictionary<string, string>>)data["examples"];

            Label definitionLabel = new Label();
            definitionLabel.Text = definition;
            definitionLabel.ForeColor = ColorTranslator.FromHtml("#202855");
            definitionLabel.Font = new Font("Roboto Condensed", 14);
            definitionLabel.AutoSize = true;
            definitionLabel.Margin = new Padding(0, 0, 0, 0);
            definitionLabel.Padding = new Padding(0, 0, 0, 0);
            flow_Definition.Controls.Add(definitionLabel);

            height += flow_Definition.Height + 10;

            if (string.IsNullOrWhiteSpace(synonym))
            {
                flow_Synonym.Visible = false;
            }
            else
            {
                string[] array = synonym.Split(' ');
                foreach (string item in array)
                {
                    Label synonymLabel = new Label();
                    synonymLabel.Text = item;
                    synonymLabel.ForeColor = ColorTranslator.FromHtml("#8E83A6");
                    synonymLabel.Font = new Font("Roboto Condensed", 12);
                    synonymLabel.AutoSize = true;
                    synonymLabel.Margin = new Padding(0, 0, 0, 0);
                    synonymLabel.Padding = new Padding(0, 0, 0, 0);
                    synonymLabel.Cursor = Cursors.Hand;
                    synonymLabel.MouseMove += handle_Word_Mouse_Move;
                    synonymLabel.MouseLeave += handle_Word_Mouse_Leave;
                    synonymLabel.Click += handle_Word_Click;
                    flow_Synonym.Controls.Add(synonymLabel);
                }
                //Label synonymLabel = new Label();
                //synonymLabel.Text = synonym;
                //synonymLabel.ForeColor = ColorTranslator.FromHtml("#8E83A6");
                //synonymLabel.Font = new Font("Roboto Condensed", 13);
                //synonymLabel.AutoSize = true;
                //synonymLabel.Margin = new Padding(0, 0, 0, 0);
                //synonymLabel.Padding = new Padding(0, 0, 0, 0);
                //synonymLabel.MouseMove += handle_Word_Mouse_Move;
                //synonymLabel.MouseLeave += handle_Word_Mouse_Leave;
                //synonymLabel.Click += handle_Word_Click;
                //flow_Synonym.Controls.Add(synonymLabel);
                //flow_Synonym.Width = this.Width;

                flow_Synonym.Location = new Point(flow_Synonym.Location.X, flow_Definition.Location.Y + flow_Definition.Height + 5);

                height = flow_Synonym.Location.Y + flow_Synonym.Height + 5;
            }
            if (string.IsNullOrWhiteSpace(explain))
            {
                flow_Explain.Visible = false;
            }
            else
            {
                Label explainLabel = new Label();
                explainLabel.Text = explain;
                explainLabel.ForeColor = ColorTranslator.FromHtml("#5c5c5c");
                explainLabel.Font = new Font("Roboto Condensed", 11);
                explainLabel.AutoSize = true;
                explainLabel.Margin = new Padding(0, 0, 0, 0);
                explainLabel.Padding = new Padding(0, 0, 0, 0);
                flow_Explain.Controls.Add(explainLabel);
                flow_Explain.Width = this.Width;
                //label_Explain.Text = explain;
                //flow_Explain.Width = this.Width;
                if (flow_Synonym.Visible)
                {
                    flow_Explain.Location = new Point(flow_Explain.Location.X, flow_Synonym.Location.Y + flow_Synonym.Height + 5);
                } else
                {
                    flow_Explain.Location = new Point(flow_Explain.Location.X, flow_Definition.Location.Y + flow_Definition.Height + 5);
                }

                height = flow_Explain.Location.Y + flow_Explain.Height + 5;
            }
            if (examples.Count == 0)
            {
                flow_Example.Visible = false;
                height += 5;
            }
            else
            {
                flow_Example.BackColor = Color.FromArgb(50, 216, 191, 216);
                panel_ExampleTitle.BackColor = Color.Transparent;
                panel_ExampleTitle.Width = flow_Example.Width;

                flow_Example.Height = panel_ExampleTitle.Height + 5;
                foreach (var item in examples)
                {
                    WordExample wordExample = new WordExample(item["example"], item["explain"], flow_Example.Width);
                    int exampleHeight = wordExample.Height;
                    wordExample.WordClick += Handle_Word_Click;
                    flow_Example.Controls.Add(wordExample);
                    flow_Example.Height += exampleHeight + 10;
                }
                if (flow_Explain.Visible)
                {
                    flow_Example.Location = new Point(flow_Example.Location.X, flow_Explain.Location.Y + flow_Explain.Height + 5);
                }
                else
                {
                    if (flow_Synonym.Visible)
                    {
                        flow_Example.Location = new Point(flow_Example.Location.X, flow_Synonym.Location.Y + flow_Synonym.Height + 5);
                    }
                    else
                    {
                        flow_Example.Location = new Point(flow_Example.Location.X, flow_Definition.Location.Y + flow_Definition.Height + 5);
                    }
                }

                height = flow_Example.Location.Y + flow_Example.Height + 10;
            }

            this.Height = height + 10;

        }

        void handle_Word_Event(object sender, string type)
        {
            Label label = (Label)sender;
            switch (type)
            {
                case "mouse move":
                    label.Font = new Font("Roboto Condensed", 12, FontStyle.Underline);
                    break;

                case "mouse leave":
                    label.Font = new Font("Roboto Condensed", 12, FontStyle.Regular);
                    break;
                case "click":
                    onWordClick(sender, new EventArgs());

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
        public delegate void WordClickEventHandler(object sender, EventArgs e);

        public event WordClickEventHandler WordClick;

        protected virtual void onWordClick(object sender, EventArgs e)
        {
            if (e != null)
            {
                WordClick(sender, e);
            }
        }

        void Handle_Word_Click(object sender, EventArgs e)
        {
            onWordClick(sender, new EventArgs());
        }
    }
}
