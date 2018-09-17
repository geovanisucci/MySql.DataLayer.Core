using NUnit.Framework;
using MySql.DataLayer.Core.Repository;
using MySql.DataLayer.Core;
using MySql.DataLayer.Core.Connection;
using MySql.DataLayer.Core.Attributes.EntityConfig.Table;
using System.Collections.Generic;
using System;
using MySql.DataLayer.Core.Attributes.StoredProcedureConfig.StoredProcedure;
using MySql.DataLayer.Core.UnitTest.GivenRepository.StoredProcedures;
using System.Threading.Tasks;

namespace MySql.DataLayer.Core.UnitTest.GivenRepository.When_StoredProcedure_Is_Called
{
    public class And_StoredProcedure_Exec
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
            Database.InsertFooTable(_connectionFactory, 100);
            Database.CreateFooStoredProcedureWithParameter(_connectionFactory);

            _repository = new Repository(_connectionFactory);
        }

        [Test]
        public void Should_Return_Success()
        {
            List<QueryParameter> queryParameters = new List<QueryParameter>();
            queryParameters.Add(new QueryParameter
            {
                ParameterName = "limitToSelect",
                ParameterValue = 50
            });

            List<FooStoredProcedureWithParameter> resultList =
                                           _repository.ExecuteStoredProcedure<FooStoredProcedureWithParameter>(queryParameters.ToArray()).Result;

            Assert.IsTrue(resultList != null);
        }

        [TearDown]
        public virtual void TearDown()
        {
            Database.Drop(_connectionFactory);
        }

    }
}
