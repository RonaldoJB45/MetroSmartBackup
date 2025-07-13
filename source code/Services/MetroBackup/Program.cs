using MetroBackup.ApplicationService.Configuracoes;
using MetroBackup.ApplicationService.BancoDados;
using MetroBackup.ApplicationService.Restauracoes;
using MetroBackup.ApplicationService.Backup;
using MetroBackup.Domain.Interfaces.Services;
using MetroBackup.Domain.Interfaces.Repository;
using MetroBackup.Domain.Interfaces;
using MetroBackup.Domain.Services;
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

            string caminhoArquivoConfiguracao = Application.StartupPath + @"\config.json";
            string caminhoArquivoUltimosBackups = Application.StartupPath + @"\ultimos-backups.json";

            FileContext fileConfiguracaoContext = new FileContext(caminhoArquivoConfiguracao);
            FileContext fileUltimosBackupsContext = new FileContext(caminhoArquivoUltimosBackups);

            IConfiguracaoRepository configuracaoRepository = new ConfiguracaoRepository(fileConfiguracaoContext);
            IConfiguracaoAppService configuracaoAppService = new ConfiguracaoAppService(configuracaoRepository);

            IBancoDadosService bancoDadosService = new BancoDadosService();
            IBancoDadosAppService bancoDadosAppService = new BancoDadosAppService(bancoDadosService);

            IProgressReporter progressReporter = new ProgressReporter();

            IUltimoBackupRepository ultimoBackupRepository = new UltimoBackupRepository(fileUltimosBackupsContext);
            IBackupService backupService = new BackupService(progressReporter, ultimoBackupRepository);
            IFtpService ftpService = new FtpService(progressReporter);
            IBackupAppService backupAppService = new BackupAppService(
                backupService,
                ftpService,
                configuracaoRepository,
                ultimoBackupRepository);

            IRestoreService restoreService = new RestoreService(progressReporter);
            IRestoreAppService restoreAppService = new RestoreAppService(restoreService);

            var frmPrincipal = new frmPrincipal(configuracaoAppService,
                                                bancoDadosAppService,
                                                backupAppService,
                                                restoreAppService,
                                                progressReporter);

            Application.Run(frmPrincipal);
        }
    }
}
