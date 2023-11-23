using MetroBackup.Domain.Entities;

namespace MetroBackup.Domain.Interfaces
{
    public interface IBackupService
    {
        void Executar(Configuracao configuracao);
    }
}
