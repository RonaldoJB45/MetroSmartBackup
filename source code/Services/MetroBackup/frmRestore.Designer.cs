namespace MetroSmartBackup
{
    partial class frmRestore
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
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.lblBytesProcessados = new System.Windows.Forms.Label();
            this.lblPorcentagem = new System.Windows.Forms.Label();
            this.progressRestore = new MetroFramework.Controls.MetroProgressBar();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.metroPanel3 = new MetroFramework.Controls.MetroPanel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.btnLocalizarArquivo = new MetroFramework.Controls.MetroButton();
            this.txtCaminhoArquivo = new MetroFramework.Controls.MetroTextBox();
            this.metroPanel2 = new MetroFramework.Controls.MetroPanel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.cmbDatabases = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.pnlConexao = new MetroFramework.Controls.MetroPanel();
            this.txtSenha = new MetroFramework.Controls.MetroTextBox();
            this.btnConectar = new MetroFramework.Controls.MetroButton();
            this.txtIp = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel10 = new MetroFramework.Controls.MetroLabel();
            this.txtPorta = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel13 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel12 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel11 = new MetroFramework.Controls.MetroLabel();
            this.txtUsuario = new MetroFramework.Controls.MetroTextBox();
            this.btnRestore = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.metroPanel1.SuspendLayout();
            this.metroPanel3.SuspendLayout();
            this.metroPanel2.SuspendLayout();
            this.pnlConexao.SuspendLayout();
            this.SuspendLayout();
            // 
            // picLogo
            // 
            this.picLogo.Image = global::MetroSmartBackup.Properties.Resources.railwaystationxxl;
            this.picLogo.Location = new System.Drawing.Point(37, 29);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(79, 65);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLogo.TabIndex = 31;
            this.picLogo.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(122, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(268, 31);
            this.label1.TabIndex = 32;
            this.label1.Text = "MetroSmartRestore";
            // 
            // metroPanel1
            // 
            this.metroPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroPanel1.Controls.Add(this.lblBytesProcessados);
            this.metroPanel1.Controls.Add(this.lblPorcentagem);
            this.metroPanel1.Controls.Add(this.progressRestore);
            this.metroPanel1.Controls.Add(this.metroLabel5);
            this.metroPanel1.Controls.Add(this.metroLabel4);
            this.metroPanel1.Controls.Add(this.metroPanel3);
            this.metroPanel1.Controls.Add(this.metroPanel2);
            this.metroPanel1.Controls.Add(this.metroLabel1);
            this.metroPanel1.Controls.Add(this.pnlConexao);
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(23, 100);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(591, 266);
            this.metroPanel1.TabIndex = 33;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // lblBytesProcessados
            // 
            this.lblBytesProcessados.AutoSize = true;
            this.lblBytesProcessados.BackColor = System.Drawing.Color.Transparent;
            this.lblBytesProcessados.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBytesProcessados.Location = new System.Drawing.Point(11, 251);
            this.lblBytesProcessados.Name = "lblBytesProcessados";
            this.lblBytesProcessados.Size = new System.Drawing.Size(0, 13);
            this.lblBytesProcessados.TabIndex = 50;
            // 
            // lblPorcentagem
            // 
            this.lblPorcentagem.AutoSize = true;
            this.lblPorcentagem.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPorcentagem.Location = new System.Drawing.Point(537, 223);
            this.lblPorcentagem.Name = "lblPorcentagem";
            this.lblPorcentagem.Size = new System.Drawing.Size(0, 25);
            this.lblPorcentagem.TabIndex = 49;
            // 
            // progressRestore
            // 
            this.progressRestore.Location = new System.Drawing.Point(14, 223);
            this.progressRestore.Name = "progressRestore";
            this.progressRestore.Size = new System.Drawing.Size(517, 25);
            this.progressRestore.TabIndex = 48;
            this.progressRestore.Visible = false;
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.Location = new System.Drawing.Point(228, 122);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(138, 19);
            this.metroLabel5.TabIndex = 41;
            this.metroLabel5.Text = "3. Selecione o arquivo";
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(228, 13);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(130, 19);
            this.metroLabel4.TabIndex = 40;
            this.metroLabel4.Text = "2. Selecione o banco";
            // 
            // metroPanel3
            // 
            this.metroPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroPanel3.Controls.Add(this.metroLabel3);
            this.metroPanel3.Controls.Add(this.btnLocalizarArquivo);
            this.metroPanel3.Controls.Add(this.txtCaminhoArquivo);
            this.metroPanel3.HorizontalScrollbarBarColor = true;
            this.metroPanel3.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel3.HorizontalScrollbarSize = 10;
            this.metroPanel3.Location = new System.Drawing.Point(228, 144);
            this.metroPanel3.Name = "metroPanel3";
            this.metroPanel3.Size = new System.Drawing.Size(339, 73);
            this.metroPanel3.TabIndex = 39;
            this.metroPanel3.VerticalScrollbarBarColor = true;
            this.metroPanel3.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel3.VerticalScrollbarSize = 10;
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(18, 7);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(55, 19);
            this.metroLabel3.TabIndex = 29;
            this.metroLabel3.Text = "Arquivo";
            // 
            // btnLocalizarArquivo
            // 
            this.btnLocalizarArquivo.Location = new System.Drawing.Point(290, 29);
            this.btnLocalizarArquivo.Name = "btnLocalizarArquivo";
            this.btnLocalizarArquivo.Size = new System.Drawing.Size(36, 23);
            this.btnLocalizarArquivo.TabIndex = 3;
            this.btnLocalizarArquivo.Text = "...";
            this.btnLocalizarArquivo.UseSelectable = true;
            this.btnLocalizarArquivo.Click += new System.EventHandler(this.btnLocalizarArquivo_Click);
            // 
            // txtCaminhoArquivo
            // 
            // 
            // 
            // 
            this.txtCaminhoArquivo.CustomButton.Image = null;
            this.txtCaminhoArquivo.CustomButton.Location = new System.Drawing.Point(244, 1);
            this.txtCaminhoArquivo.CustomButton.Name = "";
            this.txtCaminhoArquivo.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtCaminhoArquivo.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtCaminhoArquivo.CustomButton.TabIndex = 1;
            this.txtCaminhoArquivo.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtCaminhoArquivo.CustomButton.UseSelectable = true;
            this.txtCaminhoArquivo.CustomButton.Visible = false;
            this.txtCaminhoArquivo.Lines = new string[0];
            this.txtCaminhoArquivo.Location = new System.Drawing.Point(18, 29);
            this.txtCaminhoArquivo.MaxLength = 32767;
            this.txtCaminhoArquivo.Name = "txtCaminhoArquivo";
            this.txtCaminhoArquivo.PasswordChar = '\0';
            this.txtCaminhoArquivo.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCaminhoArquivo.SelectedText = "";
            this.txtCaminhoArquivo.SelectionLength = 0;
            this.txtCaminhoArquivo.SelectionStart = 0;
            this.txtCaminhoArquivo.ShortcutsEnabled = true;
            this.txtCaminhoArquivo.Size = new System.Drawing.Size(266, 23);
            this.txtCaminhoArquivo.TabIndex = 2;
            this.txtCaminhoArquivo.UseSelectable = true;
            this.txtCaminhoArquivo.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtCaminhoArquivo.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroPanel2
            // 
            this.metroPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroPanel2.Controls.Add(this.metroLabel2);
            this.metroPanel2.Controls.Add(this.cmbDatabases);
            this.metroPanel2.HorizontalScrollbarBarColor = true;
            this.metroPanel2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel2.HorizontalScrollbarSize = 10;
            this.metroPanel2.Location = new System.Drawing.Point(228, 35);
            this.metroPanel2.Name = "metroPanel2";
            this.metroPanel2.Size = new System.Drawing.Size(339, 84);
            this.metroPanel2.TabIndex = 38;
            this.metroPanel2.VerticalScrollbarBarColor = true;
            this.metroPanel2.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel2.VerticalScrollbarSize = 10;
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(17, 12);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(105, 19);
            this.metroLabel2.TabIndex = 28;
            this.metroLabel2.Text = "Banco de Dados";
            // 
            // cmbDatabases
            // 
            this.cmbDatabases.FormattingEnabled = true;
            this.cmbDatabases.ItemHeight = 23;
            this.cmbDatabases.Location = new System.Drawing.Point(17, 34);
            this.cmbDatabases.Name = "cmbDatabases";
            this.cmbDatabases.Size = new System.Drawing.Size(309, 29);
            this.cmbDatabases.TabIndex = 2;
            this.cmbDatabases.UseSelectable = true;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(14, 13);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(171, 19);
            this.metroLabel1.TabIndex = 37;
            this.metroLabel1.Text = "1. Conecte ao MySql Server";
            // 
            // pnlConexao
            // 
            this.pnlConexao.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlConexao.Controls.Add(this.txtSenha);
            this.pnlConexao.Controls.Add(this.btnConectar);
            this.pnlConexao.Controls.Add(this.txtIp);
            this.pnlConexao.Controls.Add(this.metroLabel10);
            this.pnlConexao.Controls.Add(this.txtPorta);
            this.pnlConexao.Controls.Add(this.metroLabel13);
            this.pnlConexao.Controls.Add(this.metroLabel12);
            this.pnlConexao.Controls.Add(this.metroLabel11);
            this.pnlConexao.Controls.Add(this.txtUsuario);
            this.pnlConexao.HorizontalScrollbarBarColor = true;
            this.pnlConexao.HorizontalScrollbarHighlightOnWheel = false;
            this.pnlConexao.HorizontalScrollbarSize = 10;
            this.pnlConexao.Location = new System.Drawing.Point(14, 35);
            this.pnlConexao.Name = "pnlConexao";
            this.pnlConexao.Size = new System.Drawing.Size(207, 182);
            this.pnlConexao.TabIndex = 36;
            this.pnlConexao.VerticalScrollbarBarColor = true;
            this.pnlConexao.VerticalScrollbarHighlightOnWheel = false;
            this.pnlConexao.VerticalScrollbarSize = 10;
            // 
            // txtSenha
            // 
            // 
            // 
            // 
            this.txtSenha.CustomButton.Image = null;
            this.txtSenha.CustomButton.Location = new System.Drawing.Point(73, 1);
            this.txtSenha.CustomButton.Name = "";
            this.txtSenha.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtSenha.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtSenha.CustomButton.TabIndex = 1;
            this.txtSenha.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtSenha.CustomButton.UseSelectable = true;
            this.txtSenha.CustomButton.Visible = false;
            this.txtSenha.Lines = new string[] {
        "root"};
            this.txtSenha.Location = new System.Drawing.Point(96, 87);
            this.txtSenha.MaxLength = 32767;
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.PasswordChar = '*';
            this.txtSenha.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtSenha.SelectedText = "";
            this.txtSenha.SelectionLength = 0;
            this.txtSenha.SelectionStart = 0;
            this.txtSenha.ShortcutsEnabled = true;
            this.txtSenha.Size = new System.Drawing.Size(95, 23);
            this.txtSenha.TabIndex = 34;
            this.txtSenha.Text = "root";
            this.txtSenha.UseSelectable = true;
            this.txtSenha.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtSenha.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // btnConectar
            // 
            this.btnConectar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnConectar.Location = new System.Drawing.Point(15, 119);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(176, 42);
            this.btnConectar.TabIndex = 35;
            this.btnConectar.Text = "Conectar";
            this.btnConectar.UseSelectable = true;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // txtIp
            // 
            // 
            // 
            // 
            this.txtIp.CustomButton.Image = null;
            this.txtIp.CustomButton.Location = new System.Drawing.Point(93, 1);
            this.txtIp.CustomButton.Name = "";
            this.txtIp.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtIp.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtIp.CustomButton.TabIndex = 1;
            this.txtIp.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtIp.CustomButton.UseSelectable = true;
            this.txtIp.CustomButton.Visible = false;
            this.txtIp.Lines = new string[] {
        "localhost"};
            this.txtIp.Location = new System.Drawing.Point(15, 34);
            this.txtIp.MaxLength = 32767;
            this.txtIp.Name = "txtIp";
            this.txtIp.PasswordChar = '\0';
            this.txtIp.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtIp.SelectedText = "";
            this.txtIp.SelectionLength = 0;
            this.txtIp.SelectionStart = 0;
            this.txtIp.ShortcutsEnabled = true;
            this.txtIp.Size = new System.Drawing.Size(115, 23);
            this.txtIp.TabIndex = 28;
            this.txtIp.Text = "localhost";
            this.txtIp.UseSelectable = true;
            this.txtIp.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtIp.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel10
            // 
            this.metroLabel10.AutoSize = true;
            this.metroLabel10.Location = new System.Drawing.Point(15, 12);
            this.metroLabel10.Name = "metroLabel10";
            this.metroLabel10.Size = new System.Drawing.Size(79, 19);
            this.metroLabel10.TabIndex = 27;
            this.metroLabel10.Text = "Endereço IP";
            // 
            // txtPorta
            // 
            // 
            // 
            // 
            this.txtPorta.CustomButton.Image = null;
            this.txtPorta.CustomButton.Location = new System.Drawing.Point(33, 1);
            this.txtPorta.CustomButton.Name = "";
            this.txtPorta.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtPorta.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtPorta.CustomButton.TabIndex = 1;
            this.txtPorta.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtPorta.CustomButton.UseSelectable = true;
            this.txtPorta.CustomButton.Visible = false;
            this.txtPorta.Lines = new string[] {
        "3306"};
            this.txtPorta.Location = new System.Drawing.Point(136, 34);
            this.txtPorta.MaxLength = 32767;
            this.txtPorta.Name = "txtPorta";
            this.txtPorta.PasswordChar = '\0';
            this.txtPorta.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtPorta.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtPorta.SelectedText = "";
            this.txtPorta.SelectionLength = 0;
            this.txtPorta.SelectionStart = 0;
            this.txtPorta.ShortcutsEnabled = true;
            this.txtPorta.Size = new System.Drawing.Size(55, 23);
            this.txtPorta.TabIndex = 30;
            this.txtPorta.Text = "3306";
            this.txtPorta.UseSelectable = true;
            this.txtPorta.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtPorta.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel13
            // 
            this.metroLabel13.AutoSize = true;
            this.metroLabel13.Location = new System.Drawing.Point(95, 64);
            this.metroLabel13.Name = "metroLabel13";
            this.metroLabel13.Size = new System.Drawing.Size(44, 19);
            this.metroLabel13.TabIndex = 33;
            this.metroLabel13.Text = "Senha";
            // 
            // metroLabel12
            // 
            this.metroLabel12.AutoSize = true;
            this.metroLabel12.Location = new System.Drawing.Point(14, 64);
            this.metroLabel12.Name = "metroLabel12";
            this.metroLabel12.Size = new System.Drawing.Size(53, 19);
            this.metroLabel12.TabIndex = 31;
            this.metroLabel12.Text = "Usuário";
            // 
            // metroLabel11
            // 
            this.metroLabel11.AutoSize = true;
            this.metroLabel11.Location = new System.Drawing.Point(136, 12);
            this.metroLabel11.Name = "metroLabel11";
            this.metroLabel11.Size = new System.Drawing.Size(41, 19);
            this.metroLabel11.TabIndex = 29;
            this.metroLabel11.Text = "Porta";
            // 
            // txtUsuario
            // 
            // 
            // 
            // 
            this.txtUsuario.CustomButton.Image = null;
            this.txtUsuario.CustomButton.Location = new System.Drawing.Point(53, 1);
            this.txtUsuario.CustomButton.Name = "";
            this.txtUsuario.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtUsuario.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtUsuario.CustomButton.TabIndex = 1;
            this.txtUsuario.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtUsuario.CustomButton.UseSelectable = true;
            this.txtUsuario.CustomButton.Visible = false;
            this.txtUsuario.Lines = new string[] {
        "root"};
            this.txtUsuario.Location = new System.Drawing.Point(15, 87);
            this.txtUsuario.MaxLength = 32767;
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.PasswordChar = '\0';
            this.txtUsuario.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtUsuario.SelectedText = "";
            this.txtUsuario.SelectionLength = 0;
            this.txtUsuario.SelectionStart = 0;
            this.txtUsuario.ShortcutsEnabled = true;
            this.txtUsuario.Size = new System.Drawing.Size(75, 23);
            this.txtUsuario.TabIndex = 32;
            this.txtUsuario.Text = "root";
            this.txtUsuario.UseSelectable = true;
            this.txtUsuario.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtUsuario.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // btnRestore
            // 
            this.btnRestore.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnRestore.BackgroundImage = global::MetroSmartBackup.Properties.Resources.databaserestore2_25;
            this.btnRestore.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRestore.Enabled = false;
            this.btnRestore.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnRestore.FontWeight = MetroFramework.MetroButtonWeight.Light;
            this.btnRestore.Location = new System.Drawing.Point(539, 39);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(75, 55);
            this.btnRestore.Style = MetroFramework.MetroColorStyle.Lime;
            this.btnRestore.TabIndex = 44;
            this.btnRestore.Text = "Restaurar";
            this.btnRestore.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRestore.Theme = MetroFramework.MetroThemeStyle.Light;
            this.btnRestore.UseSelectable = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // frmRestore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 389);
            this.Controls.Add(this.metroPanel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picLogo);
            this.Controls.Add(this.btnRestore);
            this.MaximizeBox = false;
            this.Name = "frmRestore";
            this.Load += new System.EventHandler(this.frmRestore_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.metroPanel1.ResumeLayout(false);
            this.metroPanel1.PerformLayout();
            this.metroPanel3.ResumeLayout(false);
            this.metroPanel3.PerformLayout();
            this.metroPanel2.ResumeLayout(false);
            this.metroPanel2.PerformLayout();
            this.pnlConexao.ResumeLayout(false);
            this.pnlConexao.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label label1;
        private MetroFramework.Controls.MetroPanel metroPanel1;
        private MetroFramework.Controls.MetroButton btnConectar;
        private MetroFramework.Controls.MetroTextBox txtSenha;
        private MetroFramework.Controls.MetroLabel metroLabel10;
        private MetroFramework.Controls.MetroLabel metroLabel13;
        private MetroFramework.Controls.MetroTextBox txtIp;
        private MetroFramework.Controls.MetroTextBox txtUsuario;
        private MetroFramework.Controls.MetroLabel metroLabel11;
        private MetroFramework.Controls.MetroLabel metroLabel12;
        private MetroFramework.Controls.MetroTextBox txtPorta;
        private MetroFramework.Controls.MetroPanel pnlConexao;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroPanel metroPanel2;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroComboBox cmbDatabases;
        private MetroFramework.Controls.MetroPanel metroPanel3;
        private MetroFramework.Controls.MetroTextBox txtCaminhoArquivo;
        private MetroFramework.Controls.MetroButton btnLocalizarArquivo;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroLabel metroLabel5;
        private MetroFramework.Controls.MetroButton btnRestore;
        private System.Windows.Forms.Label lblBytesProcessados;
        private System.Windows.Forms.Label lblPorcentagem;
        private MetroFramework.Controls.MetroProgressBar progressRestore;
    }
}