using Oracle.ManagedDataAccess.Client;
using System.Configuration;

namespace Core.Infra.Data.Connection
{
    public class Connection
    {
        private static readonly ConnectionStringSettings Conn = ConfigurationManager.ConnectionStrings["BASE"];
        private static readonly string ConnectionString = Conn.ConnectionString;
        private static OracleConnection _connection;

        public OracleConnection GetOpenConnection()
        {
            _connection = new OracleConnection(ConnectionString);
            _connection.Open();
            return _connection;
        }

        public void Desconection()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
