using Dapper;
using MySql.DataLayer.Core.Connection;
using MySql.DataLayer.Core.DatabasePatcher;
using MySql.DataLayer.Core.UnitTest.GivenRepository;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MySql.DataLayer.Core.UnitTest.GivenDatabasePatcher.When_Migration_Is_Called
{
    public class And_Migrations_Exec
    {
        Migration migration;
        IMySqlConnectionFactory _connectionFactory;

        [SetUp]
        public void Setup()
        {
            if (Environment.GetEnvironmentVariable("MYSQL_HOST") == null)
            {
                Environment.SetEnvironmentVariable("MYSQL_HOST", "192.168.99.100");
            }
            Database.Create(out _connectionFactory);
            migration = new Migration(Database.GetConnectionString(), Database.GetDatabaseNameString());
        }

        [Test]
        public void Should_Return_Success()
        {
            var scriptsFolder = Path.Combine(Path.GetDirectoryName (this.GetType ().Assembly.Location), "GivenDatabasePatcher", "Scripts");

            
            //Path.Combine (, "GivenDatabasePatcher", "Scripts", migration.GetType ().Name + suffix + ".sql"));
            migration.ExecuteMigrations(scriptsFolder);

            string sqlCommand = $"SELECT IFNULL((SELECT 1 FROM information_schema.tables WHERE table_schema = '{Database.GetDatabaseNameString()}' AND table_name = '_DatabaseMigrations' LIMIT 1),0) as TableExist;";

            int result = 0;
            //Execute SQL Query
            using (var c = _connectionFactory.GetAsync().Result)
            {
                result = c.ExecuteScalarAsync<int>(sqlCommand.ToString()).Result;
            }
            
            Assert.IsTrue(result == 1);
        }

        [TearDown]
        public virtual void TearDown()
        {
            Database.Drop(_connectionFactory);
        }
    }
}
