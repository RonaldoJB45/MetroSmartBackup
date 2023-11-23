using MetroBackup.Domain.Entities;
using System;
using System.Collections.Generic;

namespace MetroBackup.Domain.Interfaces
{
    public interface IBackupRepository
    {
        List<Configuracao> ObterTodos();
        Configuracao ObterPorId(Guid id);
        void Adicionar(Configuracao configuracao);
        void Atualizar(Configuracao configuracao);
    }
}
