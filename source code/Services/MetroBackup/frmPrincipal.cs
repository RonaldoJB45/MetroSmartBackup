using MetroBackup.ApplicationService;
using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MetroBackup
{
    public partial class frmPrincipal : MetroForm
    {
        private readonly IConfiguracaoAppService _configuracaoAppService;

        private Guid? ConfiguracaoSelecionadaId = null;

        public frmPrincipal(IConfiguracaoAppService configuracaoAppService)
        {
            InitializeComponent();
            _configuracaoAppService = configuracaoAppService;
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            cmbCompactador.SelectedIndex = 0;
            mPnlPrincipal.Enabled = false;

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

            var configuracoesDto = _configuracaoAppService.ObterTodos();

            foreach (var configuracao in configuracoesDto)
            {
                dgLista.Rows.Add(configuracao.Id.ToString(), configuracao.Descricao);
            }
        }

        #region Botoes Principais

        private void btnNovo_Click(object sender, EventArgs e)
        {
            mPnlPrincipal.Enabled = true;
            dgLista.Enabled = false;

            LimpaCampos();

            HabilitaBotoesPrincipais(Salvar: true, Cancelar: true);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDescricao.Text))
            {
                MetroMessageBox.Show(this, "Selecione uma configuração para editar!");
                return;
            }

            mPnlPrincipal.Enabled = true;
            dgLista.Enabled = false;

            HabilitaBotoesPrincipais(Salvar: true, Cancelar: true);
        }

        private void PreencherBancos(ConfiguracaoDto configuracaoDto)
        {
            var bancos = RetornaListaBancos();

            foreach (var nomeBanco in bancos)
            {
                var servidorDto = new ServidorDto
                {
                    IpBanco = txtIp.Text,
                    NomeBanco = nomeBanco,
                    PortaBanco = txtPorta.Text,
                    UsuarioBanco = txtUsuario.Text,
                    SenhaBanco = txtSenha.Text
                };

                configuracaoDto.Servidores.Add(servidorDto);
            }
        }

        private void PreencherConfiguracaoDto(ConfiguracaoDto configuracaoDto)
        {
            PreencherBancos(configuracaoDto);

            configuracaoDto.Id = ConfiguracaoSelecionadaId;
            configuracaoDto.Descricao = txtDescricao.Text;
            configuracaoDto.DiasDaSemana = RetornaDiasDaSemana();
            configuracaoDto.UsarIntervaloHoras = chkIntervalo.Checked;
            configuracaoDto.ValorIntervaloHoras = !string.IsNullOrEmpty(txtIntervalo.Text) ? Convert.ToInt32(txtIntervalo.Text) : 0;
            configuracaoDto.UsarHoraFixa = chkHoraFixa.Checked;
            configuracaoDto.ValorHoraFixa = Convert.ToString(dtpHoraFixa.Value);
            configuracaoDto.UsarConfigApagar = chkApagar.Checked;
            configuracaoDto.QtdeDiasParaApagar = !string.IsNullOrEmpty(txtDiasApagar.Text) ? Convert.ToInt32(txtDiasApagar.Text) : 0;
            configuracaoDto.Compactar = chkCompactar.Checked;
            configuracaoDto.Compactador = cmbCompactador.Text;
            configuracaoDto.Destinos = RetornaDestinos();
            configuracaoDto.MostrarJanelaNotificacao = chkMostrarNotificacao.Checked;
            configuracaoDto.UtilizarHostFtp = chkUtilizarHostFtp.Checked;
            configuracaoDto.HostFtp = txtHostFtp.Text;
            configuracaoDto.UserFtp = txtUserFtp.Text;
            configuracaoDto.PasswordFtp = txtPasswordFtp.Text;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            ConfiguracaoDto configuracaoDto = new ConfiguracaoDto();
            PreencherConfiguracaoDto(configuracaoDto);

            if (ConfiguracaoSelecionadaId.HasValue)
                _configuracaoAppService.Alterar(configuracaoDto);
            else
                _configuracaoAppService.Adicionar(configuracaoDto);

            PreencherListaConfiguracoes();

            LimpaCampos();
            mPnlPrincipal.Enabled = false;
            dgLista.Enabled = true;

            HabilitaBotoesPrincipais(Novo: true);
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (ConfiguracaoSelecionadaId.HasValue)
            {
                if (MetroMessageBox.Show(this, "Você tem certeza que deseja excluir essa configuração?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    _configuracaoAppService.Remover(ConfiguracaoSelecionadaId.Value);

                    HabilitaBotoesPrincipais(Novo: true);

                    mPnlPrincipal.Enabled = false;
                    dgLista.Enabled = true;

                    LimpaCampos();
                }
            }

            PreencherListaConfiguracoes();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpaCampos();
            mPnlPrincipal.Enabled = false;
            dgLista.Enabled = true;

            HabilitaBotoesPrincipais(Novo: true);
        }

        #endregion

        #region Metodos
        private string[] RetornaListaBancos()
        {
            string[] lista;

            List<string> myCollection = new List<string>();

            foreach (Control c in mPnlDataBase.Controls)
                if (c is CheckBox)
                    if (((CheckBox)c).Checked)
                        myCollection.Add(((CheckBox)c).Text);

            lista = myCollection.ToArray();

            return lista;
        }
        private string[] RetornaDiasDaSemana()
        {
            string[] dias;

            List<string> myCollection = new List<string>();

            foreach (Control c in mPnlDiasSemana.Controls)
                if (c is CheckBox)
                    if (((CheckBox)c).Checked)
                        myCollection.Add(((CheckBox)c).Text);

            dias = myCollection.ToArray();

            return dias;
        }
        private string[] RetornaDestinos()
        {
            string[] destinos;

            List<string> myCollection = new List<string>();

            foreach (DataGridViewRow item in dgDestinos.Rows)
            {
                myCollection.Add(item.Cells[0].Value.ToString());
            }

            destinos = myCollection.ToArray();

            return destinos;
        }

        private void LimpaCampos()
        {
            foreach (Control c in mPnlDiasSemana.Controls)
                if (c is CheckBox)
                    ((CheckBox)c).Checked = false;

            while (mPnlDataBase.Controls.Count > 0)
            {
                mPnlDataBase.Controls.RemoveAt(0);
            }

            foreach (Control c in mPnlHorarios.Controls)
                if (c is CheckBox)
                    ((CheckBox)c).Checked = false;

            foreach (Control c in mPnlDescricaoOutrasOpcoes.Controls)
                if (c is CheckBox)
                    ((CheckBox)c).Checked = false;

            ConfiguracaoSelecionadaId = null;
            txtIp.Text = "localhost";
            txtPorta.Text = "3306";
            txtUsuario.Text = "root";
            txtSenha.Text = "";
            txtIntervalo.Clear();
            dtpHoraFixa.Value = DateTime.Now;
            txtDescricao.Clear();
            cmbCompactador.SelectedIndex = 0;
            txtDiasApagar.Clear();
            dgDestinos.Rows.Clear();
            chkUtilizarHostFtp.Checked = false;
            txtHostFtp.Clear();
            txtUserFtp.Clear();
            txtPasswordFtp.Clear();
        }

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
            LimpaCampos();
            mPnlPrincipal.Enabled = false;

            if (dgLista.CurrentCell != null)
            {
                var id = dgLista.SelectedCells[0].Value?.ToString();

                var configuracaoDto = _configuracaoAppService.ObterPorId(Guid.Parse(id));

                ConfiguracaoSelecionadaId = configuracaoDto.Id;

                var servidor = configuracaoDto.Servidores.FirstOrDefault();

                if (servidor != null)
                {
                    txtIp.Text = servidor.IpBanco;
                    txtPorta.Text = servidor.PortaBanco;
                    txtUsuario.Text = servidor.UsuarioBanco;
                    txtSenha.Text = servidor.SenhaBanco;
                }

                txtDescricao.Text = configuracaoDto.Descricao;
                foreach (string item in configuracaoDto.DiasDaSemana)
                {
                    foreach (Control c in mPnlDiasSemana.Controls)
                        if (c is CheckBox)
                            if (((CheckBox)c).Text == item)
                                ((CheckBox)c).Checked = true;
                }

                chkIntervalo.Checked = configuracaoDto.UsarIntervaloHoras;
                txtIntervalo.Text = configuracaoDto.ValorIntervaloHoras.ToString();
                chkHoraFixa.Checked = configuracaoDto.UsarHoraFixa;
                dtpHoraFixa.Value = DateTime.Parse(configuracaoDto.ValorHoraFixa);
                chkApagar.Checked = configuracaoDto.UsarConfigApagar;
                txtDiasApagar.Text = configuracaoDto.QtdeDiasParaApagar.ToString();
                chkCompactar.Checked = configuracaoDto.Compactar;
                cmbCompactador.SelectedItem = configuracaoDto.Compactador;

                int i = 0;
                foreach (string item in configuracaoDto.Destinos)
                {
                    dgDestinos.Rows.Add(item);
                    if (!Directory.Exists(item))
                    {
                        dgDestinos.Rows[i].DefaultCellStyle.BackColor = Color.LightCoral;
                        dgDestinos.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                    }

                    i++;
                }

                dgDestinos.ClearSelection();
                chkMostrarNotificacao.Checked = configuracaoDto.MostrarJanelaNotificacao;
                chkUtilizarHostFtp.Checked = configuracaoDto.UtilizarHostFtp;
                txtHostFtp.Text = configuracaoDto.HostFtp;
                txtUserFtp.Text = configuracaoDto.UserFtp;
                txtPasswordFtp.Text = configuracaoDto.PasswordFtp;

                HabilitaBotoesPrincipais(Novo: true, Editar: true, Excluir: true, Backup: true);

            }
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
