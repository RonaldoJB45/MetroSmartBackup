using MetroBackup.Domain.ValueObjets;
using System;
using System.Collections.Generic;

namespace MetroBackup.Domain.Entities
{
    public class Configuracao
    {
        public Configuracao(
            Guid? id,
            string descricao,
            List<Servidor> servidores,
            Ftp ftp,
            bool usarIntervaloHoras,
            bool usarHoraFixa,
            int intervaloHora,
            string horaFixa,
            string[] diasDaSemana,
            string[] destinos,
            bool usarConfigApagar,
            int qtdeDiasParaApagar,
            bool compactar,
            string compactador,
            bool exibirNotificacao)
        {
            Id = id ?? Guid.NewGuid();
            Descricao = descricao;
            Servidores = servidores;
            UsarIntervaloHoras = usarIntervaloHoras;
            UsarHoraFixa = usarHoraFixa;
            IntervaloHora = intervaloHora;
            HoraFixa = horaFixa;
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

        public bool UsarIntervaloHoras { get; private set; }
        public bool UsarHoraFixa { get; private set; }

        public int IntervaloHora { get; private set; }
        public string HoraFixa { get; private set; }

        public Ftp Ftp { get; private set; }

        public List<Servidor> Servidores { get; private set; }
        public string[] DiasDaSemana { get; private set; }
        public string[] Destinos { get; private set; }

        public bool UsarConfigApagar { get; private set; }
        public int QtdeDiasParaApagar { get; private set; }
        public bool Compactar { get; private set; }
        public string Compactador { get; private set; }
        public bool ExibirNotificacao { get; private set; }

        public void Alterar(
            string descricao,
            List<Servidor> servidores,
            Ftp ftp,
            bool usarIntervaloHoras,
            bool usarHoraFixa,
            int intervaloHora,
            string horaFixa,
            string[] diasDaSemana,
            string[] destinos,
            bool usarConfigApagar,
            int qtdeDiasParaApagar,
            bool compactar,
            string compactador,
            bool exibirNotificacao)
        {
            Descricao = descricao;
            Servidores = servidores;
            Ftp = ftp;
            UsarIntervaloHoras = usarIntervaloHoras;
            UsarHoraFixa = usarHoraFixa;
            IntervaloHora = intervaloHora;
            HoraFixa = horaFixa;
            DiasDaSemana = diasDaSemana;
            Destinos = destinos;
            UsarConfigApagar = usarConfigApagar;
            QtdeDiasParaApagar = qtdeDiasParaApagar;
            Compactar = compactar;
            Compactador = compactador;
            ExibirNotificacao = exibirNotificacao;
        }
    }
}
