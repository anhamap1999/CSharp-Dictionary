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
    public partial class WordType : UserControl
    {
        public WordType()
        {
            InitializeComponent();
        }
        public WordType(string type, List<DataRow> data, int width = 0)
        {
            InitializeComponent();

            if (width != 0)
            {
                this.Width = width;
            }

            Label typeLabel = new Label();
            typeLabel.Text = string.IsNullOrWhiteSpace(type) ? "Khác" : type;
            typeLabel.ForeColor = ColorTranslator.FromHtml("#8E83A6");
            typeLabel.Font = new Font("Roboto Condensed", 15);
            typeLabel.AutoSize = true;
            typeLabel.Margin = new Padding(0, 0, 0, 0);
            typeLabel.Padding = new Padding(0, 0, 0, 0);
            flow_Type.Controls.Add(typeLabel);

            if (data.Count == 0)
            {
                flow_Definition.Visible = false;
            }
            else
            {
                flow_Definition.Height = flow_Type.Location.Y + flow_Type.Height + 11;
                int index = 0;
                foreach (DataRow item in data)
                {
                    WordDefinition wordDefinition = new WordDefinition(item, flow_Definition.Width);
                    int height = wordDefinition.Height;
                    wordDefinition.WordClick += Handle_Word_Click;
                    flow_Definition.Controls.Add(wordDefinition);
                    List<Dictionary<string, string>> examples = (List<Dictionary<string, string>>)item["examples"];
                    if (examples.Count > 0)
                    {
                        flow_Definition.Height += height + 10;
                    } else
                    {
                        flow_Definition.Height += height + 5;
                    }
                    index++;
                    //flow_Definition.Height += examples.Count * 10;
                }
                flow_Definition.Location = new Point(flow_Definition.Location.X, flow_Type.Location.Y + flow_Type.Height + 11);
                flow_Definition.Height += 10;

            }

            this.Height = flow_Definition.Location.Y + flow_Definition.Height + 10;
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
