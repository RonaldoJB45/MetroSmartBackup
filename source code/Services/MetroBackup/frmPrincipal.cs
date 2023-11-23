using Ionic.Zip;
using MetroFramework;
using MetroFramework.Forms;
using MetroSmartBackup;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace MetroBackup
{
    public partial class frmPrincipal : MetroForm
    {
        MetroFramework.Controls.MetroCheckBox chk;
        BackgroundWorker mAsyncWorkerWaitProcess = new BackgroundWorker();
        BackgroundWorker mAsyncWorker = new BackgroundWorker();
        BackgroundWorker mAsyncEnviarFtp = new BackgroundWorker();
        private frmTelaAguardeProcesso _progresso;
        private frmTelaAguardeProcessoProgressBar _telaProgressBar;

        Queue<Propriedades> mFilaBackup = new Queue<Propriedades>();
        Queue<ConfigFtp> mFilaEnviarFtp = new Queue<ConfigFtp>();

        DataTable dtBancos;
        List<Propriedades> MemoryProprierties;
        Propriedades mItem;
        string mValorMSG = "";

        DateTime dtUltimoBackup;

        BackgroundWorker worker = null;

        public frmPrincipal()
        {
            InitializeComponent();

            mAsyncWorker.WorkerReportsProgress = true;
            mAsyncWorker.ProgressChanged += new ProgressChangedEventHandler(mAsyncWorker_ProgressChanged);
            mAsyncWorker.DoWork += new DoWorkEventHandler(mAsyncWorker_DoWork);
            mAsyncWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mAsyncWorker_RunWorkerCompleted);
            
            mAsyncEnviarFtp.WorkerReportsProgress = true;
            mAsyncEnviarFtp.DoWork += new DoWorkEventHandler(mAsyncEnviarFtp_DoWork);
            mAsyncEnviarFtp.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mAsyncEnviarFtp_RunWorkerCompleted);
            mAsyncEnviarFtp.ProgressChanged += new ProgressChangedEventHandler(mAsyncEnviarFtp_ProgressChanged);

            mAsyncWorkerWaitProcess.DoWork += new DoWorkEventHandler(mAsyncWorkerWaitProcess_DoWork);
            mAsyncWorkerWaitProcess.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mAsyncWorkerWaitProcess_RunWorkerCompleted);
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            DirectoryInfo diretorio = new DirectoryInfo(Application.StartupPath + "//images");

            if (!diretorio.Exists)
                diretorio.Create();

            string path = Application.StartupPath + "//images//logo.png";
           
            if (new FileInfo(path).Exists)
                picLogo.ImageLocation = path;

            cmbCompactador.SelectedIndex = 0;
            mPnlPrincipal.Enabled = false;
            HabilitaBotoesPrincipais(Novo: true);

            MemoryProprierties = new List<Propriedades>();
            ObterListaConfiguracoes();

            dtUltimoBackup = DateTime.Parse("19:47:00");

            System.Windows.Forms.Timer timer2 = new System.Windows.Forms.Timer();
            timer2.Interval = 60000;
            timer2.Tick += new EventHandler(Timer2_Tick);
            timer2.Start();
        }

        public void HabilitaBotoesPrincipais(bool Novo = false, bool Editar = false, bool Salvar = false, bool Excluir = false, bool Cancelar = false, bool Backup = false)
        {
            btnNovo.Enabled = Novo;
            btnEditar.Enabled = Editar;
            btnSalvar.Enabled = Salvar;
            btnExcluir.Enabled = Excluir;
            btnCancelar.Enabled = Cancelar;
            btnBackup.Enabled = Backup;
        }

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

        private void txtDescricao_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtSenha_KeyDown(object sender, KeyEventArgs e)
        {

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
        private void dgLista_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            LimpaCampos();

            mPnlPrincipal.Enabled = false;

            if (dgLista.CurrentCell != null)
            {
                Propriedades propriedades = new Propriedades();
                propriedades = MemoryProprierties.Where(r => r.Nome == dgLista.SelectedCells[0].Value.ToString()).FirstOrDefault();

                txtIp.Text = propriedades.IpBanco;
                txtPorta.Text = propriedades.PortaBanco;
                txtUsuario.Text = propriedades.UsuarioBanco;
                txtSenha.Text = propriedades.SenhaBanco;

                RetornaConexao();

                foreach (string item in propriedades.ListaBancos)
                {
                    foreach (Control c in mPnlDataBase.Controls)
                        if (c is CheckBox)
                            if (((CheckBox)c).Text == item)
                                ((CheckBox)c).Checked = true;
                }

                txtDescricao.Text = propriedades.Nome;

                foreach (string item in propriedades.DiasDaSemana)
                {
                    foreach (Control c in mPnlDiasSemana.Controls)
                        if (c is CheckBox)
                            if (((CheckBox)c).Text == item)
                                ((CheckBox)c).Checked = true;
                }

                chkIntervalo.Checked = propriedades.UsarIntervaloHoras;
                txtIntervalo.Text = propriedades.ValorIntervaloHoras.ToString();
                chkHoraFixa.Checked = propriedades.UsarHoraFixa;
                dtpHoraFixa.Value = DateTime.Parse(propriedades.ValorHoraFixa);
                chkApagar.Checked = propriedades.UsarConfigApagar;
                txtDiasApagar.Text = propriedades.QtdeDiasParaApagar.ToString();
                chkCompactar.Checked = propriedades.Compactar;
                cmbCompactador.SelectedItem = propriedades.Compactador;

                int i = 0;
                foreach (string item in propriedades.Destinos)
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
                chkMostrarNotificacao.Checked = propriedades.MostrarJanelaNotificacao;
                chkUtilizarHostFtp.Checked = propriedades.UtilizarHostFtp;
                txtHostFtp.Text = propriedades.HostFtp;
                txtUserFtp.Text = propriedades.UserFtp;
                txtPasswordFtp.Text = propriedades.PasswordFtp;

                HabilitaBotoesPrincipais(Novo: true, Editar: true, Excluir: true, Backup: true);
            }
        }

        private void dgDestinos_DoubleClick(object sender, EventArgs e)
        {
            if (dgDestinos.CurrentCell != null)
            {
                dgDestinos.Rows.Remove(dgDestinos.SelectedRows[0]);
            }
        }
        private void btnConectar_Click(object sender, EventArgs e)
        {
            RetornaConexao();
        }

        #region Botoes Principais

        private void btnNovo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDescricao.Text) && mPnlPrincipal.Enabled)
            {
                if (MetroMessageBox.Show(this, "Existe um item em edição. Deseja salvá-lo antes de continuar?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    btnSalvar.PerformClick();
                }
            }

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

            dgLista.Rows.RemoveAt(dgLista.SelectedCells[0].RowIndex);

            mPnlPrincipal.Enabled = true;
            dgLista.Enabled = false;

            HabilitaBotoesPrincipais(Salvar: true, Cancelar: true);
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Validacao())
                    return;

                List<Propriedades> ListaPropriedades = new List<Propriedades>();
                Propriedades propriedades = new Propriedades();

                string caminhoArquivo = Application.StartupPath + @"\config.json";

                if (!File.Exists(caminhoArquivo))
                {
                    File.Create(Application.StartupPath + @"\config.json").Close();
                }

                dynamic conteudo = JsonUtil.ObterConteudo(caminhoArquivo);

                if (conteudo != null)
                    ListaPropriedades = JsonConvert.DeserializeObject<List<Propriedades>>(Convert.ToString(conteudo));

                foreach (DataGridViewRow r in dgLista.Rows)
                {
                    if (r.Cells["nome"].Value.ToString() == txtDescricao.Text)
                    {
                        if (MetroMessageBox.Show(this, "Já existe uma configuração com essa descrição, deseja substituí-la!", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            foreach (Propriedades p in ListaPropriedades)
                            {
                                if (p.Nome == txtDescricao.Text)
                                {
                                    ListaPropriedades.Remove(p);
                                    break;
                                }
                            }
                        }
                    }
                }

                propriedades = new Propriedades();

                propriedades.Nome = txtDescricao.Text;
                propriedades.IpBanco = txtIp.Text;
                propriedades.PortaBanco = txtPorta.Text;
                propriedades.UsuarioBanco = txtUsuario.Text;
                propriedades.SenhaBanco = txtSenha.Text; //criptografa
                propriedades.ListaBancos = RetornaListaBancos();
                propriedades.DiasDaSemana = RetornaDiasDaSemana();
                propriedades.UsarIntervaloHoras = chkIntervalo.Checked;
                propriedades.ValorIntervaloHoras = !string.IsNullOrEmpty(txtIntervalo.Text) ? Convert.ToInt32(txtIntervalo.Text) : 0;
                propriedades.UsarHoraFixa = chkHoraFixa.Checked;
                propriedades.ValorHoraFixa = Convert.ToString(dtpHoraFixa.Value);
                propriedades.UsarConfigApagar = chkApagar.Checked;
                propriedades.QtdeDiasParaApagar = !string.IsNullOrEmpty(txtDiasApagar.Text) ? Convert.ToInt32(txtDiasApagar.Text) : 0;
                propriedades.Compactar = chkCompactar.Checked;
                propriedades.Compactador = cmbCompactador.Text;
                propriedades.Destinos = RetornaDestinos();
                propriedades.MostrarJanelaNotificacao = chkMostrarNotificacao.Checked;
                propriedades.UtilizarHostFtp = chkUtilizarHostFtp.Checked;
                propriedades.HostFtp = txtHostFtp.Text;
                propriedades.UserFtp = txtUserFtp.Text;
                propriedades.PasswordFtp = txtPasswordFtp.Text;

                foreach (Propriedades item in ListaPropriedades)
                {
                    if (propriedades.Nome == item.Nome)
                    {
                        ListaPropriedades.Remove(item);
                        break;
                    }
                }

                ListaPropriedades.Add(propriedades);

                JsonUtil.SalvarConteudo(caminhoArquivo, ListaPropriedades, true);

                ObterListaConfiguracoes();

                HabilitaBotoesPrincipais(Novo: true);

                mPnlPrincipal.Enabled = false;
                dgLista.Enabled = true;

                LimpaCampos();
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message);
                LogErro("LOGERRO.TXT", "btnSalvar_Click", ex.Message);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgLista.CurrentCell != null)
            {
                if (MetroMessageBox.Show(this, "Você tem certeza que deseja excluir essa configuração?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    int indexRow = dgLista.CurrentCell.RowIndex;

                    List<Propriedades> ListaPropriedades = new List<Propriedades>();

                    string caminhoArquivo = Application.StartupPath + @"\config.json";

                    if (!File.Exists(caminhoArquivo))
                    {
                        File.Create(Application.StartupPath + @"\config.json").Close();
                    }

                    dynamic conteudo = JsonUtil.ObterConteudo(caminhoArquivo);

                    if (conteudo != null)
                        ListaPropriedades = JsonConvert.DeserializeObject<List<Propriedades>>(Convert.ToString(conteudo));

                    ListaPropriedades.RemoveAt(indexRow);

                    JsonUtil.SalvarConteudo(caminhoArquivo, ListaPropriedades, true);

                    ObterListaConfiguracoes();

                    HabilitaBotoesPrincipais(Novo: true);

                    mPnlPrincipal.Enabled = false;
                    dgLista.Enabled = true;

                    LimpaCampos();
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpaCampos();
            mPnlPrincipal.Enabled = false;
            ObterListaConfiguracoes();
            dgLista.Enabled = true;

            HabilitaBotoesPrincipais(Novo: true);
        }

        #endregion

        #region Metodos

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

        private void ObterListaConfiguracoes()
        {
            string caminhoArquivo = Application.StartupPath + @"\config.json";

            try
            {
                MemoryProprierties = JsonUtil.Listar<List<Propriedades>>(caminhoArquivo);

                dgLista.Rows.Clear();

                foreach (Propriedades propriedades in MemoryProprierties)
                    dgLista.Rows.Add(propriedades.Nome);
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233088)
                    MetroMessageBox.Show(this, ex.Message, "Carácteres inválido nas propriedades.", MessageBoxButtons.OK, MessageBoxIcon.Question);
                else if (ex.HResult == -2147024894)
                    MetroMessageBox.Show(this, "Não foi criada as configurações ou não foi possível encontrar o arquivo de configuração.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Question);
                else
                    MetroMessageBox.Show(this, ex.Message, "ERRO!", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }

        private string[] RetornaListaBancos()
        {
            string[] lista = new string[] { };

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
            string[] dias = new string[] { };

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
            string[] destino = new string[] { };

            List<string> myCollection = new List<string>();

            foreach (DataGridViewRow item in dgDestinos.Rows)
            {
                myCollection.Add(item.Cells[0].Value.ToString());
            }

            destino = myCollection.ToArray();

            return destino;
        }

        #endregion

        #region BancoDeDados

        private void mAsyncWorkerWaitProcess_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Conexao conexao = new Conexao(txtIp.Text, txtPorta.Text, "information_schema", txtUsuario.Text, txtSenha.Text);
                dtBancos = conexao.ConectarBancos(dtBancos);
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message, "Não foi possível conectar no banco.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void mAsyncWorkerWaitProcess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                int i = 0;

                foreach (DataRow item in dtBancos.Rows)
                {
                    i++;
                    chk = new MetroFramework.Controls.MetroCheckBox();
                    chk.AutoSize = true;
                    chk.Location = new System.Drawing.Point(10, (((i + 1) * 20)) - 30);
                    chk.Name = "chk" + item[0].ToString();
                    chk.Size = new System.Drawing.Size(113, 15);
                    chk.TabIndex = 2 + i;
                    chk.Text = item[0].ToString();
                    chk.UseVisualStyleBackColor = true;
                    chk.Style = MetroFramework.MetroColorStyle.Orange;
                    mPnlDataBase.Controls.Add(chk);
                }
                _progresso.Close();
            }
            catch (Exception ex) { MetroMessageBox.Show(this, ex.Message); }
        }
        private void RetornaConexao()
        {
            mPnlDataBase.Controls.Clear();

            dtBancos = new DataTable();
            dtBancos.Columns.Add("schema_name");

            mAsyncWorkerWaitProcess.RunWorkerAsync();
            _progresso = new frmTelaAguardeProcesso();
            _progresso.lblMsg.Text = "Conectando...";
            _progresso.ShowDialog();
        }

        #endregion

        #region Validacao

        private bool VerificaBancoSelecionado()
        {
            bool retorno = true;
            int counter = 0;
            foreach (Control c in mPnlDataBase.Controls)
                if (c is CheckBox)
                    if (((CheckBox)c).Checked)
                        counter++;

            if (counter <= 0)
            {
                MetroMessageBox.Show(this, "Selecione os bancos de dados!");
                return false;
            }

            return retorno;
        }
        private bool VerificaDiasDaSemanaSelecionado()
        {
            bool retorno = true;
            int counter = 0;
            foreach (Control c in mPnlDiasSemana.Controls)
                if (c is CheckBox)
                    if (((CheckBox)c).Checked)
                        counter++;

            if (counter <= 0)
            {
                MetroMessageBox.Show(this, "Selecione os dias da semana!");
                return false;
            }

            return retorno;
        }
        private bool VerificaHorariosSelecionado()
        {
            bool retorno = true;
            int counter = 0;
            foreach (Control c in mPnlHorarios.Controls)
                if (c is CheckBox)
                    if (((CheckBox)c).Checked)
                        counter++;

            if (counter <= 0)
            {
                MetroMessageBox.Show(this, "Selecione os horarios!");
                return false;
            }
            else if (chkIntervalo.Checked && string.IsNullOrEmpty(txtIntervalo.Text))
            {
                MetroMessageBox.Show(this, "Defina o intervalo!");
                txtIntervalo.Focus();
                return false;
            }

            return retorno;
        }
        private bool Validacao()
        {
            bool retorno = true;
            if (!VerificaBancoSelecionado())
                return false;

            if (!VerificaDiasDaSemanaSelecionado())
                return false;

            if (!VerificaHorariosSelecionado())
                return false;

            if (string.IsNullOrEmpty(txtDescricao.Text))
            {
                MetroMessageBox.Show(this, "Defina a descrição da configuração!");
                txtDescricao.Focus();
                return false;
            }

            if (chkApagar.Checked && string.IsNullOrEmpty(txtDiasApagar.Text))
            {
                MetroMessageBox.Show(this, "Defina a quantidade de dias que os arquivos irão ficar guardados!");
                txtDiasApagar.Focus();
                return false;
            }

            if (dgDestinos.Rows.Count < 1)
            {
                MetroMessageBox.Show(this, "Selecione os destinos dos arquivos de backup!");
                btnSelecionarDestino.PerformClick();
                return false;
            }
            return retorno;
        }

        #endregion

        private void Timer2_Tick(object Sender, EventArgs e)
        {
            try
            {
                if (MemoryProprierties != null)
                {
                    CultureInfo cultureInfo = new CultureInfo("pt-BR");
                    TimeSpan horaAtual = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));

                    string dia = cultureInfo.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);

                    foreach (Propriedades item in MemoryProprierties)
                    {
                        if (item.UsarIntervaloHoras)
                        {
                            mItem = new Propriedades();

                            TimeSpan intervalo = TimeSpan.FromHours(Convert.ToInt32(item.ValorIntervaloHoras));
                            TimeSpan ultimoBackup = TimeSpan.Parse(dtUltimoBackup.ToString("HH:mm"));
                            TimeSpan diferencaHoras = horaAtual - ultimoBackup;

                            if ((diferencaHoras) == intervalo)
                            {
                                CallBackup(item);

                                dtUltimoBackup = DateTime.Now;
                            }
                        }

                        foreach (string d in item.DiasDaSemana)
                        {
                            if (d.ToLower() == dia)
                            {
                                if (DateTime.Parse(item.ValorHoraFixa).ToString("HH:mm") == DateTime.Now.ToString("HH:mm"))
                                {
                                    CallBackup(item);
                                }
                            }
                        }

                        if (item.UsarConfigApagar)
                        {
                            int dias = item.QtdeDiasParaApagar;

                            foreach (string d in item.Destinos)
                            {
                                if (Directory.Exists(d))
                                {
                                    string[] files = Directory.GetFiles(d);

                                    foreach (string s in files)
                                    {
                                        FileInfo file = new FileInfo(s);
                                        if (Path.GetExtension(file.ToString()) == ".sql" || Path.GetExtension(file.ToString()) == ".rar" || Path.GetExtension(file.ToString()) == ".zip")
                                            if (file.CreationTime < DateTime.Now.AddDays(-dias))
                                                file.Delete();
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogErro("LOGERRO.TXT", "Timer2_Tick", ex.Message);
            }
        }
        private void FazerBackup(Propriedades item)
        {
            try
            {
                string nomeArquivo = "", constring = "";

                string pathDestinoArquivo = string.Empty;

                foreach (string b in item.ListaBancos)
                {
                    constring = "server=" + item.IpBanco + ";port=" + item.PortaBanco + ";user id=" + item.UsuarioBanco + ";Password=" + item.SenhaBanco + ";database=" + b + ";Convert Zero Datetime=True;pooling=false; Allow User Variables=True;";

                    using (MySqlConnection conn = new MySqlConnection(constring))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            using (MySqlBackup mb = new MySqlBackup(cmd))
                            {
                                cmd.Connection = conn;
                                conn.Open();

                                mb.ExportProgressChanged += new MySqlBackup.exportProgressChange(mb_ExportProgressChange);

                                foreach (string destino in item.Destinos)
                                {
                                    MemoryStream ms = new MemoryStream();

                                    nomeArquivo = item.Nome.RemoveCaracteresEspeciais().RemoveWhitespace() + "_" + b + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".sql";
                                    mValorMSG = "Fazendo backup de " + b + " para \r\nPasta: " + destino + "\r\nArquivo: " + nomeArquivo;

                                    if (item.Compactar)
                                    {
                                        mb.ExportToMemoryStream(ms);
                                    }

                                    else
                                    {
                                        pathDestinoArquivo = destino + "\\" + nomeArquivo;
                                        mb.ExportToFile(pathDestinoArquivo);
                                        Log("log.txt", "FazerBackup", item.Nome, b, destino, "Backup efetuado com sucesso!");
                                    }


                                    if (item.Compactar)
                                    {
                                        byte[] data = ms.ToArray();
                                        using (MemoryStream stream = new MemoryStream(data))
                                        {
                                            using (ZipFile zip = new ZipFile())
                                            {
                                                zip.AddEntry(nomeArquivo, stream);
                                                pathDestinoArquivo = destino + "\\" + Path.GetFileNameWithoutExtension(nomeArquivo) + "." + item.Compactador;
                                                zip.Save(pathDestinoArquivo);
                                                Log("log.txt", "FazerBackup", item.Nome, b, destino, "Backup em " + item.Compactador + " efetuado com sucesso!");
                                            }
                                        }

                                        File.Delete(destino + "\\" + nomeArquivo);
                                    }

                                    ConfigFtp host = new ConfigFtp() {
                                        UtilizarHostFtp = item.UtilizarHostFtp,
                                        FilePath = pathDestinoArquivo,
                                        MostrarJanelaNotificacao = item.MostrarJanelaNotificacao,
                                        NomeBanco = item.Nome
                                    };

                                    mFilaEnviarFtp.Enqueue(host);                                    
                                }

                                conn.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogErro("LOGERRO.TXT", "FazerBackup", ex.Message);
            }
        }

        public void mAsyncEnviarFtp_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _telaProgressBar.Text = "Enviando Arquivos...";
            _telaProgressBar.metroProgressBar.Value = e.ProgressPercentage;
            _telaProgressBar.lblProgresso.Text = e.ProgressPercentage.ToString() + "%";
            _telaProgressBar.lblMsg.Text = e.UserState.ToString();
        }

        private void EnviarFtp(BackgroundWorker _worker, string _pathDestino)
        {
            try
            {
                FtpWebRequest ftpRequest;
                FtpWebResponse ftpResponse;

                string nomeArquivo = Path.GetFileName(_pathDestino);

                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(txtHostFtp.Text + @"/" + nomeArquivo));
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                ftpRequest.Proxy = null;
                ftpRequest.UseBinary = true;
                ftpRequest.Credentials = new NetworkCredential(txtUserFtp.Text, txtPasswordFtp.Text);

                using (FileStream fs = File.OpenRead(_pathDestino))
                {
                    using (Stream writer = ftpRequest.GetRequestStream())
                    {
                        var buffer = new byte[1024 * 1024];
                        int totalReadBytesCount = 0;
                        int readBytesCount;
                        while ((readBytesCount = fs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            writer.Write(buffer, 0, readBytesCount);
                            totalReadBytesCount += readBytesCount;
                            var progress = totalReadBytesCount * 100.0 / fs.Length;
                            _worker.ReportProgress((int)progress, "Enviando " + nomeArquivo + " para " + txtHostFtp.Text);
                        }
                    }
                }

                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            }
            catch
            {
                throw;
            }
        }

        private void CallEnviarFtp(ConfigFtp _configFtp)
        {
            if (!mAsyncEnviarFtp.IsBusy)
            {
                _telaProgressBar = new frmTelaAguardeProcessoProgressBar();
                _telaProgressBar.Text = "Enviando FTP...";
                _telaProgressBar.SetDimensions(_telaProgressBar.Width, _telaProgressBar.Height);

                if (_configFtp.MostrarJanelaNotificacao)
                    _telaProgressBar.Notify();
                else
                    _telaProgressBar.Hide();

                mAsyncEnviarFtp.RunWorkerAsync(_configFtp.FilePath);                
            }
        }

        private void mAsyncEnviarFtp_DoWork(object sender, DoWorkEventArgs e)
        {
            worker = sender as BackgroundWorker;
            try
            {
                ConfigFtp config = mFilaEnviarFtp.Dequeue();

                if(config.UtilizarHostFtp)
                    EnviarFtp(worker, e.Argument.ToString());
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message);
                LogErro("LOGERRO.TXT", "backgroudBackupWorker_DoWork", ex.Message);
            }
        }
        private void mAsyncEnviarFtp_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                AtualizaFila();

                _telaProgressBar.ClockState = TestNotifyWindow.NotifyWindow.ClockStates.Showing;

                if (mFilaEnviarFtp.Count != 0)
                    CallEnviarFtp(mFilaEnviarFtp.Peek());

                btnBackup.Enabled = true;
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, "Não foi possível concluir o backup!", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogErro("LOGERRO.TXT", "backgroundackupWorker_RunWorkerCompleted", ex.Message);
            }
        }
        internal void CallBackup(Propriedades _item)
        {
            if (!mAsyncWorker.IsBusy)
            {                
                _telaProgressBar = new frmTelaAguardeProcessoProgressBar();
                _telaProgressBar.Text = "Fazendo Backup...";
                _telaProgressBar.SetDimensions(_telaProgressBar.Width, _telaProgressBar.Height);

                if (_item.MostrarJanelaNotificacao)
                    _telaProgressBar.Notify();
                else
                    _telaProgressBar.Hide();

                mAsyncWorker.RunWorkerAsync(_item);
            }
            else
            {
                mFilaBackup.Enqueue(_item);
                AtualizaFila();
            }
        }

        private void AtualizaFila()
        {
            lstLog.Items.Clear();

            foreach (var item in mFilaBackup)
                lstLog.Items.Add("Backup: " + item.Nome + " - Aguardando...");

            foreach (var item in mFilaEnviarFtp)
                lstLog.Items.Add("Enviar: " + item.NomeBanco);
        }

        private void mAsyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            worker = sender as BackgroundWorker;
            try
            {
                FazerBackup((Propriedades)e.Argument);
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message);
                LogErro("LOGERRO.TXT", "backgroudBackupWorker_DoWork", ex.Message);
            }
        }
        private void mAsyncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                _telaProgressBar.ClockState = TestNotifyWindow.NotifyWindow.ClockStates.Showing;

                if (mFilaBackup.Count != 0)
                {
                    CallBackup(mFilaBackup.Dequeue());
                    AtualizaFila();
                }
                else
                {
                    if (mFilaEnviarFtp.Count != 0)
                        CallEnviarFtp(mFilaEnviarFtp.Peek());
                }

                btnBackup.Enabled = true;
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, "Não foi possível concluir o backup!", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogErro("LOGERRO.TXT", "backgroundackupWorker_RunWorkerCompleted", ex.Message);
            }
        }
       
        private void btnBackup_Click(object sender, EventArgs e)
        {
            btnBackup.Enabled = false;

            if (dgLista.CurrentCell != null)
            {
                foreach (Propriedades item in MemoryProprierties)
                {
                    if (item.Nome == dgLista.SelectedRows[0].Cells[0].Value.ToString())
                    {
                        CallBackup(item);
                        break;
                    }
                }
            }
        }

        private void mb_ExportProgressChange(object sender, ExportProgressArgs e)
        {
            int _totalTables = (int)e.TotalTables;
            int _currentTable = (int)e.CurrentTableIndex;
            int _resultado = (_currentTable * 100) / _totalTables;
            worker.ReportProgress(_resultado);
        }

        public void mAsyncWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _telaProgressBar.lblMsg.Text = mValorMSG;
            _telaProgressBar.metroProgressBar.Value = e.ProgressPercentage;
            _telaProgressBar.lblProgresso.Text = e.ProgressPercentage.ToString() + "%";
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

        public void Log(string _arquivo, string _modulo, string _descricaoConfig, string _banco, string _destino, string _logMensagem)
        {
            using (StreamWriter w = File.AppendText(_arquivo))
            {
                w.WriteLine(" ");
                w.WriteLine(DateTime.Now.ToString("dd/MM/yy-HH:mm") + ";" + _modulo + "; Descricao Config: " + _descricaoConfig + ";Banco:" + _banco + ";Destino:" + _destino + ";Resultado:" + _logMensagem + ";");
                w.Flush();
                w.Close();
            }
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

        private void chkUtilizarHostFtp_CheckedChanged(object sender, EventArgs e)
        {
            grpDadosFtp.Enabled = chkUtilizarHostFtp.Checked;
        }

        private void txtSenha_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                btnConectar.PerformClick();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            using (frmRestore frm = new frmRestore())
            {
                frm.ShowDialog();
            }
        }
    }

    public class ConfigFtp
    {
        public string NomeBanco { get; set; }
        public bool UtilizarHostFtp { get; set; }
        public string FilePath { get; set; }
        public bool MostrarJanelaNotificacao { get; set; }
    }
}
