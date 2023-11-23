namespace MetroBackup
{
    partial class frmTelaAguardeProcesso
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
            this.spnAguarde = new MetroFramework.Controls.MetroProgressSpinner();
            this.lblMsg = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // spnAguarde
            // 
            this.spnAguarde.Location = new System.Drawing.Point(207, 63);
            this.spnAguarde.Maximum = 100;
            this.spnAguarde.Name = "spnAguarde";
            this.spnAguarde.Size = new System.Drawing.Size(93, 75);
            this.spnAguarde.Style = MetroFramework.MetroColorStyle.Orange;
            this.spnAguarde.TabIndex = 0;
            this.spnAguarde.Theme = MetroFramework.MetroThemeStyle.Light;
            this.spnAguarde.UseSelectable = true;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Location = new System.Drawing.Point(20, 150);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 0);
            this.lblMsg.TabIndex = 1;
            // 
            // frmTelaAguardeProcesso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 235);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.spnAguarde);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTelaAguardeProcesso";
            this.Style = MetroFramework.MetroColorStyle.Orange;
            this.Text = "Aguarde";
            this.Load += new System.EventHandler(this.frmTelaAguardeProcesso_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroProgressSpinner spnAguarde;
        public MetroFramework.Controls.MetroLabel lblMsg;
    }
}