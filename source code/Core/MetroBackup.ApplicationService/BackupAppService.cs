using MetroBackup.Domain.Interfaces;
using System;

namespace MetroBackup.ApplicationService
{
    public class BackupAppService : IBackupAppService
    {
        private readonly IBackupService _backupService;
        private readonly IConfiguracaoRepository _configuracaoRepository;

        public BackupAppService(
            IBackupService backupService,
            IConfiguracaoRepository configuracaoRepository)
        {
            _backupService = backupService;
            _configuracaoRepository = configuracaoRepository;
        }

        public void Executar(Guid id)
        {
            var configuracao = _configuracaoRepository.ObterPorId(id);
            _backupService.Executar(configuracao);
        }
    }
}
