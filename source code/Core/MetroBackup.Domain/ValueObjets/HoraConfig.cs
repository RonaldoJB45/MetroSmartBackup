using MetroBackup.Domain.Enums;

namespace MetroBackup.Domain.ValueObjets
{
    public class HoraConfig
    {
        public TipoConfiguracao TipoConfiguracao { get; private set; }
        public int Intervalo { get; set; }
        public string HoraFixa { get; set; }
    }
}
