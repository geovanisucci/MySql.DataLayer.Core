namespace MySql.DataLayer.Core.UnitTest.GivenMapper.When_FromDataEntity_Is_Called
{
    using NUnit.Framework;
    using System;
    using MySql.DataLayer.Core.UnitTest.GivenMapper.DTO;
    using MySql.DataLayer.Core.Mapper;

    public class And_Map_Happens
    {
        GivenMapper.Entities.FooEntity _fooEntity;
        [SetUp]
        public void Setup()
        {
            _fooEntity = new Entities.FooEntity()
            {
                id = Guid.NewGuid(),
                Description = "Test From DataEntity"
            };
        }

        [Test]
        public void Should_Return_DTO()
        {
            FooDTO dto = null;
            dto = Map.FromDataEntity<FooDTO, GivenMapper.Entities.FooEntity>(_fooEntity);

            Assert.IsNotNull(dto);

            Assert.IsTrue(dto.id == _fooEntity.id && dto.Description == _fooEntity.Description);
        }

    }
}