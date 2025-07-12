using MetroBackup.Domain.ValueObjets;

namespace MetroBackup.Domain.Interfaces.Services
{
    public interface IRestoreService
    {
        void Restaurar(Servidor servidor);
    }
}