namespace MetroSmartBackup
{
    partial class frmTelaAguardeProcessoProgressBar
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.metroProgressBar = new MetroFramework.Controls.MetroProgressBar();
            this.lblMsg = new MetroFramework.Controls.MetroLabel();
            this.lblProgresso = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // metroProgressBar
            // 
            this.metroProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroProgressBar.Location = new System.Drawing.Point(23, 54);
            this.metroProgressBar.Name = "metroProgressBar";
            this.metroProgressBar.Size = new System.Drawing.Size(323, 19);
            this.metroProgressBar.TabIndex = 0;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblMsg.Location = new System.Drawing.Point(23, 85);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(145, 15);
            this.lblMsg.TabIndex = 2;
            this.lblMsg.Text = "Aguarde iniciando backup...";
            // 
            // lblProgresso
            // 
            this.lblProgresso.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgresso.AutoSize = true;
            this.lblProgresso.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.lblProgresso.Location = new System.Drawing.Point(352, 49);
            this.lblProgresso.Name = "lblProgresso";
            this.lblProgresso.Size = new System.Drawing.Size(35, 25);
            this.lblProgresso.TabIndex = 3;
            this.lblProgresso.Text = "0%";
            // 
            // frmTelaAguardeProcessoProgressBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 155);
            this.Controls.Add(this.lblProgresso);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.metroProgressBar);
            this.MaximizeBox = false;
            this.Name = "frmTelaAguardeProcessoProgressBar";
            this.Resizable = false;
            this.Text = "Aguarde";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTelaAguardeProcessoProgressBar_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public MetroFramework.Controls.MetroLabel lblMsg;
        public MetroFramework.Controls.MetroProgressBar metroProgressBar;
        public MetroFramework.Controls.MetroLabel lblProgresso;
    }
}