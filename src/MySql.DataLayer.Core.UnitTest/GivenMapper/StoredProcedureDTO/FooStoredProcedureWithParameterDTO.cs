using MySql.DataLayer.Core.Attributes.StoredProcedureConfig.StoredProcedure;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySql.DataLayer.Core.UnitTest.GivenMapper.StoredProcedureDTO
{
    public class FooStoredProcedureWithParameterDTO
    {
        public Guid id { get; set; }
        public string Description { get; set; }
    }
}
