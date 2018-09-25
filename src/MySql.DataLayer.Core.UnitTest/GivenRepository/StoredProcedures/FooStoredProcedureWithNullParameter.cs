using MySql.DataLayer.Core.Attributes.StoredProcedureConfig.StoredProcedure;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySql.DataLayer.Core.UnitTest.GivenRepository.StoredProcedures
{
    [StoredProcedureName("FooStoredProcedureWithNullParameter")]
    public class FooStoredProcedureWithNullParameter : IDataStoredProcedure
    {
        public Guid id { get; set; }
        public string Description { get; set; }
    }
}
