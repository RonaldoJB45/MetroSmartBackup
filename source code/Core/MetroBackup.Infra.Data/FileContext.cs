using Newtonsoft.Json;
using System.IO;

namespace MetroBackup.Infra.Data
{
    static class FileContext
    {
        private static string caminhoArquivo { get; set; }

        public static void DefinirCaminhoArquivo(string caminho) => caminhoArquivo = caminho;

        public static dynamic ObterConteudo()
        {
            string texto = File.ReadAllText(caminhoArquivo);
            return JsonConvert.DeserializeObject(texto);
        }

        public static dynamic Listar<T>()
        {
            string texto = File.ReadAllText(caminhoArquivo);
            return JsonConvert.DeserializeObject<T>(texto);
        }

        public static void SalvarConteudo(dynamic conteudo, bool identar = true)
        {
            Formatting formatacao = identar ? Formatting.Indented : Formatting.None;
            string texto = JsonConvert.SerializeObject(conteudo, formatacao);
            File.WriteAllText(caminhoArquivo, texto);
        }
    }
}
