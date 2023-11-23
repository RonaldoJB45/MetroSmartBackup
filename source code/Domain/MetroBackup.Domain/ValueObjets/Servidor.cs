namespace MetroBackup.Domain.ValueObjets
{
    public class Servidor
    {
        public Servidor(
            string endereco,
            string porta,
            string usuario,
            string senha)
        {
            Endereco = endereco;
            Porta = porta;
            Usuario = usuario;
            Senha = senha;
        }

        public string Endereco { get; private set; }
        public string Porta { get; private set; }
        public string Usuario { get; private set; }
        public string Senha { get; private set; }
    }
}
