namespace MetroBackup.Application
{
    public interface IBackupAppService
    {
        void Executar(ConfiguracaoDto dto);
    }
}
