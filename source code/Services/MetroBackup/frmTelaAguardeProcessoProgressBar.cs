using System.Windows.Forms;
using TestNotifyWindow;

namespace MetroSmartBackup
{
    public partial class frmTelaAguardeProcessoProgressBar : NotifyWindow
    {
        public frmTelaAguardeProcessoProgressBar()
        {
            InitializeComponent();
        }

        public void UpdateProgress(int progresso)
        {
            metroProgressBar.Value = progresso;
            lblProgresso.Text = progresso.ToString() + "%";

            if (progresso >= 100)
                Close();
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
