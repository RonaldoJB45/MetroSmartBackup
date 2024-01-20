using MetroFramework.Forms;
using System;
using System.IO;
using System.Windows.Forms;

namespace MetroBackup
{
    public partial class frmPrincipal : MetroForm
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            RenderizarLogo();
            HabilitaBotoesPrincipais(Novo: true);
            ObterListaConfiguracoes();
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

        private void ObterListaConfiguracoes()
        {

        }

        private void btnSelecionarDestino_Click(object sender, EventArgs e)
        {
        }

        private void txtDescricao_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void txtPorta_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void txtIntervalo_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void txtDiasApagar_KeyPress(object sender, KeyPressEventArgs e)
        {
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
    }
}
