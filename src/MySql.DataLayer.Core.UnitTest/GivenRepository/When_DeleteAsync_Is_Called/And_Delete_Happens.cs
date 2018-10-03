using MySql.DataLayer.Core.Connection;
using MySql.DataLayer.Core.UnitTest.GivenMapper.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySql.DataLayer.Core.UnitTest.GivenRepository.When_DeleteAsync_Is_Called
{
    public class And_Delete_Happens
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

            _repository = new Repository(_connectionFactory);
        }

        [Test]
        public void Delete_ByEntity_Should_Success()
        {

            var entityToDelete = _repository.GetAllAsync().Result.FirstOrDefault();

            Assert.IsNotNull(entityToDelete);
            
            var result = _repository.RemoveAsync(entityToDelete).Result;

            Assert.IsTrue(result == 1);
        }

        [TearDown]
        public virtual void TearDown()
        {
            Database.Drop(_connectionFactory);
        }
    }
}
