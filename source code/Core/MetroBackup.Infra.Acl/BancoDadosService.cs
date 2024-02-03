using MetroBackup.Domain.Interfaces;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace MetroBackup.Infra.Acl
{
    public class BancoDadosService : IBancoDadosService
    {
        public IEnumerable<string> ObterTodos(
            string server,
            string port,
            string dataBase,
            string uid,
            string password)
        {
            List<string> schemas = new List<string>();

            string connectionString = $"SERVER={server};PORT={port};DATABASE={dataBase};UID={uid};PASSWORD={password}";

            string comando = "select schema_name from information_schema.schemata";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand(comando, connection);

                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        string schema = rdr["schema_name"].ToString();
                        schemas.Add(schema);
                    }
                }

                connection.Close();
            }

            return schemas;
        }
    }
}
