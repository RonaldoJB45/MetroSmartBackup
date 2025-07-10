using MetroBackup.Domain.Interfaces;

namespace MetroBackup.ApplicationService.Backup
{
    public class BackupAppService : IBackupAppService
    {
        private readonly IBackupService _backupService;
        private readonly IFtpService _ftpService;
        private readonly IConfiguracaoRepository _configuracaoRepository;

        public BackupAppService(
            IBackupService backupService,
            IFtpService ftpService,
            IConfiguracaoRepository configuracaoRepository)
        {
            _backupService = backupService;
            _ftpService = ftpService;
            _configuracaoRepository = configuracaoRepository;
        }

        public void Executar(BackupDto dto)
        {
            var configuracao = _configuracaoRepository.ObterPorId(dto.ConfiguracaoId);
            _backupService.Executar(configuracao);

            if (configuracao.Ftp.Utilizar)
                _ftpService.Enviar(configuracao);
        }
    }
}
