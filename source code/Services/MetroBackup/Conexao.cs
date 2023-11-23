using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace MetroSmartBackup
{
    public class Conexao
    {
        private MySqlConnection connection;

        public Conexao()
        {

        }
        public Conexao(string server, string port, string database, string uid, string password)
        {
            this.Server = server;
            this.Port = port;
            this.DataBase = database;
            this.Uid = uid;
            this.Password = password;
        }

        public string Server { get; set; }
        public string Port { get; set; }
        public string DataBase { get; set; }
        public string Uid { get; set; }
        public string Password { get; set; }
        
        #region BancoDeDados
        private void Initialize()
        {            
            string connectionString;
            connectionString = "SERVER=" + this.Server + ";" + "PORT=" + this.Port + ";" + "DATABASE=" +
            this.DataBase + ";" + "UID=" + this.Uid + ";" + "PASSWORD=" + this.Password + ";";

            connection = new MySqlConnection(connectionString);
        }
        
        public DataTable ConectarBancos(DataTable dtBancos)
        {
            Initialize();

            try
            {
                if (OpenConnection())
                {
                    string comando = "select schema_name from information_schema.schemata";

                    MySqlCommand cmd = new MySqlCommand(comando, connection);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            dtBancos.Rows.Add(rdr["schema_name"].ToString());
                        }
                    }

                    this.CloseConnection();
                }
            }
            catch
            {
                throw;
            }

            return dtBancos;
        }
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                string msg = string.Empty;
                switch (ex.Number)
                {
                    case 0:
                        msg = "Sem conexão com o servidor! " + ex.Message;
                        break;
                    case 1045:
                        msg = "Usuario ou senha inválido, por favor tente novamente! " + ex.Message;
                        break;
                    default:
                        msg = ex.Message;
                        break;
                }

                throw new Exception(msg);
            }
        }
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
