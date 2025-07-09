using MetroBackup.Domain.ValueObjets;
using System.Collections.Generic;

namespace MetroBackup.Domain.Interfaces
{
    public interface IBancoDadosService
    {
        void TestarConexao(Servidor servidor);
        IEnumerable<string> ObterTodos(Servidor servidor);
    }
}
