using System.Collections.Generic;

namespace MetroBackup.ApplicationService.BancoDados
{
    public interface IBancoDadosAppService
    {
        IEnumerable<string> ObterTodos(BancoDadosDto bancoDadosDto);
        void TestarConexao(BancoDadosDto bancoDadosDto);
    }
}
