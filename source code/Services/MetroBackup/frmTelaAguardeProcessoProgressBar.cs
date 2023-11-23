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

        private void frmTelaAguardeProcessoProgressBar_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = false;
            else
                e.Cancel = true;

        }
    }
}
