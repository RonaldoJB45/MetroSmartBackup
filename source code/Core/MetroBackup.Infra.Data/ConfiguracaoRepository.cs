﻿using MetroBackup.Domain.Entities;
using MetroBackup.Domain.Interfaces;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace MetroBackup.Infra.Data
{
    public class ConfiguracaoRepository : IConfiguracaoRepository
    {
        public List<Configuracao> ObterTodos()
        {
            var conteudo = FileContext.ObterConteudo();
            var result = JsonConvert.DeserializeObject<List<Configuracao>>(Convert.ToString(conteudo));
            return result;
        }

        public Configuracao ObterPorId(Guid id)
        {
            var configuracoes = ObterTodos();
            var configuracao = configuracoes.FirstOrDefault(c => c.Id == id);
            return configuracao;
        }

        public void Adicionar(Configuracao configuracao)
        {
            var configuracoes = ObterTodos();
            configuracoes.Add(configuracao);
            FileContext.SalvarConteudo(configuracoes);
        }

        public void Alterar(Configuracao configuracao)
        {
            var configuracoes = ObterTodos();
            configuracoes.RemoveAll(c => c.Id == configuracao.Id);
            configuracoes.Add(configuracao);
            FileContext.SalvarConteudo(configuracoes);
        }
    }
}