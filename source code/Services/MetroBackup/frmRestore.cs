using MetroFramework;
using MetroFramework.Forms;
using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace MetroSmartBackup
{
    public partial class frmRestore : MetroForm
    {
        BackgroundWorker bw = new BackgroundWorker();
        string constring = "";

        long mCurrentBytes = 0;
        long mTotalBytes = 0;

        MySqlBackup mb = null;

        public frmRestore()
        {
            InitializeComponent();
        }

        private void frmRestore_Load(object sender, EventArgs e)
        {
            DirectoryInfo diretorio = new DirectoryInfo(Application.StartupPath + "//images");

            if (!diretorio.Exists)
                diretorio.Create();

            string path = Application.StartupPath + "//images//logo.png";

            if (new FileInfo(path).Exists)
                picLogo.ImageLocation = path;

            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int _porcentagem = e.ProgressPercentage;

            if (_porcentagem > 0)
            {
                progressRestore.Value = _porcentagem;
                lblPorcentagem.Text = _porcentagem + "%";
                lblBytesProcessados.Text = mCurrentBytes + " bytes processados de um total de " + mTotalBytes + " bytes";
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            string file = txtCaminhoArquivo.Text;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(constring))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        mb = new MySqlBackup(cmd);

                        cmd.Connection = conn;

                        conn.Open();

                        mb.ImportProgressChanged += new MySqlBackup.importProgressChange(mb_ImportProgressChange);

                        mb.ImportFromFile(file);

                        conn.Close();

                        mb.Dispose();
                    }
                }
            }
            catch (Exception err)
            {
                MetroMessageBox.Show(this, err.Message);
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (mb.LastError == null)
            {
                MetroMessageBox.Show(this, "Restauração completada com sucesso!", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                LogErro("LOGERRO.TXT", "Restauração", mb.LastError.ToString());
                MetroMessageBox.Show(this, "Completado com erro(s). Consulte o log de erro para mais detalhes.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            btnRestore.Enabled = true;
        }

        public void LogErro(string _arquivo, string _modulo, string _logMensagem)
        {
            using (StreamWriter w = File.AppendText(_arquivo))
            {
                w.WriteLine(" ");
                w.WriteLine(DateTime.Now.ToString("dd/MM/yy-HH:mm") + ";" + _modulo + ";ERRO:" + _logMensagem + ";");
                w.Flush();
                w.Close();
            }
        }

        private void btnLocalizarArquivo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "SQL Files (*.sql)|*.sql";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtCaminhoArquivo.Text = ofd.FileName;
                }
            }
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            DataTable dtBancos = new DataTable();
            dtBancos = new DataTable();
            dtBancos.Columns.Add("schema_name");

            Conexao conexao = new Conexao(txtIp.Text, txtPorta.Text, "information_schema", txtUsuario.Text, txtSenha.Text);
            dtBancos = conexao.ConectarBancos(dtBancos);

            if (dtBancos.Rows.Count > 0)
                btnRestore.Enabled = true;

            string strCnx = $"server={txtIp.Text};port={txtPorta.Text};uid={txtUsuario.Text};pwd={txtSenha.Text}";

            using (MySqlConnection conn = new MySqlConnection(strCnx))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;

                    conn.Open();
                    cmd.CommandText = "SET GLOBAL max_allowed_packet=1024*1024*1024;";
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            cmbDatabases.DataSource = dtBancos;
            cmbDatabases.DisplayMember = "schema_name";
        }

        private bool Validacao()
        {
            bool sucesso = true;

            if (string.IsNullOrEmpty(txtIp.Text))
            {
                MetroMessageBox.Show(this, "O campo IP não pode ficar em branco.");
                txtIp.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtPorta.Text))
            {
                MetroMessageBox.Show(this, "O campo PORTA não pode ficar em branco.");
                txtPorta.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtUsuario.Text))
            {
                MetroMessageBox.Show(this, "O campo USUÁRIO não pode ficar em branco.");
                txtUsuario.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtSenha.Text))
            {
                MetroMessageBox.Show(this, "O campo SENHA não pode ficar em branco.");
                txtSenha.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(cmbDatabases.Text))
            {
                MetroMessageBox.Show(this, "Favor selecionar o banco de dados.");
                cmbDatabases.Focus();
                return false;
            }

            return sucesso;
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (!Validacao())
                return;

            btnRestore.Enabled = false;

            constring = $"server={txtIp.Text};user={txtUsuario.Text};pwd={txtSenha.Text};database={cmbDatabases.Text};charset=utf8;convertzerodatetime=true; default command timeout=0;";
            progressRestore.Visible = true;
            lblPorcentagem.Text = "0%";

            if (!bw.IsBusy)
                bw.RunWorkerAsync();
        }

        private void mb_ImportProgressChange(object sender, ImportProgressArgs e)
        {
            mCurrentBytes = e.CurrentBytes;
            mTotalBytes = e.TotalBytes;
            bw.ReportProgress(e.PercentageCompleted);
        }
    }
}
