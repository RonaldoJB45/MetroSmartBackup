using MetroBackup.ApplicationService.Configuracoes;
using MetroBackup.ApplicationService.BancoDados;
using MetroBackup.Domain.Interfaces;
using MetroBackup.Infra.Acl;
using MetroBackup.Infra.Data;
using System.Windows.Forms;
using System;

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

            IBancoDadosService bancoDadosService = new BancoDadosService();
            IBancoDadosAppService bancoDadosAppService = new BancoDadosAppService(bancoDadosService);

            Application.Run(new frmPrincipal(configuracaoAppService, bancoDadosAppService));
        }
    }
}
