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

                        mb.ExportInfo.GetTotalRowsBeforeExport = true;
                        mb.ExportProgressChanged += (object sender, ExportProgressArgs e) =>
                        {
                            int progress = 0;

                            if (e.TotalTables > 0)
                            {
                                double tableProgress = (double)e.CurrentTableIndex / e.TotalTables;

                                double rowProgress = (e.TotalRowsInCurrentTable > 0)
                                    ? (double)e.CurrentRowIndexInCurrentTable / e.TotalRowsInCurrentTable
                                    : 0;

                                double combinedProgress = tableProgress + (rowProgress / e.TotalTables);

                                progress = (int)(combinedProgress * 100);
                                if (progress > 100) progress = 100;
                            }

                            _progressReporter?.ReportProgress(progress);
                        };

                        mb.ExportToMemoryStream(ms);
                    }
                }
            }
        }

    }
}
