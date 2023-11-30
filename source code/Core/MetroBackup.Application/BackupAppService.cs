using MetroBackup.Domain.Interfaces;

namespace MetroBackup.Application
{
    public class BackupAppService : IBackupAppService
    {
        private readonly IBackupService _backupService;

        public BackupAppService(IBackupService backupService)
        {
            _backupService = backupService;
        }

        public void Executar(ConfiguracaoDto dto)
        {
            var configuracao = dto.ToConfiguracaoEntity();
            _backupService.Executar(configuracao);
        }
    }
}
