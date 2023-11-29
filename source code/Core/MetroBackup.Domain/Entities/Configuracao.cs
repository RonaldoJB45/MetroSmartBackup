using MetroBackup.Domain.ValueObjets;
using System;
using System.Collections.Generic;

namespace MetroBackup.Domain.Entities
{
    public class Configuracao
    {
        public Configuracao(
            string descricao,
            List<Servidor> servidores,
            HoraConfig horaConfig,
            Ftp ftp,
            string[] diasDaSemana,
            string[] destinos,
            bool usarConfigApagar,
            int qtdeDiasParaApagar,
            bool compactar,
            string compactador,
            bool exibirNotificacao)
        {
            Id = Guid.NewGuid();
            Descricao = descricao;
            Servidores = servidores;
            HoraConfig = horaConfig;
            Ftp = ftp;
            DiasDaSemana = diasDaSemana;
            Destinos = destinos;
            UsarConfigApagar = usarConfigApagar;
            QtdeDiasParaApagar = qtdeDiasParaApagar;
            Compactar = compactar;
            Compactador = compactador;
            ExibirNotificacao = exibirNotificacao;
        }

        public Guid Id { get; private set; }
        public string Descricao { get; private set; }
        public HoraConfig HoraConfig { get; private set; }
        public Ftp Ftp { get; private set; }

        public List<Servidor> Servidores { get; private set; }
        public string[] DiasDaSemana { get; private set; }
        public string[] Destinos { get; private set; }

        public bool UsarConfigApagar { get; private set; }
        public int QtdeDiasParaApagar { get; private set; }
        public bool Compactar { get; private set; }
        public string Compactador { get; private set; }
        public bool ExibirNotificacao { get; private set; }
    }
}
