using MetroBackup.Domain.Interfaces;
using MetroBackup.Domain.Services;
using MetroBackup.Domain.ValueObjets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MetroBackup.Infra.Acl.Tests
{
    [TestClass]
    public class RestoreTests
    {
        [TestMethod]
        public void Restore_Ok()
        {
            IProgressReporter progressReporter = new ProgressReporter();
            var restoreService = new RestoreService(progressReporter);
            var servidor = new Servidor(
                "127.0.0.1",
                "3306",
                "root",
                "root",
                "test2db",
                @"C:\Users\Ronaldo\Desktop\backup linux\testdb_12072025134513943.sql");
            restoreService.Restaurar(servidor);

            Assert.ThrowsException<Exception>(() =>
            {
                restoreService.Restaurar(servidor);
            });
        }
    }
}
