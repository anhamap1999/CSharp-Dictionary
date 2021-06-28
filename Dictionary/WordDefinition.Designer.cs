namespace Dictionary
{
    partial class WordDefinition
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
            this.flow_Synonym = new System.Windows.Forms.FlowLayoutPanel();
            this.flow_Example = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel_ExampleTitle = new System.Windows.Forms.Panel();
            this.flow_Explain = new System.Windows.Forms.FlowLayoutPanel();
            this.flow_Definition = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.flow_Synonym.SuspendLayout();
            this.flow_Example.SuspendLayout();
            this.panel_ExampleTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // flow_Synonym
            // 
            this.flow_Synonym.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flow_Synonym.AutoSize = true;
            this.flow_Synonym.BackColor = System.Drawing.Color.Transparent;
            this.flow_Synonym.Controls.Add(this.label2);
            this.flow_Synonym.Location = new System.Drawing.Point(26, 30);
            this.flow_Synonym.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.flow_Synonym.Name = "flow_Synonym";
            this.flow_Synonym.Padding = new System.Windows.Forms.Padding(5);
            this.flow_Synonym.Size = new System.Drawing.Size(1104, 29);
            this.flow_Synonym.TabIndex = 0;
            // 
            // flow_Example
            // 
            this.flow_Example.BackColor = System.Drawing.Color.Thistle;
            this.flow_Example.Controls.Add(this.panel_ExampleTitle);
            this.flow_Example.Location = new System.Drawing.Point(26, 86);
            this.flow_Example.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.flow_Example.Name = "flow_Example";
            this.flow_Example.Padding = new System.Windows.Forms.Padding(5);
            this.flow_Example.Size = new System.Drawing.Size(1104, 50);
            this.flow_Example.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Roboto Condensed", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ví dụ:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Roboto Condensed", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(8, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Đồng nghĩa:";
            // 
            // panel_ExampleTitle
            // 
            this.panel_ExampleTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_ExampleTitle.Controls.Add(this.label1);
            this.panel_ExampleTitle.Location = new System.Drawing.Point(8, 8);
            this.panel_ExampleTitle.Name = "panel_ExampleTitle";
            this.panel_ExampleTitle.Size = new System.Drawing.Size(1094, 25);
            this.panel_ExampleTitle.TabIndex = 4;
            // 
            // flow_Explain
            // 
            this.flow_Explain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flow_Explain.AutoSize = true;
            this.flow_Explain.BackColor = System.Drawing.Color.Transparent;
            this.flow_Explain.Location = new System.Drawing.Point(26, 59);
            this.flow_Explain.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.flow_Explain.Name = "flow_Explain";
            this.flow_Explain.Padding = new System.Windows.Forms.Padding(5);
            this.flow_Explain.Size = new System.Drawing.Size(1104, 29);
            this.flow_Explain.TabIndex = 4;
            // 
            // flow_Definition
            // 
            this.flow_Definition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flow_Definition.AutoSize = true;
            this.flow_Definition.BackColor = System.Drawing.Color.Transparent;
            this.flow_Definition.Location = new System.Drawing.Point(18, 0);
            this.flow_Definition.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.flow_Definition.Name = "flow_Definition";
            this.flow_Definition.Padding = new System.Windows.Forms.Padding(5);
            this.flow_Definition.Size = new System.Drawing.Size(1112, 29);
            this.flow_Definition.TabIndex = 5;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Dictionary.Properties.Resources.dot;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(7, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(10, 10);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // WordDefinition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.flow_Definition);
            this.Controls.Add(this.flow_Explain);
            this.Controls.Add(this.flow_Synonym);
            this.Controls.Add(this.flow_Example);
            this.Font = new System.Drawing.Font("Roboto Condensed", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "WordDefinition";
            this.Size = new System.Drawing.Size(1148, 139);
            this.flow_Synonym.ResumeLayout(false);
            this.flow_Synonym.PerformLayout();
            this.flow_Example.ResumeLayout(false);
            this.panel_ExampleTitle.ResumeLayout(false);
            this.panel_ExampleTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flow_Synonym;
        private System.Windows.Forms.FlowLayoutPanel flow_Example;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel_ExampleTitle;
        private System.Windows.Forms.FlowLayoutPanel flow_Explain;
        private System.Windows.Forms.FlowLayoutPanel flow_Definition;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
