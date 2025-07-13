using MetroBackup.Domain.Entities;
using System;

namespace MetroBackup.ApplicationService.Backup.Dtos
{
    public class BackupDto
    {
        public Guid ConfiguracaoId { get; set; }
        public Origem Origem { get; set; }
    }
}