namespace MySql.DataLayer.Core.UnitTest.GivenMapper.When_ToEntity_Is_Called
{
    using NUnit.Framework;
    using System;
    using MySql.DataLayer.Core.UnitTest.GivenMapper.DTO;
    using MySql.DataLayer.Core.Mapper;
    using MySql.DataLayer.Core.UnitTest.GivenMapper.Entities;

    public class And_Map_Happens
    {
         FooDTO _fooDTO;
        [SetUp]
        public void Setup()
        {
            _fooDTO = new FooDTO()
            {
                id = Guid.NewGuid(),
                Description = "Test to DataEntity"
            };
        }

        [Test]
        public void Should_Return_DataEntity()
        {
            FooEntity entity = null;
            entity = Map.ToEntity<FooDTO,FooEntity>(_fooDTO); 

            Assert.IsNotNull(entity);

            Assert.IsTrue(entity.id == _fooDTO.id && entity.Description == _fooDTO.Description);
        }
    }
}