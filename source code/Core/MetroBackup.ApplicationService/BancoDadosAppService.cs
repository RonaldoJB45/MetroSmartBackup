using MetroBackup.Domain.Interfaces;
using System.Collections.Generic;

namespace MetroBackup.ApplicationService
{
    public class BancoDadosAppService : IBancoDadosAppService
    {
        private readonly IBancoDadosService _bancoDadosService;

        public BancoDadosAppService(IBancoDadosService bancoDadosService)
        {
            _bancoDadosService = bancoDadosService;
        }

        public IEnumerable<string> ObterTodos(
            string server,
            string port,
            string dataBase,
            string uid,
            string password)
        {
            return _bancoDadosService.ObterTodos(
                server,
                port,
                dataBase,
                uid,
                password);
        }
    }
}
