using MySql.DataLayer.Core.Connection;
using MySql.DataLayer.Core.UnitTest.GivenRepository.Views;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySql.DataLayer.Core.UnitTest.GivenRepository.When_View_Is_Called
{
    public class And_View_Exec
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
            Database.CreateFooView(_connectionFactory);

            _repository = new Repository(_connectionFactory);
        }

        [Test]
        public void Should_Return_Success()
        {
            List<FooView> resultList = _repository.ExecuteViewAsync<FooView>().Result;

            Assert.IsTrue(resultList != null);
        }

        [Test]
        public void With_Columns_Should_Return_Success()
        {
            //Creating list of Columns
            List<ColumnTable> columns = new List<ColumnTable>();
            columns.Add(new ColumnTable("Description"));
            columns.Add(new ColumnTable("Concat"));

            List<FooView> resultList = _repository.ExecuteViewAsync<FooView>(columns.ToArray()).Result;

            Assert.IsTrue(resultList[0].id == Guid.Empty
                          && resultList[0].Description != null 
                          && resultList[0].Concat != null);
        }

        [Test]
        public void With_Conditions_Should_Return_Success()
        {
            //Creating list of Conditions
            List<ConditionSearch> conditions = new List<ConditionSearch>();
            conditions.Add(new ConditionSearch
            {
                Condition = Condition.Where,
                Operator = ConditionOperator.Equal,
                ParameterName = "Description",
                ParameterValue = "row 1"
            });

            List<FooView> resultList = _repository.ExecuteViewAsync<FooView>(null,conditions.ToArray()).Result;

            Assert.IsTrue(resultList[0].Description == "row 1");
        }

        [Test]
        public void With_Columns_And_Conditions_Should_Return_Success()
        {
            //Creating list of Columns
            List<ColumnTable> columns = new List<ColumnTable>();
            columns.Add(new ColumnTable("Description"));
            columns.Add(new ColumnTable("Concat"));

            //Creating list of Conditions
            List<ConditionSearch> conditions = new List<ConditionSearch>();
            conditions.Add(new ConditionSearch
            {
                Condition = Condition.Where,
                Operator = ConditionOperator.Equal,
                ParameterName = "Description",
                ParameterValue = "row 1"
            });            

            List<FooView> resultList = _repository.ExecuteViewAsync<FooView>(columns.ToArray(),conditions.ToArray()).Result;

            Assert.IsTrue(resultList[0].id == Guid.Empty
                          && resultList[0].Description == "row 1"
                          && resultList[0].Concat != null);
        }

        [TearDown]
        public virtual void TearDown()
        {
            Database.Drop(_connectionFactory);
        }
    }
}
