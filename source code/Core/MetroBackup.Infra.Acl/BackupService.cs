using Ionic.Zip;
using MetroBackup.Domain.Entities;
using MetroBackup.Domain.Interfaces;
using System;
using System.IO;

namespace MetroBackup.Infra.Acl
{
    public class BackupService : IBackupService
    {
        private readonly IProgressReporter _progressReporter;

        public BackupService(IProgressReporter progressReporter)
        {
            _progressReporter = progressReporter;
        }

        public void Executar(Configuracao configuracao)
        {
            var mySqlService = new MySqlService(_progressReporter);

            foreach (var servidor in configuracao.Servidores)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    mySqlService.BackupToMemoryStream(ms, servidor.MySqlConnectionStrings);

                    foreach (var destino in configuracao.Destinos)
                    {
                        if (configuracao.Compactar)
                            Compactar(ms, servidor.NomeBanco, destino, configuracao.Compactador);
                        else
                            SalvarArquivo(ms, servidor.NomeBanco, destino);
                    }
                }
            }
        }

        static void Compactar(MemoryStream ms, string nomeBanco, string destino, string compactador)
        {
            if (!Directory.Exists(destino))
                Directory.CreateDirectory(destino);

            byte[] data = ms.ToArray();

            using (MemoryStream stream = new MemoryStream(data))
            {
                using (ZipFile zip = new ZipFile())
                {
                    string pathDestinoArquivo = destino + "\\" + Formatar(nomeBanco, compactador);
                    zip.AddEntry(Formatar(nomeBanco, "sql"), stream);
                    zip.Save(pathDestinoArquivo);
                }
            }
        }

        static void SalvarArquivo(MemoryStream memoryStream, string nomeBanco, string destino)
        {
            if (!Directory.Exists(destino))
                Directory.CreateDirectory(destino);

            string pathDestinoArquivo = destino + "\\" + Formatar(nomeBanco, "sql");

            using (FileStream fileStream = new FileStream(pathDestinoArquivo, FileMode.Create, FileAccess.Write))
            {
                memoryStream.WriteTo(fileStream);
            }
        }
        static string Formatar(string nome, string extensao)
        {
            return nome + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + "." + extensao;
        }
    }
}
