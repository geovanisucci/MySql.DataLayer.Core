using MySql.DataLayer.Core.Connection;
using Dapper;
using System.Text;
using System;

namespace MySql.DataLayer.Core.UnitTest.GivenRepository
{
    public class Database
    {
        public static void Create(out IMySqlConnectionFactory conn)
        {

            conn = new MySqlConnectionFactory(GetConnectionString(), GetDatabaseNameString());

            var sqlCreate = $"create database if not exists `mysqlcoretest`";

            using (var c = conn.GetAsync().Result)
            {
                c.ExecuteAsync(sqlCreate).Wait();
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

        public static void CreateFooTable(IMySqlConnectionFactory conn)
        {
            StringBuilder sqlCreate = new StringBuilder();
            sqlCreate.Append("CREATE TABLE IF NOT EXISTS `mysqlcoretest`.`foo`");
            sqlCreate.Append("(");
            sqlCreate.Append("`id` CHAR(36) NOT NULL ,");
            sqlCreate.Append("`Description` VARCHAR(100) NULL,");
            sqlCreate.Append(" PRIMARY KEY (`id`),");
            sqlCreate.Append("UNIQUE INDEX `ID_UNIQUE` (`id` ASC)");
            sqlCreate.Append(")");

            //Execute SQL Query
            using (var c = conn.GetAsync().Result)
            {
                c.ExecuteAsync(sqlCreate.ToString()).Wait();
            }
        }

        public static void InsertFooTable(IMySqlConnectionFactory conn, int numberOfRowsToInsert = 10)
        {
            StringBuilder sqlInsert = new StringBuilder();

            sqlInsert.Append("INSERT INTO `mysqlcoretest`.`foo` (id,Description) VALUES");

            bool first = true;
            for (int i = 1; i <= numberOfRowsToInsert; i++)
            {
                Guid guidInsert = Guid.NewGuid();
                if (first)
                {
                    sqlInsert.Append($"('{guidInsert.ToString()}','row {i.ToString()}')");

                    first = false;
                }
                else
                    sqlInsert.Append($",('{guidInsert.ToString()}','row {i.ToString()}')");
            }

            //Execute SQL Query
            using (var c = conn.GetAsync().Result)
            {
                c.ExecuteAsync(sqlInsert.ToString()).Wait();
            }
        }

        public static void CreateFooStoredProcedureWithParameter(IMySqlConnectionFactory conn)
        {
            StringBuilder sqlCreateStoredProcedure = new StringBuilder();

            sqlCreateStoredProcedure.AppendLine("CREATE PROCEDURE `mysqlcoretest`.FooStoredProcedureWithParameter (IN limitToSelect INT) ");
            sqlCreateStoredProcedure.AppendLine("BEGIN ");
            sqlCreateStoredProcedure.AppendLine("SELECT `id`,`Description` FROM `mysqlcoretest`.`foo` LIMIT limitToSelect; ");
            sqlCreateStoredProcedure.AppendLine("END ");

            //Execute SQL Query
            using (var c = conn.GetAsync().Result)
            {
                c.ExecuteAsync(sqlCreateStoredProcedure.ToString()).Wait();
            }
        }

        public static string GetConnectionString()
        {
            return "Server=127.0.0.1;DataBase=sys;Uid=root;Pwd=developer;Pooling=True;Allow User Variables=true";
        }
        public static string GetDatabaseNameString()
        {
            return "mysqlcoretest";
        }
    }
}