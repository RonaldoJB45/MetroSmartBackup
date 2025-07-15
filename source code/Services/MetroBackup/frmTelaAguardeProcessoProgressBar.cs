using System;
using System.Windows.Forms;
using TestNotifyWindow;

namespace MetroSmartBackup
{
    public partial class frmTelaAguardeProcessoProgressBar : NotifyWindow
    {
        public frmTelaAguardeProcessoProgressBar()
        {
            InitializeComponent();
            SetDimensions(Width, Height);
        }

        public void AtualizarProgresso(int progresso, string mensagem)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AtualizarProgresso(progresso, mensagem)));
                return;
            }

            metroProgressBar.Value = progresso;
            lblProgresso.Text = progresso.ToString() + "%";
            lblMsg.Text = mensagem ?? "Aguarde!";
        }

        private void frmTelaAguardeProcessoProgressBar_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = false;
            else
                e.Cancel = true;
        }
    }
}
