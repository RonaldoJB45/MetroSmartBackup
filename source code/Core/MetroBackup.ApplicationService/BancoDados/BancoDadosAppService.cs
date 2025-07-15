using MetroBackup.Domain.Interfaces.Services;
using MetroBackup.Domain.ValueObjets;
using System.Collections.Generic;

namespace MetroBackup.ApplicationService.BancoDados
{
    public class BancoDadosAppService : IBancoDadosAppService
    {
        private readonly IBancoDadosService _bancoDadosService;

        public BancoDadosAppService(IBancoDadosService bancoDadosService)
        {
            _bancoDadosService = bancoDadosService;
        }

        public void TestarConexao(BancoDadosDto bancoDadosDto)
        {
            var servidor = new Servidor(
                   bancoDadosDto.Endereco,
                   bancoDadosDto.Porta,
                   bancoDadosDto.Usuario,
                   bancoDadosDto.Senha,
                   bancoDadosDto.NomeBanco);

            _bancoDadosService.TestarConexao(servidor);
            bancoDadosDto.Erro = servidor.Erro;
        }

        public IEnumerable<string> ObterTodos(BancoDadosDto bancoDadosDto)
        {
            return _bancoDadosService.ObterTodos(new Servidor(
                bancoDadosDto.Endereco,
                bancoDadosDto.Porta,
                bancoDadosDto.Usuario,
                bancoDadosDto.Senha,
                bancoDadosDto.NomeBanco));
        }
    }
}
