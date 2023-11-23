using MetroBackup.Domain.Entities;
using System.Collections.Generic;

namespace MetroBackup.Domain.Services
{
    public class ConfiguracaoService
    {
        private readonly List<Configuracao> _configuracoes = new List<Configuracao>();
        public IReadOnlyCollection<Configuracao> Configuracoes => _configuracoes.AsReadOnly();
        public void AdicionarConfiguracao(Configuracao configuracao) => _configuracoes.Add(configuracao);
    }
}
