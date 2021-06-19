﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Dictionary
{
    public partial class Dictionary : Form
    {
        public Dictionary()
        {
            InitializeComponent();
            PicOfWordPb.ImageLocation = "https://cdn.britannica.com/22/206222-131-E921E1FB/Domestic-feline-tabby-cat.jpg";
        }

        private void SearchBox_Enter(object sender, EventArgs e)
        {
            if(SearchBox.Text == "Search some word...")
            {
                SearchBox.Text = "";
                SearchBox.ForeColor = Color.Black;
            }
        }

        private void SearchBox_Leave(object sender, EventArgs e)
        {
            if(SearchBox.Text == "")
            {
                SearchBox.Text = "Search some word...";
                SearchBox.ForeColor = Color.Silver;
            }
        }
    }
}
