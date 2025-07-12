using MetroBackup.Domain.Entities;
using MetroBackup.Domain.Interfaces;
using MetroBackup.Domain.ValueObjets;
using MySql.Data.MySqlClient;

namespace MetroBackup.Infra.Acl
{
    public class RestoreService : IRestoreService
    {
        private readonly IProgressReporter _progressReporter;

        public RestoreService(IProgressReporter progressReporter)
        {
            _progressReporter = progressReporter;
        }

        public void Restaurar(Servidor servidor)
        {
            string connection = servidor.MySqlConnectionStrings;
            string file = servidor.CaminhoBackup;

            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();

                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        mb.ImportProgressChanged += (sender, e) =>
                        {
                            double porcentagem = e.PercentageCompleted;

                            if (porcentagem < 100)
                                _progressReporter.ReportProgress(porcentagem, "Restaurando banco! Aguarde...");
                            else
                                _progressReporter.ReportProgress(porcentagem, "Banco restaurado com sucesso!");
                        };
                        mb.ImportFromFile(file);
                    }
                }
            }
        }
    }
}