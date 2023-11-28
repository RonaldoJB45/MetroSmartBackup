using MetroBackup.Domain.Entities;
using MetroBackup.Domain.Enums;
using MetroBackup.Domain.ValueObjets;
using System.Collections.Generic;

namespace MetroBackup.Application
{
    internal static class ConfiguracaoTranslator
    {
        internal static Configuracao ToConfiguracaoEntity(this ConfiguracaoDto configuracaoDto)
        {
            var servidor = new Servidor(
                configuracaoDto.IpBanco,
                configuracaoDto.PortaBanco,
                configuracaoDto.UsuarioBanco,
                configuracaoDto.SenhaBanco);

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

            var configuracao = new Configuracao(
                configuracaoDto.Descricao,
                servidor,
                horaConfig,
                ftp,
                configuracaoDto.ListaBancos,
                configuracaoDto.DiasDaSemana,
                configuracaoDto.Destinos,
                configuracaoDto.UsarConfigApagar,
                configuracaoDto.QtdeDiasParaApagar,
                configuracaoDto.Compactar,
                configuracaoDto.Compactador,
                configuracaoDto.MostrarJanelaNotificacao);

            return configuracao;
        }
        internal static List<ConfiguracaoDto> ToConfiguracoesDto(this List<Configuracao> configuracoes)
        {
            List<ConfiguracaoDto> configuracoesDto = new List<ConfiguracaoDto>();

            if (configuracoes == null) return configuracoesDto;

            foreach (var configuracao in configuracoes)
                configuracoesDto.Add(configuracao.ToConfiguracaoDto());

            return configuracoesDto;
        }
        internal static ConfiguracaoDto ToConfiguracaoDto(this Configuracao configuracao)
        {
            return new ConfiguracaoDto
            {
                Id = configuracao.Id,
                Descricao = configuracao.Descricao,
                IpBanco = configuracao.Servidor.Endereco,
                PortaBanco = configuracao.Servidor.Porta,
                UsuarioBanco = configuracao.Servidor.Usuario,
                SenhaBanco = configuracao.Servidor.Senha,
                ListaBancos = configuracao.ListaBancos,
                DiasDaSemana = configuracao.DiasDaSemana,
                UsarIntervaloHoras = configuracao.HoraConfig.TipoConfiguracao == TipoConfiguracao.Intervalo,
                ValorIntervaloHoras = configuracao.HoraConfig.Intervalo,
                UsarHoraFixa = configuracao.HoraConfig.TipoConfiguracao == TipoConfiguracao.Fixo,
                ValorHoraFixa = configuracao.HoraConfig.HoraFixa,
                UsarConfigApagar = configuracao.UsarConfigApagar,
                QtdeDiasParaApagar = configuracao.QtdeDiasParaApagar,
                Compactar = configuracao.Compactar,
                Compactador = configuracao.Compactador,
                Destinos = configuracao.Destinos,
                MostrarJanelaNotificacao = configuracao.ExibirNotificacao,
                UtilizarHostFtp = configuracao.Ftp.Utilizar,
                HostFtp = configuracao.Ftp.Host,
                UserFtp = configuracao.Ftp.User,
                PasswordFtp = configuracao.Ftp.Password
            };
        }
    }
}
