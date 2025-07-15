using MetroBackup.ApplicationService.Configuracoes;
using MetroBackup.ApplicationService.Restauracoes;
using MetroBackup.ApplicationService.BancoDados;
using MetroBackup.ApplicationService.Backup;
using MetroBackup.ApplicationService.Backup.Dtos;
using MetroBackup.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using MetroFramework.Forms;
using System.Windows.Forms;
using MetroSmartBackup;
using System.Globalization;
using MetroFramework;
using System.Drawing;
using System.Linq;
using System.IO;
using System;

namespace MetroBackup
{
    public partial class frmPrincipal : MetroForm
    {
        private readonly IConfiguracaoAppService _configuracaoAppService;
        private readonly IBancoDadosAppService _bancoDadosAppService;
        private readonly IBackupAppService _backupAppService;
        private readonly IRestoreAppService _restoreAppService;
        private readonly IProgressReporter _progressReporter;
        private readonly CultureInfo cultureInfo = new CultureInfo("pt-BR");
        private Guid? ConfiguracaoSelecionadaId = null;
        private string diaAtual;
        private int intervalo = 30000;
        private Timer _timer;
        private readonly HashSet<Guid> _backupsEmExecucao = new HashSet<Guid>();

        private readonly Queue<(Guid ConfiguracaoId, bool MostrarNotificacao)> _filaBackups = new Queue<(Guid ConfiguracaoId, bool MostrarNotificacao)>();
        private bool _processandoFila = false;
        private readonly object _lockFila = new object();

