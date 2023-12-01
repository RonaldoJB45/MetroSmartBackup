using MetroBackup.Infra.Acl;
using MetroBackup.Infra.Data;
using System;
using System.Collections.Generic;
using Xunit;

namespace MetroBackup.Application.Tests
{
    public class BackupTests
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
                        NomeBanco = "app_db",
                        IpBanco = "localhost",
                        PortaBanco = "3306",
                        UsuarioBanco = "root",
                        SenhaBanco = "admin123"
                    }
                },
                Compactar = true,
                Compactador = "zip",
                Destinos = new string[] { "C:\\Backup\\Local1", "C:\\Backup\\Local2" }
            };

            return configuracaoDto;
        }

        [Fact]
        public void Executar_Ok()
        {
            string caminho = "config_teste5.json";
            var fileContext = new FileContext(caminho, true);

            var configuracaoRepository = new ConfiguracaoRepository(fileContext);
            var configuracaoAppService = new ConfiguracaoAppService(configuracaoRepository);

            Guid id = Guid.NewGuid();
            var configuracaoDto = GerarConfiguracao(id, null);
            configuracaoAppService.Adicionar(configuracaoDto);

            var backupService = new BackupService();
            var backupAppService = new BackupAppService(backupService, configuracaoRepository);

            backupAppService.Executar(id);

            Assert.NotNull(configuracaoDto);
        }
    }
}
