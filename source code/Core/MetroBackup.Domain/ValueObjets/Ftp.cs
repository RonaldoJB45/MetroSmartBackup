namespace MetroBackup.Domain.ValueObjets
{
    public class Ftp
    {
        public Ftp(
            bool utilizar,
            string host,
            string user,
            string password)
        {
            Utilizar = utilizar;
            Host = host;
            User = user;
            Password = password;
        }

        public bool Utilizar { get; private set; }
        public string Host { get; private set; }
        public string User { get; private set; }
        public string Password { get; private set; }
    }
}
