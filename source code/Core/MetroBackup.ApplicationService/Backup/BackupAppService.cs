using MetroBackup.Domain.Interfaces.Services;
using MetroBackup.Domain.Interfaces.Repository;
using MetroBackup.ApplicationService.Backup.Dtos;
using System.Collections.Generic;

namespace MetroBackup.ApplicationService.Backup
{
    public class BackupAppService : IBackupAppService
    {
        private readonly IBackupService _backupService;
        private readonly IFtpService _ftpService;
        private readonly IConfiguracaoRepository _configuracaoRepository;
        private readonly IUltimoBackupRepository _ultimoBackupRepository;

        public BackupAppService(
            IBackupService backupService,
            IFtpService ftpService,
            IConfiguracaoRepository configuracaoRepository,
            IUltimoBackupRepository ultimoBackupRepository)
        {
            _backupService = backupService;
            _ftpService = ftpService;
            _configuracaoRepository = configuracaoRepository;
            _ultimoBackupRepository = ultimoBackupRepository;
        }

        public void Executar(BackupDto dto)
        {
            var configuracao = _configuracaoRepository.ObterPorId(dto.ConfiguracaoId);
            _backupService.Executar(configuracao);

            if (configuracao.Ftp.Utilizar)
                _ftpService.Enviar(configuracao);
        }

        public List<UltimoBackupDto> ObterTodos()
        {
            var todos = _ultimoBackupRepository.ObterTodos();

            List<UltimoBackupDto> lst = new List<UltimoBackupDto>();

            foreach (var ultimoBackup in todos)
            {
                lst.Add(new UltimoBackupDto
                {
                    ConfiguracaoId = ultimoBackup.ConfiguracaoId,
                    DataHora = ultimoBackup.DataHora,
                    Origem = ultimoBackup.Origem
                });
            }

            return lst;
        }
    }
}
