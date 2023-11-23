using Newtonsoft.Json;
using System.IO;

namespace MetroBackup
{
    static class JsonUtil
    {
        public static dynamic ObterConteudo(string caminhoArquivo)
        {
            string texto = File.ReadAllText(caminhoArquivo);
            return JsonConvert.DeserializeObject(texto);
        }

        public static dynamic Listar<T>(string caminhoArquivo)
        {
            string texto = File.ReadAllText(caminhoArquivo);
            return JsonConvert.DeserializeObject<T>(texto);
        }

        public static void SalvarConteudo(string caminhoArquivo,
            dynamic conteudo, bool identar)
        {
            Formatting formatacao = identar ? Formatting.Indented : Formatting.None;
            string texto = JsonConvert.SerializeObject(conteudo, formatacao);
            File.WriteAllText(caminhoArquivo, texto);
        }
    }
}
