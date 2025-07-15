using MetroBackup.Domain.Entities;

namespace MetroBackup.Domain.Interfaces.Services
{
    public interface IBackupService
    {
        void Executar(Configuracao configuracao);
    }
}
