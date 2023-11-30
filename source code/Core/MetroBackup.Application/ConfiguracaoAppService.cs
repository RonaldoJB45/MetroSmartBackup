using MetroBackup.Domain.Interfaces;
using System.Collections.Generic;
using System;

namespace MetroBackup.Application
{
    public class ConfiguracaoAppService : IConfiguracaoAppService
    {
        private readonly IConfiguracaoRepository _configuracaoRepository;

        public ConfiguracaoAppService(IConfiguracaoRepository configuracaoRepository)
        {
            _configuracaoRepository = configuracaoRepository;
        }

        public void Adicionar(ConfiguracaoDto configuracaoDto)
        {
            var configuracao = configuracaoDto.ToConfiguracaoEntity();
            _configuracaoRepository.Adicionar(configuracao);
        }

        public void Alterar(ConfiguracaoDto configuracaoDto)
        {
            var configuracao = _configuracaoRepository.ObterPorId(Guid.NewGuid());

            if (configuracao != null)
            {
                _configuracaoRepository.Alterar(configuracao);
            }
        }

        public ConfiguracaoDto ObterPorId(Guid id)
        {
            var configuracao = _configuracaoRepository.ObterPorId(id);
            return configuracao.ToConfiguracaoDto();
        }

        public List<ConfiguracaoDto> ObterTodos()
        {
            var configuracoes = _configuracaoRepository.ObterTodos();
            return configuracoes.ToConfiguracoesDto();
        }

        public void Remover(Guid id)
        {
            _configuracaoRepository.Remover(id);
        }
    }
}
