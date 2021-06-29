
namespace Dictionary
{
    partial class SaveWordItem
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveWordItem));
            this.WLIPanel = new System.Windows.Forms.Panel();
            this.LbWordDef = new System.Windows.Forms.Label();
            this.LbWordType = new System.Windows.Forms.Label();
            this.LbWordSaved = new System.Windows.Forms.Label();
            this.pic_WordUnsave = new System.Windows.Forms.PictureBox();
            this.WLIPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_WordUnsave)).BeginInit();
            this.SuspendLayout();
            // 
            // WLIPanel
            // 
            this.WLIPanel.Controls.Add(this.LbWordDef);
            this.WLIPanel.Location = new System.Drawing.Point(16, 39);
            this.WLIPanel.Name = "WLIPanel";
            this.WLIPanel.Size = new System.Drawing.Size(1016, 47);
            this.WLIPanel.TabIndex = 2;
            // 
            // LbWordDef
            // 
            this.LbWordDef.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LbWordDef.Font = new System.Drawing.Font("Roboto Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbWordDef.Location = new System.Drawing.Point(0, 0);
            this.LbWordDef.Name = "LbWordDef";
            this.LbWordDef.Size = new System.Drawing.Size(1016, 47);
            this.LbWordDef.TabIndex = 0;
            this.LbWordDef.Text = resources.GetString("LbWordDef.Text");
            // 
            // LbWordType
            // 
            this.LbWordType.AutoSize = true;
            this.LbWordType.Font = new System.Drawing.Font("Roboto", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbWordType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(59)))));
            this.LbWordType.Location = new System.Drawing.Point(125, 17);
            this.LbWordType.Name = "LbWordType";
            this.LbWordType.Size = new System.Drawing.Size(35, 15);
            this.LbWordType.TabIndex = 1;
            this.LbWordType.Text = "noun";
            // 
            // LbWordSaved
            // 
            this.LbWordSaved.AutoSize = true;
            this.LbWordSaved.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LbWordSaved.Font = new System.Drawing.Font("Roboto", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbWordSaved.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(59)))));
            this.LbWordSaved.Location = new System.Drawing.Point(16, 11);
            this.LbWordSaved.Name = "LbWordSaved";
            this.LbWordSaved.Size = new System.Drawing.Size(103, 23);
            this.LbWordSaved.TabIndex = 0;
            this.LbWordSaved.Text = "WordABCD";
            // 
            // pic_WordUnsave
            // 
            this.pic_WordUnsave.BackColor = System.Drawing.Color.Transparent;
            this.pic_WordUnsave.BackgroundImage = global::Dictionary.Properties.Resources.saved;
            this.pic_WordUnsave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pic_WordUnsave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pic_WordUnsave.Location = new System.Drawing.Point(1007, 9);
            this.pic_WordUnsave.Name = "pic_WordUnsave";
            this.pic_WordUnsave.Size = new System.Drawing.Size(25, 25);
            this.pic_WordUnsave.TabIndex = 5;
            this.pic_WordUnsave.TabStop = false;
            // 
            // SaveWordItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.pic_WordUnsave);
            this.Controls.Add(this.LbWordSaved);
            this.Controls.Add(this.LbWordType);
            this.Controls.Add(this.WLIPanel);
            this.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            this.Name = "SaveWordItem";
            this.Size = new System.Drawing.Size(1045, 90);
            this.WLIPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pic_WordUnsave)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel WLIPanel;
        private System.Windows.Forms.Label LbWordDef;
        private System.Windows.Forms.Label LbWordType;
        private System.Windows.Forms.Label LbWordSaved;
        private System.Windows.Forms.PictureBox pic_WordUnsave;
    }
}
