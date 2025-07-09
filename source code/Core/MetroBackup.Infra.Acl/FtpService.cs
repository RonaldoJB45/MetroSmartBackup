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

            string nomeArquivo = host + "/" + Guid.NewGuid().ToString();
            string destino = "";

            var ftpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(nomeArquivo));
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftpRequest.Proxy = null;
            ftpRequest.UseBinary = true;
            ftpRequest.Credentials = new NetworkCredential(user, password);

            using (FileStream fs = File.OpenRead(destino))
            {
                using (Stream writer = ftpRequest.GetRequestStream())
                {
                    var buffer = new byte[1024 * 1024];
                    int totalReadBytesCount = 0;
                    int readBytesCount;

                    while ((readBytesCount = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        writer.Write(buffer, 0, readBytesCount);
                        totalReadBytesCount += readBytesCount;
                        var progress = totalReadBytesCount * 100.0 / fs.Length;
                    }
                }
            }
        }
    }
}
