using MetroBackup.Domain.Entities;
using System.Collections.Generic;

namespace MetroBackup.Domain.Interfaces
{
    public interface IBackupRepository
    {
        List<Configuracao> ObterTodos();
        void Atualizar(Configuracao configuracao);
    }
}
