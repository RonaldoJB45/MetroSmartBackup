using Ionic.Zip;
using MetroBackup.Domain.Entities;
using MetroBackup.Domain.Interfaces;
using System;
using System.IO;

namespace MetroBackup.Infra.Acl
{
    public class BackupService : IBackupService
    {
        public void Executar(Configuracao configuracao)
        {
            var mySqlService = new MySqlService();

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
            using (ZipFile zip = new ZipFile())
            {
                zip.AddEntry(nomeBanco, ms);
                string pathDestinoArquivo = destino + "\\" + Formatar(nomeBanco, compactador);
                zip.Save(pathDestinoArquivo);
            }
        }

        static void SalvarArquivo(MemoryStream memoryStream, string nomeBanco, string destino)
        {
            string pathDestinoArquivo = destino + "\\" + Formatar(nomeBanco, ".sql");

            using (FileStream fileStream = new FileStream(pathDestinoArquivo, FileMode.Create, FileAccess.Write))
            {
                memoryStream.WriteTo(fileStream);
            }
        }
        static string Formatar(string nome, string extensao)
        {
            return nome + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + extensao;
        }
    }
}
