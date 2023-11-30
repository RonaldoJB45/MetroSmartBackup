using MetroBackup.Domain.Entities;
using System.Collections.Generic;
using System;

namespace MetroBackup.Domain.Interfaces
{
    public interface IConfiguracaoRepository
    {
        List<Configuracao> ObterTodos();
        Configuracao ObterPorId(Guid id);
        void Adicionar(Configuracao configuracao);
        void Alterar(Configuracao configuracao);
        void Remover(Guid id);
    }
}