        public frmPrincipal(
            IConfiguracaoAppService configuracaoAppService,
            IBancoDadosAppService bancoDadosAppService,
            IBackupAppService backupAppService,
            IRestoreAppService restoreAppService,
            IProgressReporter progressReporter)
        {
            InitializeComponent();
            _configuracaoAppService = configuracaoAppService;
            _bancoDadosAppService = bancoDadosAppService;
            _backupAppService = backupAppService;
            _restoreAppService = restoreAppService;
            _progressReporter = progressReporter;
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            cmbCompactador.SelectedIndex = 0;
            mPnlPrincipal.Enabled = false;

            RenderizarLogo();
            HabilitaBotoesPrincipais(Novo: true);
            PreencherListaConfiguracoes();
            IniciarTimer();
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

        #region Timer

        private void IniciarTimer()
        {
            _timer = new Timer();
            _timer.Interval = intervalo;
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            VerificarBackup();
        }
        private void VerificarBackup()
        {
            diaAtual = cultureInfo.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
            var configuracoesDto = _configuracaoAppService.ObterTodos();

            foreach (var configuracaoDto in configuracoesDto)
            {
                BackupIntervaloHoras(configuracaoDto);
                BackupHoraFixa(configuracaoDto);
            }
        }
        private void BackupHoraFixa(ConfiguracaoDto configuracaoDto)
        {
            var diaDaSemana = configuracaoDto.DiasDaSemana.Any(d => d.ToLower() == diaAtual.ToLower());

            if (diaDaSemana)
            {
                string valorHoraFixa = DateTime.Parse(configuracaoDto.ValorHoraFixa).ToString("HH:mm");
                string valorHoraAgora = DateTime.Now.ToString("HH:mm");

                if (valorHoraFixa == valorHoraAgora)
                {
                    EnfileirarBackup(configuracaoDto.Id.Value, configuracaoDto.MostrarJanelaNotificacao);
                }
            }
        }
        private void BackupIntervaloHoras(ConfiguracaoDto configuracaoDto)
        {
            if (configuracaoDto.UsarIntervaloHoras)
            {
                var ultimosBackupsDto = _backupAppService.ObterTodos();
                var ultimoBackupDto = ultimosBackupsDto.FirstOrDefault(u => u.ConfiguracaoId == configuracaoDto.Id);

                if (ultimoBackupDto == null)
                {
                    EnfileirarBackup(configuracaoDto.Id.Value, configuracaoDto.MostrarJanelaNotificacao);
                    return;
                }

                DateTime dataHoraAtual = DateTime.Now;
                DateTime dataHoraUltimoBackup = ultimoBackupDto.DataHora;
                TimeSpan intervaloPermitido = TimeSpan.FromHours(configuracaoDto.ValorIntervaloHoras);
                TimeSpan tempoDecorrido = dataHoraAtual - dataHoraUltimoBackup;
                bool ultrapassou = tempoDecorrido > intervaloPermitido;

                if (ultrapassou)
                    EnfileirarBackup(configuracaoDto.Id.Value, configuracaoDto.MostrarJanelaNotificacao);
            }
        }

        #endregion

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

        private void btnBackup_Click(object sender, EventArgs e)
        {
            if (ConfiguracaoSelecionadaId.HasValue)
                EnfileirarBackup(ConfiguracaoSelecionadaId.Value, chkMostrarNotificacao.Checked);
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            using (frmRestore frm = new frmRestore(
                _bancoDadosAppService,
                _restoreAppService,
                _progressReporter))
            {
                frm.ShowDialog();
            }
        }

        #endregion

        #region Metodos
        private string ObterNomeConfiguracao(Guid id)
        {
            var configuracao = _configuracaoAppService.ObterPorId(id);
            return configuracao?.Descricao ?? "Configuração desconhecida";
        }
        private void AtualizarFilaVisual()
        {
            if (lstFila.InvokeRequired)
            {
                lstFila.Invoke(new Action(AtualizarFilaVisual));
                return;
            }

            lstFila.Items.Clear();

            lock (_lockFila)
            {
                foreach (var item in _filaBackups)
                {
                    string nomeConfiguracao = ObterNomeConfiguracao(item.ConfiguracaoId);
                    lstFila.Items.Add($"[⏳] {nomeConfiguracao}");
                }
            }
        }
        private void AdicionarStatus(string mensagem)
        {
            if (lstLog.InvokeRequired)
            {
                lstLog.Invoke(new Action(() => lstLog.Items.Add(mensagem)));
            }
            else
            {
                lstLog.Items.Add(mensagem);
            }
        }
        private void EnfileirarBackup(Guid configuracaoId, bool mostrarNotificacao)
        {
            lock (_lockFila)
            {
                if (_filaBackups.Any(b => b.ConfiguracaoId == configuracaoId))
                    return;

                _filaBackups.Enqueue((configuracaoId, mostrarNotificacao));
                AtualizarFilaVisual();

                if (!_processandoFila)
                {
                    _processandoFila = true;
                    _ = ProcessarFilaAsync();
                }
            }
        }
        private async Task ProcessarFilaAsync()
        {
            while (true)
            {
                (Guid ConfiguracaoId, bool MostrarNotificacao) item;

                lock (_lockFila)
                {
                    if (_filaBackups.Count == 0)
                    {
                        _processandoFila = false;
                        return;
                    }

                    item = _filaBackups.Dequeue();
                    AtualizarFilaVisual();
                }

                await BackupAsync(item.ConfiguracaoId, item.MostrarNotificacao);
            }
        }
        private async Task BackupAsync(Guid configuracaoId, bool mostrarNotificacao = true)
        {
            if (_backupsEmExecucao.Contains(configuracaoId))
                return;

            _backupsEmExecucao.Add(configuracaoId);
            //btnBackup.Enabled = false;

            frmTelaAguardeProcessoProgressBar _telaProgressBar = null;

            string nomeConfiguracao = ObterNomeConfiguracao(configuracaoId);
            AdicionarStatus($"[✔] Backup iniciado para: {nomeConfiguracao}");

            try
            {
                if (mostrarNotificacao)
                {
                    _telaProgressBar = new frmTelaAguardeProcessoProgressBar();

                    _progressReporter.ProgressChanged += (progresso, mensagem) =>
                    {
                        _telaProgressBar.AtualizarProgresso((int)progresso, mensagem);
                    };

                    _telaProgressBar.Notify();

                    _telaProgressBar.Show();
                }

                await Task.Run(() =>
                {
                    _backupAppService.Executar(new BackupDto { ConfiguracaoId = configuracaoId });
                });

                AdicionarStatus($"[✔] Backup finalizado para: {nomeConfiguracao}");

                if (mostrarNotificacao && _telaProgressBar != null)
                {
                    if (_telaProgressBar.InvokeRequired)
                        _telaProgressBar.Invoke(new Action(() => _telaProgressBar.Close()));
                    else
                        _telaProgressBar.Close();
                }
            }
            catch (Exception ex)
            {
                AdicionarStatus($"[X] Falha no backup: {nomeConfiguracao} - {ex.Message}");

                if (InvokeRequired)
                    Invoke(new Action(() => MetroMessageBox.Show(this, ex.Message)));
                else
                    MetroMessageBox.Show(this, ex.Message);
            }
            finally
            {
                //if (InvokeRequired)
                //    Invoke(new Action(() => btnBackup.Enabled = true));
                //else
                //    btnBackup.Enabled = true;

                _backupsEmExecucao.Remove(configuracaoId);
            }
        }

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

        private void CarregarBancos(
            string ip,
            string porta,
            string usuario,
            string senha)
        {
            IEnumerable<string> bancos = _bancoDadosAppService.ObterTodos(new BancoDadosDto
            {
                Endereco = ip,
                Porta = porta,
                Usuario = usuario,
                Senha = senha
            });


            MetroFramework.Controls.MetroCheckBox chk;

            int i = 0;

            foreach (var banco in bancos)
            {
                i++;
                chk = new MetroFramework.Controls.MetroCheckBox();
                chk.AutoSize = true;
                chk.Location = new Point(10, (((i + 1) * 20)) - 30);
                chk.Name = "chk" + banco;
                chk.Size = new Size(113, 15);
                chk.TabIndex = 2 + i;
                chk.Text = banco;
                chk.UseVisualStyleBackColor = true;
                chk.Style = MetroColorStyle.Orange;
                mPnlDataBase.Controls.Add(chk);
            }
        }

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

                var configuracaoServidor = configuracaoDto.Servidores.FirstOrDefault();

                if (configuracaoServidor != null)
                {
                    string ip = configuracaoServidor.IpBanco;
                    string porta = configuracaoServidor.PortaBanco;
                    string usuario = configuracaoServidor.UsuarioBanco;
                    string senha = configuracaoServidor.SenhaBanco;

                    var dto = new BancoDadosDto
                    {
                        Endereco = ip,
                        Porta = porta,
                        Usuario = usuario,
                        Senha = senha
                    };

                    _bancoDadosAppService.TestarConexao(dto);

                    if (string.IsNullOrWhiteSpace(dto.Erro))
                    {
                        txtIp.Text = ip;
                        txtPorta.Text = porta;
                        txtUsuario.Text = usuario;
                        txtSenha.Text = senha;

                        CarregarBancos(ip, porta, usuario, senha);

                        foreach (var servidor in configuracaoDto.Servidores)
                        {
                            string banco = servidor.NomeBanco;

                            foreach (Control c in mPnlDataBase.Controls)
                                if (c is CheckBox)
                                    if (((CheckBox)c).Text == banco)
                                        ((CheckBox)c).Checked = true;
                        }
                    }
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
            string ip = txtIp.Text;
            string porta = txtPorta.Text;
            string usuario = txtUsuario.Text;
            string senha = txtSenha.Text;

            var dto = new BancoDadosDto
            {
                Endereco = ip,
                Porta = porta,
                Usuario = usuario,
                Senha = senha
            };

            _bancoDadosAppService.TestarConexao(dto);

            if (!string.IsNullOrWhiteSpace(dto.Erro))
            {
                MetroMessageBox.Show(this, "Conexão com o banco de dados falhou! \r\n" + dto.Erro);
                return;
            }

            CarregarBancos(ip, porta, usuario, senha);
        }

        private void frmPrincipal_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void notifySmartBackup_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void chkUtilizarHostFtp_CheckedChanged(object sender, EventArgs e)
        {
            grpDadosFtp.Enabled = chkUtilizarHostFtp.Checked;
        }

        private void txtSenha_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                btnConectar.PerformClick();
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

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_backupsEmExecucao.Any())
            {
                MetroMessageBox.Show(this, "Há backups em andamento. Aguarde a finalização antes de sair.", "Backup em andamento", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }
    }
}