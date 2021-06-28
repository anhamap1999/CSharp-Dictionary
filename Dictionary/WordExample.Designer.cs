namespace Dictionary
{
    partial class WordExample
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
            this.flow_Example = new System.Windows.Forms.FlowLayoutPanel();
            this.flow_Explain = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flow_Example
            // 
            this.flow_Example.AutoSize = true;
            this.flow_Example.BackColor = System.Drawing.Color.Transparent;
            this.flow_Example.Location = new System.Drawing.Point(2, 3);
            this.flow_Example.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.flow_Example.Name = "flow_Example";
            this.flow_Example.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.flow_Example.Size = new System.Drawing.Size(1104, 26);
            this.flow_Example.TabIndex = 0;
            // 
            // flow_Explain
            // 
            this.flow_Explain.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flow_Explain.AutoSize = true;
            this.flow_Explain.BackColor = System.Drawing.Color.Transparent;
            this.flow_Explain.Location = new System.Drawing.Point(2, 31);
            this.flow_Explain.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.flow_Explain.Name = "flow_Explain";
            this.flow_Explain.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.flow_Explain.Size = new System.Drawing.Size(1102, 31);
            this.flow_Explain.TabIndex = 1;
            // 
            // WordExample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.flow_Example);
            this.Controls.Add(this.flow_Explain);
            this.Font = new System.Drawing.Font("Roboto Condensed", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "WordExample";
            this.Size = new System.Drawing.Size(1104, 65);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flow_Example;
        private System.Windows.Forms.FlowLayoutPanel flow_Explain;
    }
}
