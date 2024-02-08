using System;

namespace MetroBackup.ApplicationService.Backup
{
    public interface IBackupAppService
    {
        void Executar(Guid id);
    }
}
