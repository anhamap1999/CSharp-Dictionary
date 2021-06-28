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
    public partial class WordExample : UserControl
    {
        public WordExample()
        {
            InitializeComponent();
        }
        public WordExample(string example = "hello word", string explain = "hello word", int width = 0)
        {
            InitializeComponent();
            if (width != 0)
            {
                this.Width = width;
            }
            string[] exampleWords = example.Split(' ');
            flow_Example.Controls.Clear();
            foreach (string word in exampleWords)
            {
                Label label = new Label();
                label.Text = word;
                label.ForeColor = ColorTranslator.FromHtml("#303A63");
                label.Font = new Font("Roboto Condensed", 14, FontStyle.Bold);
                label.Cursor = Cursors.Hand;
                label.AutoSize = true;
                label.Margin = new Padding(0, 0, 0, 0);
                label.Padding = new Padding(0, 0, 0, 0);
                label.MouseMove += handle_Word_Mouse_Move;
                label.MouseLeave += handle_Word_Mouse_Leave;
                label.Click += handle_Word_Click;
                flow_Example.Controls.Add(label);
            }
            //flow_Example.Width = this.Width;

            flow_Explain.Controls.Clear();
            Label explainLabel = new Label();
            explainLabel.Text = explain;
            explainLabel.ForeColor = ColorTranslator.FromHtml("#000");
            explainLabel.Font = new Font("Roboto Condensed", 12);
            explainLabel.AutoSize = true;
            explainLabel.Margin = new Padding(0, 0, 0, 0);
            explainLabel.Padding = new Padding(0, 0, 0, 0);
            flow_Explain.Controls.Add(explainLabel);
            //flow_Explain.Width = this.Width;
            this.Height = flow_Example.Height + flow_Explain.Height;

            flow_Explain.Location = new Point(0, flow_Example.Height);
            //label1.Text = explain;
            //label1.Width = this.Width;
            //label1.Location = new Point(0, flow_Example.Height);

            //this.Height = flow_Example.Height + label1.Height;
        }

        void handle_Word_Event(object sender, string type)
        {
            Label label = (Label)sender;
            switch (type)
            {
                case "mouse move":
                    label.Font = new Font("Roboto Condensed", 14, FontStyle.Bold ^ FontStyle.Underline);
                    break;

                case "mouse leave":
                    label.Font = new Font("Roboto Condensed", 14, FontStyle.Bold);
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
    }
}
