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
    public partial class SaveWordItem : UserControl
    {
        public SaveWordItem()
        {
            InitializeComponent();
        }

        List<string> savedWords;
        public SaveWordItem(DataRow row, List<string> SWords)
        {
            InitializeComponent();
            savedWords = SWords;
            string word = row["word"].ToString();
            string type = row["type"].ToString();
            string definition = row["definition"].ToString();

            LbWordSaved.Text = word;
            LbWordType.Text = type;
            LbWordDef.Text = definition;
            LbWordType.Location = new Point(22 + LbWordSaved.Width, 17);
            LbWordSaved.Click += handle_Word_Click;
            LbWordSaved.MouseMove += handle_Word_Mouse_Move;
            LbWordSaved.MouseLeave += handle_Word_Mouse_Leave;
            pic_WordUnsave.Click += handle_PicUnsave_Click;
        }
        void handle_Word_Mouse_Move(object sender, MouseEventArgs e)
        {
            LbWordSaved.Font = new Font("Roboto Condensed", 14, FontStyle.Underline);
        }
        void handle_Word_Mouse_Leave(object sender, EventArgs e)
        {
            LbWordSaved.Font = new Font("Roboto Condensed", 14, FontStyle.Regular);
        }
        void handle_Word_Click(object sender, EventArgs e)
        {
            onWordClick(sender, new EventArgs());
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

        void handle_PicUnsave_Click(object sender, EventArgs e)
        {
            int index = savedWords.FindIndex(x => x == LbWordSaved.Text);
            if (index >= 0)
            {
                savedWords.RemoveAt(index);
                pic_WordUnsave.BackgroundImage = global::Dictionary.Properties.Resources.unsaved;
            }
        }
    }
}
