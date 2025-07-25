﻿using System;
using System.Collections.Generic;

namespace MetroBackup.ApplicationService.Configuracoes
{
    public class ConfiguracaoDto
    {
        public ConfiguracaoDto()
        {
            Servidores = new List<ServidorDto>();
        }

        public Guid? Id { get; set; }
        public string Descricao { get; set; }
        public List<ServidorDto> Servidores { get; set; }
        public string[] DiasDaSemana { get; set; }
        public bool UsarIntervaloHoras { get; set; }
        public int ValorIntervaloHoras { get; set; }
        public bool UsarHoraFixa { get; set; }
        public string ValorHoraFixa { get; set; }
        public bool UsarConfigApagar { get; set; }
        public int QtdeDiasParaApagar { get; set; }
        public bool Compactar { get; set; }
        public string Compactador { get; set; }
        public string[] Destinos { get; set; }
        public bool MostrarJanelaNotificacao { get; set; }
        public bool UtilizarHostFtp { get; set; }
        public string HostFtp { get; set; }
        public string UserFtp { get; set; }
        public string PasswordFtp { get; set; }
    }
}
