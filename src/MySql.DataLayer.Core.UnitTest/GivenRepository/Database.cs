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

            conn = new MySqlConnectionFactory(GetConnectionString(),GetDatabaseNameString());

            var sqlCreate = $"create database if not exists `mysqlcoretest`";

            using (var c = conn.GetAsync("sys").Result)
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
            sqlCreate.Append("`CreatedAt` DateTime NULL,");
            sqlCreate.Append(" PRIMARY KEY (`id`),");
            sqlCreate.Append("UNIQUE INDEX `ID_UNIQUE` (`id` ASC)");
            sqlCreate.Append(")");

            //Execute SQL Query
            using (var c = conn.GetAsync(GetDatabaseNameString()).Result)
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
            using (var c = conn.GetAsync(GetDatabaseNameString()).Result)
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
            using (var c = conn.GetAsync(GetDatabaseNameString()).Result)
            {
                c.ExecuteAsync(sqlCreateStoredProcedure.ToString()).Wait();
            }
        }
        public static void CreateFooStoredProcedureWithoutParameter(IMySqlConnectionFactory conn)
        {
            StringBuilder sqlCreateStoredProcedure = new StringBuilder();

            sqlCreateStoredProcedure.AppendLine("CREATE PROCEDURE `mysqlcoretest`.FooStoredProcedureWithoutParameter() ");
            sqlCreateStoredProcedure.AppendLine("BEGIN ");
            sqlCreateStoredProcedure.AppendLine("SELECT `id`,`Description` FROM `mysqlcoretest`.`foo`; ");
            sqlCreateStoredProcedure.AppendLine("END ");

            //Execute SQL Query
            using (var c = conn.GetAsync(GetDatabaseNameString()).Result)
            {
                c.ExecuteAsync(sqlCreateStoredProcedure.ToString()).Wait();
            }
        }
        public static void CreateFooStoredProcedureWithNullParameter(IMySqlConnectionFactory conn)
        {
            StringBuilder sqlCreateStoredProcedure = new StringBuilder();

            sqlCreateStoredProcedure.AppendLine("CREATE PROCEDURE `mysqlcoretest`.FooStoredProcedureWithNullParameter (IN idValue CHAR(36), IN descriptionValue VARCHAR(200)) ");
            sqlCreateStoredProcedure.AppendLine("BEGIN ");
            sqlCreateStoredProcedure.AppendLine("INSERT INTO `mysqlcoretest`.`foo` (`id`,`Description`) VALUEs(idValue,descriptionValue); ");
            sqlCreateStoredProcedure.AppendLine("END ");

            //Execute SQL Query
            using (var c = conn.GetAsync(GetDatabaseNameString()).Result)
            {
                c.ExecuteAsync(sqlCreateStoredProcedure.ToString()).Wait();
            }
        }


        public static void CreateFooView(IMySqlConnectionFactory conn)
        {
            StringBuilder sqlCreateView = new StringBuilder();

            sqlCreateView.AppendLine("CREATE VIEW `mysqlcoretest`.FooView ");
            sqlCreateView.AppendLine("AS ");
            sqlCreateView.AppendLine("SELECT `id`,`Description`,CONCAT('Foo --> ',Description) as Concat FROM `mysqlcoretest`.`foo`");

            //Execute SQL Query
            using (var c = conn.GetAsync(GetDatabaseNameString()).Result)
            {
                c.ExecuteAsync(sqlCreateView.ToString()).Wait();
            }
        }

        public static string GetConnectionString()
        {
            return  $@"Server={Environment.GetEnvironmentVariable("MYSQL_HOST")};User=root;Password=admin;";
        }
        public static string GetDatabaseNameString()
        {
            return "mysqlcoretest";
        }
    }
}