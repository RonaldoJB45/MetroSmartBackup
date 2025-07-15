using MetroBackup.ApplicationService.Backup.Dtos;
using System.Collections.Generic;

namespace MetroBackup.ApplicationService.Backup
{
    public interface IBackupAppService
    {
        void Executar(BackupDto dto);
        List<UltimoBackupDto> ObterTodos();
    }
}
