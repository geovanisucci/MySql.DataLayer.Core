using MySql.DataLayer.Core.Mapper;
using MySql.DataLayer.Core.UnitTest.GivenMapper.StoredProcedureDTO;
using MySql.DataLayer.Core.UnitTest.GivenMapper.StoredProcedures;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySql.DataLayer.Core.UnitTest.GivenMapper.When_ToStoredProcedure_Is_Called
{
    public class And_Map_Happens
    {
        FooStoredProcedureWithParameterDTO _fooDTO;
        [SetUp]
        public void Setup()
        {
            _fooDTO = new FooStoredProcedureWithParameterDTO()
            {
                id = Guid.NewGuid(),
                Description = "Test to DataStoredProcedure"
            };
        }

        [Test]
        public void Should_Return_DataStoredProcedure()
        {
            FooStoredProcedureWithParameter storedProcedure = null;
            storedProcedure = Map.ToStoredProcedure<FooStoredProcedureWithParameterDTO, FooStoredProcedureWithParameter>(_fooDTO);

            Assert.IsNotNull(storedProcedure);

            Assert.IsTrue(storedProcedure.id == _fooDTO.id && storedProcedure.Description == _fooDTO.Description);
        }
    }
}
