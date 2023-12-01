using MetroBackup.Domain.Interfaces;
using MySql.Data.MySqlClient;
using System.IO;

namespace MetroBackup.Infra.Acl
{
    public class MySqlService
    {
        private readonly IProgressReporter _progressReporter;

        public MySqlService(IProgressReporter progressReporter)
        {
            _progressReporter = progressReporter;
        }

        public void BackupToMemoryStream(MemoryStream ms, string constring)
        {
            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();

                        mb.ExportProgressChanged += (object sender, ExportProgressArgs e) =>
                        {
                            double _totalRowsInAllTables = e.TotalRowsInAllTables;
                            double _currentRowInAllTables = e.CurrentRowIndexInAllTables;

                            double porcentagem = (_currentRowInAllTables * 100) / _totalRowsInAllTables;

                            double _resultado = System.Math.Round(porcentagem, 2);

                            _progressReporter.ReportProgress((double)_resultado);
                        };

                        mb.ExportToMemoryStream(ms);
                    }
                }
            }
        }
    }
}
