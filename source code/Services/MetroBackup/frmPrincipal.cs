using MetroBackup.ApplicationService;
using MetroFramework;
using MetroFramework.Forms;
using System;
using System.IO;
using System.Windows.Forms;

namespace MetroBackup
{
    public partial class frmPrincipal : MetroForm
    {
        private readonly IConfiguracaoAppService _configuracaoAppService;

        public frmPrincipal(IConfiguracaoAppService configuracaoAppService)
        {
            InitializeComponent();
            _configuracaoAppService = configuracaoAppService;
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            RenderizarLogo();
            HabilitaBotoesPrincipais(Novo: true);
            PreencherListaConfiguracoes();
        }

        private void RenderizarLogo()
        {
            string path = Application.StartupPath + "//images//logo.png";

            if (new FileInfo(path).Exists)
                picLogo.ImageLocation = path;
        }

        public void HabilitaBotoesPrincipais(
            bool Novo = false,
            bool Editar = false,
            bool Salvar = false,
            bool Excluir = false,
            bool Cancelar = false,
            bool Backup = false)
        {
            btnNovo.Enabled = Novo;
            btnEditar.Enabled = Editar;
            btnSalvar.Enabled = Salvar;
            btnExcluir.Enabled = Excluir;
            btnCancelar.Enabled = Cancelar;
            btnBackup.Enabled = Backup;
        }

        private void PreencherListaConfiguracoes()
        {
            dgLista.Rows.Clear();

            var configuracoes = _configuracaoAppService.ObterTodos();

            foreach (var configuracao in configuracoes)
                dgLista.Rows.Add(configuracao.Descricao);
        }

        #region Botoes Principais

        private void btnNovo_Click(object sender, EventArgs e)
        {
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
        }

        #endregion

        #region Metodos


        #endregion

        #region BancoDeDados

        #endregion

        #region Validacao

        #endregion

        private void btnSelecionarDestino_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    foreach (DataGridViewRow item in dgDestinos.Rows)
                    {
                        if (fbd.SelectedPath == item.Cells[0].Value.ToString())
                        {
                            MetroMessageBox.Show(this, "Já foi adicionado esse destino para backup. Favor selecionar outro destino!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                    dgDestinos.Rows.Add(fbd.SelectedPath);
                }
            }
        }

        private void dgLista_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
        }

        private void dgDestinos_DoubleClick(object sender, EventArgs e)
        {
        }
        private void btnConectar_Click(object sender, EventArgs e)
        {
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
        }

        private void frmPrincipal_Resize(object sender, EventArgs e)
        {
        }

        private void notifySmartBackup_DoubleClick(object sender, EventArgs e)
        {
        }

        private void chkUtilizarHostFtp_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void txtSenha_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
        }


        private void txtDescricao_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtPorta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                e.Handled = true;
        }

        private void txtIntervalo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                e.Handled = true;
        }

        private void txtDiasApagar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                e.Handled = true;
        }
    }
}
