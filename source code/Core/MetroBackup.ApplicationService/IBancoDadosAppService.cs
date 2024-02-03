using System.Collections.Generic;

namespace MetroBackup.ApplicationService
{
    public interface IBancoDadosAppService
    {
        IEnumerable<string> ObterTodos(
            string server,
            string port,
            string dataBase,
            string uid,
            string password);
    }
}
