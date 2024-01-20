using System;

namespace MetroBackup.ApplicationService
{
    public interface IBackupAppService
    {
        void Executar(Guid id);
    }
}
