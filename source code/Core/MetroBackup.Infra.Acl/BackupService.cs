using MetroBackup.Domain.Interfaces.Repository;
using MetroBackup.Domain.Interfaces.Services;
using MetroBackup.Domain.Interfaces;
using MetroBackup.Domain.Entities;
using System;
using System.IO;
using Ionic.Zip;

namespace MetroBackup.Infra.Acl
{
    public class BackupService : IBackupService
    {
        private readonly IProgressReporter _progressReporter;
        private readonly IUltimoBackupRepository _ultimoBackupRepository;

        public BackupService(
            IProgressReporter progressReporter, 
            IUltimoBackupRepository ultimoBackupRepository)
        {
            _progressReporter = progressReporter;
            _ultimoBackupRepository = ultimoBackupRepository;
        }

        public void Executar(Configuracao configuracao)
        {
            var mySqlService = new MySqlService(_progressReporter);

            foreach (var servidor in configuracao.Servidores)
            {
                using (var msOriginal = new MemoryStream())
                {
                    mySqlService.BackupToMemoryStream(msOriginal, servidor.MySqlConnectionStrings);
                    byte[] backupBytes = msOriginal.ToArray();

                    foreach (var destino in configuracao.Destinos)
                    {
                        string pathArquivo;

                        if (configuracao.Compactar)
                        {
                            pathArquivo = Compactar(backupBytes, servidor.NomeBanco, destino, configuracao.Compactador);
                        }
                        else
                        {
                            pathArquivo = SalvarArquivo(backupBytes, servidor.NomeBanco, destino);
                        }

                        servidor.AddCaminhoBackup(pathArquivo);
                    }
                }
            }

            SalvarUltimoBackup(configuracao.Id);
        }
        private void SalvarUltimoBackup(Guid configuracaoId)
        {
            var ultimoBackup = new UltimoBackup(configuracaoId);
            _ultimoBackupRepository.Alterar(ultimoBackup);
        }
        private static string Compactar(byte[] backupData, string nomeBanco, string destino, string compactador)
        {
            if (!Directory.Exists(destino))
                Directory.CreateDirectory(destino);

            string nomeArquivoZip = Formatar(nomeBanco, compactador);
            string pathDestinoArquivo = Path.Combine(destino, nomeArquivoZip);

            using (var stream = new MemoryStream(backupData))
            using (var zip = new ZipFile())
            {
                zip.AddEntry(Formatar(nomeBanco, "sql"), stream);
                zip.Save(pathDestinoArquivo);
            }

            return pathDestinoArquivo;
        }
        private static string SalvarArquivo(byte[] backupData, string nomeBanco, string destino)
        {
            if (!Directory.Exists(destino))
                Directory.CreateDirectory(destino);

            string nomeArquivo = Formatar(nomeBanco, "sql");
            string pathDestinoArquivo = Path.Combine(destino, nomeArquivo);

            using (var fileStream = new FileStream(pathDestinoArquivo, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(backupData, 0, backupData.Length);
            }

            return pathDestinoArquivo;
        }

        private static string Formatar(string nome, string extensao)
        {
            return $"{nome}_{DateTime.Now:ddMMyyyyHHmmssfff}.{extensao}";
        }
    }
}
