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

        public void UpdateProgress(int progresso)
        {
            if (progresso >= 100)
            {
                ClockState = ClockStates.Showing;
                progresso = 100;
            }

            metroProgressBar.Value = progresso;
            lblProgresso.Text = progresso.ToString() + "%";
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
