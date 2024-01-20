using MetroBackup.ApplicationService;
using MetroBackup.Domain.Interfaces;
using MetroBackup.Infra.Data;
using System;
using System.Windows.Forms;

namespace MetroBackup
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string caminhoArquivo = Application.StartupPath + @"\config.json";
            FileContext fileContext = new FileContext(caminhoArquivo);
            IConfiguracaoRepository configuracaoRepository = new ConfiguracaoRepository(fileContext);
            IConfiguracaoAppService configuracaoAppService = new ConfiguracaoAppService(configuracaoRepository);

            Application.Run(new frmPrincipal(configuracaoAppService));
        }
    }
}
