namespace MetroBackup.ApplicationService.Restauracoes
{
    public class RestoreDto
    {
        public string NomeBanco { get; set; }
        public string IpBanco { get; set; }
        public string PortaBanco { get; set; }
        public string UsuarioBanco { get; set; }
        public string SenhaBanco { get; set; }
        public string Arquivo { get; set; }
    }
}