namespace MySql.DataLayer.Core.UnitTest.GivenRepository.When_CreateAsync_Is_Called
{
    using NUnit.Framework;
    using MySql.DataLayer.Core.Repository;
    using MySql.DataLayer.Core;
    using MySql.DataLayer.Core.Connection;
    using MySql.DataLayer.Core.Attributes.EntityConfig.Table;
    using System.Collections.Generic;
    using System;
    using MySql.DataLayer.Core.Attributes.StoredProcedureConfig.StoredProcedure;
    using System.Threading.Tasks;
    using Dapper;
    using MySql.DataLayer.Core.UnitTest.GivenRepository.Entities;

    public class And_Create_Happens
    {

        IMySqlConnectionFactory _connectionFactory;
        Repository _repository;

        [SetUp]
        public void Setup()
        {
            if (Environment.GetEnvironmentVariable("MYSQL_HOST") == null)
            {
                Environment.SetEnvironmentVariable("MYSQL_HOST", "192.168.99.100");
            }
            Database.Create(out _connectionFactory); 
            Database.CreateFooTable(_connectionFactory);

            _repository = new Repository(_connectionFactory);
        }

        [Test]
        public void Should_Return_Success()
        {
            int result = 0;

            string sqlCommand = "SELECT IFNULL((SELECT 1 FROM information_schema.tables WHERE table_schema = 'mysqlcoretest' AND table_name = 'foo' LIMIT 1),0) as TableExist;";

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