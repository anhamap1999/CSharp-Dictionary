namespace Dictionary
{
    partial class WordType
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
            this.flow_Type = new System.Windows.Forms.FlowLayoutPanel();
            this.flow_Definition = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // flow_Type
            // 
            this.flow_Type.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flow_Type.AutoSize = true;
            this.flow_Type.BackColor = System.Drawing.Color.Transparent;
            this.flow_Type.Location = new System.Drawing.Point(5, 9);
            this.flow_Type.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.flow_Type.Name = "flow_Type";
            this.flow_Type.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.flow_Type.Size = new System.Drawing.Size(1161, 26);
            this.flow_Type.TabIndex = 2;
            // 
            // flow_Definition
            // 
            this.flow_Definition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.flow_Definition.BackColor = System.Drawing.Color.Transparent;
            this.flow_Definition.Location = new System.Drawing.Point(18, 46);
            this.flow_Definition.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.flow_Definition.Name = "flow_Definition";
            this.flow_Definition.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.flow_Definition.Size = new System.Drawing.Size(1148, 31);
            this.flow_Definition.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(131)))), ((int)(((byte)(166)))));
            this.panel1.Location = new System.Drawing.Point(5, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1161, 1);
            this.panel1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(131)))), ((int)(((byte)(166)))));
            this.panel2.Location = new System.Drawing.Point(5, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1161, 1);
            this.panel2.TabIndex = 5;
            // 
            // WordType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.flow_Type);
            this.Controls.Add(this.flow_Definition);
            this.Font = new System.Drawing.Font("Roboto Condensed", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "WordType";
            this.Size = new System.Drawing.Size(1169, 80);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flow_Type;
        private System.Windows.Forms.FlowLayoutPanel flow_Definition;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}
