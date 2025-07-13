using MetroBackup.Domain.Entities;
using System;

namespace MetroBackup.ApplicationService.Backup.Dtos
{
    public class UltimoBackupDto
    {
        public Guid ConfiguracaoId { get; set; }
        public DateTime DataHora { get; set; }
        public Origem Origem { get; set; }
    }
}