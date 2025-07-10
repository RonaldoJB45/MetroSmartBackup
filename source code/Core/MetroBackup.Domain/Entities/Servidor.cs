namespace MetroBackup.Domain.ValueObjets
{
    public class Servidor
    {
        public Servidor(
            string endereco,
            string porta,
            string usuario,
            string senha,
            string nomeBanco = null)
        {
            NomeBanco = nomeBanco ?? "information_schema.SCHEMATA";
            Endereco = endereco;
            Porta = porta;
            Usuario = usuario;
            Senha = senha;
        }

        public string NomeBanco { get; private set; }
        public string Endereco { get; private set; }
        public string Porta { get; private set; }
        public string Usuario { get; private set; }
        public string Senha { get; private set; }

        public string CaminhoBackup { get; private set; }
        public void AddCaminhoBackup(string caminhoBackup) => CaminhoBackup = caminhoBackup;

        public string Erro { get; private set; }
        public void AddErro(string erro) => Erro = erro;

        private string Database => NomeBanco == "information_schema.SCHEMATA" ? null : ";database=" + NomeBanco;

        public string MySqlConnectionStrings => "server=" + Endereco +
                                                ";port=" + Porta +
                                                ";user id=" + Usuario +
                                                ";Password=" + Senha +
                                                Database +
                                                ";Convert Zero Datetime=True;pooling=false; Allow User Variables=True;";
    }
}
