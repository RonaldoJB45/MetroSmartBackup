using Newtonsoft.Json;
using System.IO;

namespace MetroBackup.Infra.Data
{
    public class FileContext
    {
        private readonly string _caminhoArquivo;

        public FileContext(string caminhoArquivo)
        {
            if (!File.Exists(caminhoArquivo))
            {
                File.Create(caminhoArquivo).Close();
            }

            _caminhoArquivo = caminhoArquivo;
        }

        public FileContext(string caminhoArquivo, bool removeArquivo)
        {
            if (removeArquivo)
                File.Delete(caminhoArquivo);

            if (!File.Exists(caminhoArquivo))
                File.Create(caminhoArquivo).Close();

            _caminhoArquivo = caminhoArquivo;
        }

        public dynamic ObterConteudo()
        {
            string texto = File.ReadAllText(_caminhoArquivo);
            return JsonConvert.DeserializeObject(texto);
        }

        public dynamic Listar<T>()
        {
            string texto = File.ReadAllText(_caminhoArquivo);
            return JsonConvert.DeserializeObject<T>(texto);
        }

        public void SalvarConteudo(dynamic conteudo, bool identar = true)
        {
            Formatting formatacao = identar ? Formatting.Indented : Formatting.None;
            string texto = JsonConvert.SerializeObject(conteudo, formatacao);
            File.WriteAllText(_caminhoArquivo, texto);
        }
    }
}
