using System.Collections.Generic;

namespace MetroBackup.Domain.Interfaces
{
    public interface IBancoDadosService
    {
        IEnumerable<string> ObterTodos(
            string server,
            string port,
            string dataBase,
            string uid,
            string password);
    }
}
