﻿using MetroBackup.Domain.ValueObjets;
using MetroBackup.Domain.Interfaces;
using System.Collections.Generic;
using MetroBackup.Domain.Enums;
using System;

namespace MetroBackup.ApplicationService
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
            var configuracao = _configuracaoRepository.ObterPorId(configuracaoDto.Id.Value);

            if (configuracao != null)
            {
                List<Servidor> servidores = new List<Servidor>();

                foreach (var servidorDto in configuracaoDto.Servidores)
                {
                    servidores.Add(new Servidor(
                        servidorDto.NomeBanco,
                        servidorDto.IpBanco,
                        servidorDto.PortaBanco,
                        servidorDto.UsuarioBanco,
                        servidorDto.SenhaBanco));
                }

                var tipoConfiguracao = configuracaoDto.UsarIntervaloHoras ? TipoConfiguracao.Intervalo : TipoConfiguracao.Fixo;

                var horaConfig = new HoraConfig(
                    tipoConfiguracao,
                    configuracaoDto.ValorIntervaloHoras,
                    configuracaoDto.ValorHoraFixa);

                var ftp = new Ftp(
                    configuracaoDto.UtilizarHostFtp,
                    configuracaoDto.HostFtp,
                    configuracaoDto.UserFtp,
                    configuracaoDto.PasswordFtp);

                configuracao.Alterar(
                    configuracaoDto.Descricao,
                    servidores,
                    horaConfig,
                    ftp,
                    configuracaoDto.DiasDaSemana,
                    configuracaoDto.Destinos,
                    configuracaoDto.UsarConfigApagar,
                    configuracaoDto.QtdeDiasParaApagar,
                    configuracaoDto.Compactar,
                    configuracaoDto.Compactador,
                    configuracaoDto.MostrarJanelaNotificacao
                    );

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
