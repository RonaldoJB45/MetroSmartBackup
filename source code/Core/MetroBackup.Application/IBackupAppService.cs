using System;

namespace MetroBackup.Application
{
    public interface IBackupAppService
    {
        void Executar(Guid id);
    }
}
