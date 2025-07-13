using System;

namespace MetroBackup.Domain.Entities
{
    public class UltimoBackup
    {
        public UltimoBackup(
            Guid configuracaoId, 
            DateTime? dataHora = null,
            Origem origem = Origem.Manual)
        {
            ConfiguracaoId = configuracaoId;
            DataHora = dataHora ?? DateTime.Now;
            Origem = origem;
        }

        public Guid ConfiguracaoId { get; private set; }
        public DateTime DataHora { get; private set; }
        public Origem Origem { get; private set; }
    }
    public enum Origem
    {
        Manual = 0,
        HoraFixa = 1,
        Intervalo = 2
    }
}