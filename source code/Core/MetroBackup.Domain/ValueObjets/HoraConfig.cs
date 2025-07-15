using MetroBackup.Domain.Enums;

namespace MetroBackup.Domain.ValueObjets
{
    public class HoraConfig
    {
        public HoraConfig(
            TipoConfiguracao tipoConfiguracao,
            int intervalo,
            string horaFixa)
        {
            TipoConfiguracao = tipoConfiguracao;
            Intervalo = intervalo;
            HoraFixa = horaFixa;
        }

        public TipoConfiguracao TipoConfiguracao { get; private set; }
        public int Intervalo { get; private set; }
        public string HoraFixa { get; private set; }
    }
}
