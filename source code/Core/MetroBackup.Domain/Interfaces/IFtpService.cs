using MetroBackup.Domain.Entities;

namespace MetroBackup.Domain.Interfaces
{
    public interface IFtpService
    {
        void Enviar(Configuracao configuracao);
    }
}