using MetroBackup.Domain.Entities;
using MetroBackup.Domain.Enums;
using MetroBackup.Domain.Interfaces;
using MetroBackup.Domain.ValueObjets;

namespace MetroBackup.Application
{
    public class BackupAppService : IBackupAppService
    {
        private readonly IBackupService _backupService;

        public BackupAppService(IBackupService backupService)
        {
            _backupService = backupService;
        }

        public void Executar(ConfiguracaoDto dto)
        {
            var servidor = new Servidor(dto.IpBanco, dto.PortaBanco, dto.UsuarioBanco, dto.SenhaBanco);

            var tipoConfiguracao = dto.UsarIntervaloHoras ? TipoConfiguracao.Intervalo : TipoConfiguracao.Fixo;

            var horaConfig = new HoraConfig(tipoConfiguracao, dto.ValorIntervaloHoras, dto.ValorHoraFixa);

            var ftp = new Ftp(dto.UtilizarHostFtp, dto.HostFtp, dto.UserFtp, dto.PasswordFtp);

            var configuracao = new Configuracao(
                dto.Descricao,
                servidor,
                horaConfig,
                ftp,
                dto.ListaBancos,
                dto.DiasDaSemana,
                dto.Destinos,
                dto.UsarConfigApagar,
                dto.QtdeDiasParaApagar,
                dto.Compactar,
                dto.Compactador,
                dto.MostrarJanelaNotificacao);

            _backupService.Executar(configuracao);
        }
    }
}
