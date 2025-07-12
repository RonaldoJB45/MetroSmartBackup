using MetroBackup.Domain.ValueObjets;

namespace MetroBackup.Domain.Interfaces
{
    public interface IRestoreService
    {
        void Restaurar(Servidor servidor);
    }
}