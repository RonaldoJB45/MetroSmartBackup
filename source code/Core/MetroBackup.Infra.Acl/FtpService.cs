using MetroBackup.Domain.Entities;
using MetroBackup.Domain.Interfaces;
using System;
using System.IO;
using System.Net;

namespace MetroBackup.Infra.Acl
{
    public class FtpService : IFtpService
    {
        public void Enviar(Configuracao configuracao)
        {
            var ftp = configuracao.Ftp;

            string host = ftp.Host;
            string user = ftp.User;
            string password = ftp.Password;

            if (!host.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase))
                host = "ftp://" + host;

            foreach (var servidor in configuracao.Servidores)
            {
                string caminhoArquivoLocal = servidor.CaminhoBackup;

                try
                {
                    if (!File.Exists(caminhoArquivoLocal))
                        continue;

                    string nomeArquivo = Path.GetFileName(caminhoArquivoLocal);
                    string caminhoRemoto = $"{host}/{nomeArquivo}";

                    UploadArquivoFtp(caminhoArquivoLocal, caminhoRemoto, user, password);
                }
                catch (Exception ex)
                {
                    servidor.AddErro($"Erro ao enviar {caminhoArquivoLocal}: {ex.Message}");
                }
            }
        }
        private void UploadArquivoFtp(string caminhoLocal, string caminhoRemoto, string user, string password)
        {
            var request = (FtpWebRequest)WebRequest.Create(caminhoRemoto);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.UseBinary = true;
            request.Proxy = null;
            request.Credentials = new NetworkCredential(user, password);

            using (FileStream fs = File.OpenRead(caminhoLocal))
            using (Stream requestStream = request.GetRequestStream())
            {
                byte[] buffer = new byte[8192];
                int bytesRead;
                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                }
            }
        }
    }
}
