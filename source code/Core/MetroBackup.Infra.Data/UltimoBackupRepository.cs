using MetroBackup.Domain.Interfaces.Repository;
using MetroBackup.Domain.Entities;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace MetroBackup.Infra.Data
{
    public class UltimoBackupRepository : IUltimoBackupRepository
    {
        private readonly FileContext _fileContext;

        public UltimoBackupRepository(FileContext fileContext)
        {
            _fileContext = fileContext;
        }

        public List<UltimoBackup> ObterTodos()
        {
            var conteudo = _fileContext.ObterConteudo();

            if (conteudo == null) return new List<UltimoBackup>();

            var result = JsonConvert.DeserializeObject<List<UltimoBackup>>(Convert.ToString(conteudo));
            return result;
        }

        public void Adicionar(UltimoBackup ultimoBackup)
        {
            var ultimosBackps = ObterTodos();
            ultimosBackps.Add(ultimoBackup);
            _fileContext.SalvarConteudo(ultimosBackps);
        }

        public void Alterar(UltimoBackup ultimoBackup)
        {
            var ultimosBackups = ObterTodos();
            ultimosBackups.RemoveAll(b =>
                b.ConfiguracaoId == ultimoBackup.ConfiguracaoId && b.Origem == ultimoBackup.Origem);
            ultimosBackups.Add(ultimoBackup);
            _fileContext.SalvarConteudo(ultimosBackups);
        }
    }
}