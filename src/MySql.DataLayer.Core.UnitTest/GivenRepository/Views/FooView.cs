using MySql.DataLayer.Core.Attributes.ViewConfig.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySql.DataLayer.Core.UnitTest.GivenRepository.Views
{
    [ViewName("FooView")]
    public class FooView : IDataView
    {
        public Guid id { get; set; }
        public string Description { get; set; }
        public string Concat { get; set; }
    }
}
