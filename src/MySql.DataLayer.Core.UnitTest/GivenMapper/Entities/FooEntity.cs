using System;
using MySql.DataLayer.Core.Attributes.EntityConfig.Table;

namespace MySql.DataLayer.Core.UnitTest.GivenMapper.Entities
{
    [TableName("foo")]
    public class FooEntity : IDataEntity
    {
        [PK("id")]
        public Guid id { get; set; }
        [ColumnName("Description")]
        public string Description { get; set; }
    }
}