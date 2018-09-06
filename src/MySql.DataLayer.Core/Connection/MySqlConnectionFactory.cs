namespace MySql.DataLayer.Core.Connection
{
    using System.Threading.Tasks;
    using MySql.Data.MySqlClient;
    /// <summary>
    /// Implmementation of IMySqlConnectionFactory with the methods
    /// that will provides the MySql Connection operations.
    /// </summary>
    public class MySqlConnectionFactory : IMySqlConnectionFactory
    {
        private string _connectionString;
        private string _databaseName;

        /// <summary>
        /// The constructor of the MySqlConnectionFactory.
        /// </summary>
        /// <param name="connectionString"></param>
        public MySqlConnectionFactory(string connectionString, string databaseName = null)
        {
            _connectionString = connectionString;
            _databaseName = databaseName;
        }
        /// <summary>
        /// Open connection asynchronous.
        /// </summary>
        /// <returns>MySqlConnection completed.</returns>
        public async Task<MySqlConnection> GetAsync()
        {
            MySqlConnection conn = new MySqlConnection(_connectionString);

            await conn.OpenAsync();

            return conn;
        }

        public string GetDatabaseName()
        {
            string databaseNameResult = null;

            if (_databaseName != null)
                databaseNameResult = $"`{_databaseName}`";

            return databaseNameResult;
        }
    }
}