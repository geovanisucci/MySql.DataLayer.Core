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
    public class And_Create_Happens
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Should_Return_Success()
        {
            var test1 = 1;
            var test2 = 1;

            Assert.IsTrue(test1 == test2);
        }

    }
}