using MetroBackup.Domain.Interfaces;
using MetroBackup.Domain.ValueObjets;

namespace MetroBackup.ApplicationService.Restauracoes
{
    public class RestoreAppService : IRestoreAppService
    {
        private readonly IRestoreService _restoreService;

        public RestoreAppService(IRestoreService restoreService)
        {
            _restoreService = restoreService;
        }

        public void Restaurar(RestoreDto dto)
        {
            var servidor = new Servidor(
                dto.IpBanco,
                dto.PortaBanco,
                dto.UsuarioBanco,
                dto.SenhaBanco,
                dto.NomeBanco,
                dto.Arquivo);

            _restoreService.Restaurar(servidor);
        }
    }
}