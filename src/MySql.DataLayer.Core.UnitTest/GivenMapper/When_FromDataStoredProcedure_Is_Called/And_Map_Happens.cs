using MySql.DataLayer.Core.Mapper;
using MySql.DataLayer.Core.UnitTest.GivenMapper.StoredProcedureDTO;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySql.DataLayer.Core.UnitTest.GivenMapper.When_FromDataStoredProcedure_Is_Called
{
   public class And_Map_Happens
    {
        StoredProcedures.FooStoredProcedureWithParameter _fooStoredProcedure;
        [SetUp]
        public void Setup()
        {
            _fooStoredProcedure = new StoredProcedures.FooStoredProcedureWithParameter()
            {
                id = Guid.NewGuid(),
                Description = "Test From DataStoredProcedure"
            };
        }

        [Test]
        public void Should_Return_StoredProcedureDTO()
        {
            FooStoredProcedureWithParameterDTO dto = null;
            dto = Map.FromDataStoredProcedure<FooStoredProcedureWithParameterDTO, StoredProcedures.FooStoredProcedureWithParameter>(_fooStoredProcedure);

            Assert.IsNotNull(dto);

            Assert.IsTrue(dto.id == _fooStoredProcedure.id && dto.Description == _fooStoredProcedure.Description);
        }
    }
}
