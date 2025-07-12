using MetroBackup.ApplicationService.BancoDados;
using MetroBackup.ApplicationService.Restauracoes;
using MetroBackup.Domain.Interfaces;
using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MetroSmartBackup
{
    public partial class frmRestore : MetroForm
    {
        private readonly IBancoDadosAppService _bancoDadosAppService;
        private readonly IRestoreAppService _restoreAppService;
        private readonly IProgressReporter _progressReporter;

        public frmRestore(
            IBancoDadosAppService bancoDadosAppService,
            IRestoreAppService restoreAppService,
            IProgressReporter progressReporter)
        {
            InitializeComponent();
            _bancoDadosAppService = bancoDadosAppService;
            _restoreAppService = restoreAppService;
            _progressReporter = progressReporter;
        }

        private void frmRestore_Load(object sender, EventArgs e)
        {
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
            string ip = txtIp.Text;
            string porta = txtPorta.Text;
            string usuario = txtUsuario.Text;
            string senha = txtSenha.Text;

            var bancos = _bancoDadosAppService.ObterTodos(new BancoDadosDto
            {
                Endereco = ip,
                Porta = porta,
                Usuario = usuario,
                Senha = senha
            });

            cmbDatabases.DataSource = bancos;
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (!Validacao())
                return;

            btnRestore.Enabled = false;
            progressRestore.Visible = true;
            lblPorcentagem.Text = "0%";

            string ip = txtIp.Text;
            string nomeBanco = cmbDatabases.Text;
            string porta = txtPorta.Text;
            string usuario = txtUsuario.Text;
            string senha = txtSenha.Text;
            string arquivo = txtCaminhoArquivo.Text;

            var restoreDto = new RestoreDto
            {
                IpBanco = ip,
                NomeBanco = nomeBanco,
                PortaBanco = porta,
                UsuarioBanco = usuario,
                SenhaBanco = senha,
                Arquivo = arquivo
            };

            _progressReporter.ProgressChanged += (progresso, mensagem) =>
            {
                AtualizarProgresso((int)progresso, mensagem);
            };

            Restaurar(restoreDto);
        }

        public void AtualizarProgresso(int progresso, string mensagem)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AtualizarProgresso(progresso, mensagem)));
                return;
            }

            progressRestore.Value = progresso;
            lblPorcentagem.Text = progresso.ToString() + "%";
            lblMsg.Text = mensagem;
        }

        public void Restaurar(RestoreDto restoreDto)
        {
            Task.Run(() =>
            {
                _restoreAppService.Restaurar(restoreDto);
            });
        }

        private bool Validacao()
        {
            bool sucesso = true;

            if (string.IsNullOrEmpty(cmbDatabases.Text))
            {
                MetroMessageBox.Show(this, "Favor selecionar o banco de dados.");
                cmbDatabases.Focus();
                return false;
            }
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
            if (string.IsNullOrEmpty(txtCaminhoArquivo.Text))
            {
                MetroMessageBox.Show(this, "O campo CAMINHO não pode ficar em branco");
                txtCaminhoArquivo.Focus();
                return false;
            }

            return sucesso;
        }
    }
}
