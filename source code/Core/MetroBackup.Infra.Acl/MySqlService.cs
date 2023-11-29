using MySql.Data.MySqlClient;
using System.IO;

namespace MetroBackup.Infra.Acl
{
    public class MySqlService
    {
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

                        };

                        mb.ExportToMemoryStream(ms);
                    }
                }
            }
        }
    }
}
