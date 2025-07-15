using MetroBackup.Domain.Entities;
using System.Collections.Generic;

namespace MetroBackup.Domain.Interfaces.Repository
{
    public interface IUltimoBackupRepository
    {
        List<UltimoBackup> ObterTodos();
        void Adicionar(UltimoBackup ultimoBackup);
        void Alterar(UltimoBackup ultimoBackup);
    }
}