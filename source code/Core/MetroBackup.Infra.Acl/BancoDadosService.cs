using MetroBackup.Domain.Interfaces;
using MetroBackup.Domain.ValueObjets;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace MetroBackup.Infra.Acl
{
    public class BancoDadosService : IBancoDadosService
    {
        public void TestarConexao(Servidor servidor)
        {
            using (MySqlConnection connection = new MySqlConnection(servidor.MySqlConnectionStrings))
            {
                try
                {
                    connection.Open();
                }
                catch (System.Exception ex)
                {
                    servidor.AddErro(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public IEnumerable<string> ObterTodos(Servidor servidor)
        {
            List<string> schemas = new List<string>();

            string comando = "select schema_name from information_schema.schemata";

            using (MySqlConnection connection = new MySqlConnection(servidor.MySqlConnectionStrings))
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
