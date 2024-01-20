using System.Collections.Generic;
using System;

namespace MetroBackup.ApplicationService
{
    public interface IConfiguracaoAppService
    {
        List<ConfiguracaoDto> ObterTodos();
        ConfiguracaoDto ObterPorId(Guid id);
        void Adicionar(ConfiguracaoDto configuracaoDto);
        void Alterar(ConfiguracaoDto configuracaoDto);
        void Remover(Guid id);
    }
}