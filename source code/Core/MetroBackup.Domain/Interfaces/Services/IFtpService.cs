using MetroBackup.Domain.Entities;

namespace MetroBackup.Domain.Interfaces.Services
{
    public interface IFtpService
    {
        void Enviar(Configuracao configuracao);
    }
}