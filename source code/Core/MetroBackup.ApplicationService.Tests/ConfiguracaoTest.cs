using MetroBackup.ApplicationService;
using MetroBackup.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MetroBackup.Application.Tests
{
    public class ConfiguracaoTest
    {
        private ConfiguracaoDto GerarConfiguracao(Guid? id, string descricao)
        {
            var configuracaoDto = new ConfiguracaoDto
            {
                Id = id ?? Guid.NewGuid(),
                Descricao = descricao ?? "Teste",
                Servidores = new List<ServidorDto>
                {
                    new ServidorDto
                    {
                        NomeBanco = "Teste",
                        IpBanco = "localhost",
                        PortaBanco = "1433",
                        UsuarioBanco = "sa",
                        SenhaBanco = "g3soft#2628"
                    }
                },
                Compactar = false,
                Compactador = "zip",
                Destinos = new string[] { "C:/Backup/Local1", "C:/Backup/Local2" }
            };

            return configuracaoDto;
        }

        [Fact]
        public void ObterTodos_Ok()
        {
            string caminho = "config_teste1.json";
            var fileContext = new FileContext(caminho, true);

            var configuracaoRepository = new ConfiguracaoRepository(fileContext);
            var configuracaoAppService = new ConfiguracaoAppService(configuracaoRepository);

            configuracaoAppService.Adicionar(GerarConfiguracao(null, null));
            configuracaoAppService.Adicionar(GerarConfiguracao(null, null));
            configuracaoAppService.Adicionar(GerarConfiguracao(null, null));

            var configuracoesDto = configuracaoAppService.ObterTodos();

            Assert.NotEmpty(configuracoesDto);
            Assert.Equal(3, configuracoesDto.Count);
        }

        [Fact]
        public void ObterPorId_Ok()
        {
            string caminho = "config_teste2.json";
            var fileContext = new FileContext(caminho, true);

            var configuracaoRepository = new ConfiguracaoRepository(fileContext);
            var configuracaoAppService = new ConfiguracaoAppService(configuracaoRepository);

            Guid id = Guid.NewGuid();

            configuracaoAppService.Adicionar(GerarConfiguracao(null, null));
            configuracaoAppService.Adicionar(GerarConfiguracao(id, null));
            configuracaoAppService.Adicionar(GerarConfiguracao(null, null));

            var configuracaoDto = configuracaoAppService.ObterPorId(id);

            Assert.NotNull(configuracaoDto);
            Assert.Equal(id, configuracaoDto.Id);
        }

        [Fact]
        public void Adicionar_Ok()
        {
            string caminho = "config_teste3.json";
            var fileContext = new FileContext(caminho, true);

            var configuracaoRepository = new ConfiguracaoRepository(fileContext);
            var configuracaoAppService = new ConfiguracaoAppService(configuracaoRepository);

            Guid id = Guid.NewGuid();
            var configuracaoDto = GerarConfiguracao(id, null);
            configuracaoAppService.Adicionar(configuracaoDto);
            configuracaoDto = configuracaoAppService.ObterPorId(id);

            Assert.NotNull(configuracaoDto);
            Assert.Equal(id, configuracaoDto.Id);
        }

        [Fact]
        public void Remover_Ok()
        {
            string caminho = "config_teste4.json";
            var fileContext = new FileContext(caminho, true);

            var configuracaoRepository = new ConfiguracaoRepository(fileContext);
            var configuracaoAppService = new ConfiguracaoAppService(configuracaoRepository);

            Guid id = Guid.NewGuid();

            configuracaoAppService.Adicionar(GerarConfiguracao(null, null));
            configuracaoAppService.Adicionar(GerarConfiguracao(id, null));
            configuracaoAppService.Adicionar(GerarConfiguracao(null, null));

            configuracaoAppService.Remover(id);

            var configuracoesDto = configuracaoAppService.ObterTodos();

            var configuracaoDto = configuracoesDto.FirstOrDefault(c => c.Id == id);

            Assert.Null(configuracaoDto);
            Assert.Equal(2, configuracoesDto.Count);
        }
    }
}