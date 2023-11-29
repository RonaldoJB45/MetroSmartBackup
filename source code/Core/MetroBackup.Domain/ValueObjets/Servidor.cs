namespace MetroBackup.Domain.ValueObjets
{
    public class Servidor
    {
        public Servidor(
            string nomeBanco,
            string endereco,
            string porta,
            string usuario,
            string senha)
        {
            NomeBanco = nomeBanco;
            Endereco = endereco;
            Porta = porta;
            Usuario = usuario;
            Senha = senha;
        }

        public string NomeBanco { get; set; }
        public string Endereco { get; private set; }
        public string Porta { get; private set; }
        public string Usuario { get; private set; }
        public string Senha { get; private set; }

        public string MySqlConnectionStrings => "server=" + Endereco +
                                                ";port=" + Porta +
                                                ";user id=" + Usuario +
                                                ";Password=" + Senha +
                                                ";database=" + NomeBanco +
                                                ";Convert Zero Datetime=True;pooling=false; Allow User Variables=True;";
    }
}
