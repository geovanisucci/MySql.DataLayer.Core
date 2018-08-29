using MySql.DataLayer.Core.Connection;
using Dapper;

namespace MySql.DataLayer.Core.UnitTest.GivenRepository
{
    public class Database
    {
        public static void Create(out IMySqlConnectionFactory conn)
        {

            conn = new MySqlConnectionFactory(GetConnectionString());

            var sqlCreate = $"create database if not exists `mysqlcoretest`";

            using (var c = conn.GetAsync().Result)
            {
                c.ExecuteAsync(sqlCreate).Wait();

                c.ChangeDatabase("mysqlcoretest");
            }

        }

        public static void Drop(IMySqlConnectionFactory conn)
        {
            var sqlDrop = $"drop database if exists `mysqlcoretest`";

            using (var c = conn.GetAsync().Result)
            {
                c.ExecuteAsync(sqlDrop).Wait();
            }

        }


        public static string GetConnectionString()
        {
            return "Server=localhost;DataBase=sys;Uid=root;Pwd=developer;Pooling=True;Allow User Variables=true";
        }
    }
}