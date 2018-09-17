namespace MySql.DataLayer.Core.UnitTest.GivenMapper.When_ToEntity_Is_Called
{
    using NUnit.Framework;
    using System;
    using MySql.DataLayer.Core.UnitTest.GivenMapper.DTO;
    using MySql.DataLayer.Core.Mapper;
    public class And_Map_Happens
    {
         GivenMapper.DTO.FooDTO _fooDTO;
        [SetUp]
        public void Setup()
        {
            _fooDTO = new GivenMapper.DTO.FooDTO()
            {
                id = Guid.NewGuid(),
                Description = "Test From DataEntity"
            };
        }

        [Test]
        public void Should_Return_DTO()
        {
            GivenMapper.Entities.FooEntity entity = null;
            entity = Map.ToEntity<FooDTO,GivenMapper.Entities.FooEntity>(_fooDTO); 

            Assert.IsNotNull(entity);

            Assert.IsTrue(entity.id == _fooDTO.id && entity.Description == _fooDTO.Description);
        }
    }
}