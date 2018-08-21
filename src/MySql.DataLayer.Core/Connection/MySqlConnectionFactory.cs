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
        /// <summary>
        /// The constructor of the MySqlConnectionFactory.
        /// </summary>
        /// <param name="connectionString"></param>
        public MySqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
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
    }
}