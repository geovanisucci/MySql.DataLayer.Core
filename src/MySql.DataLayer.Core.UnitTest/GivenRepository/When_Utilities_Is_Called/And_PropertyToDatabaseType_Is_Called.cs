using MySql.DataLayer.Core.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MySql.DataLayer.Core.UnitTest.GivenRepository.When_Utilities_Is_Called
{
    public class And_PropertyToDatabaseType_Is_Called
    {
        [Test]
        public void Should_Return_IsTrue_DbTypeString()
        {
            string s = "string";

            DbType result = Utilities.PropertyToDatabaseType(s);

            Assert.IsTrue(result == DbType.String);
        }

        [Test]
        public void Should_Return_IsTrue_DbTypeInt32()
        {
            int i = 1;

            DbType result = Utilities.PropertyToDatabaseType(i);

            Assert.IsTrue(result == DbType.Int32);
        }

        [Test]
        public void Should_Return_IsTrue_DbTypeDecimal()
        {
            decimal d = 1000.0M;

            DbType result = Utilities.PropertyToDatabaseType(d);

            Assert.IsTrue(result == DbType.Decimal);
        }


        [Test]
        public void Should_Return_IsTrue_DbTypeDatetime()
        {
            DateTime dt = DateTime.Now;

            DbType result = Utilities.PropertyToDatabaseType(dt);

            Assert.IsTrue(result == DbType.DateTime);
        }

        [Test]
        public void Should_Return_IsTrue_DbTypeGuid()
        {
            Guid g = Guid.NewGuid();

            DbType result = Utilities.PropertyToDatabaseType(g);

            Assert.IsTrue(result == DbType.Guid);
        }

        [Test]
        public void Should_Return_IsTrue_Null_DbTypeInt32()
        {
            int? i = null;
            
            DbType result = Utilities.PropertyToDatabaseType(i,typeof(int));

            Assert.IsTrue(result == DbType.Int32);
        }

        [Test]
        public void Should_Return_IsTrue_Null_DbTypeDecimal()
        {
            decimal? d = null;

            DbType result = Utilities.PropertyToDatabaseType(d, typeof(decimal));

            Assert.IsTrue(result == DbType.Decimal);
        }


        [Test]
        public void Should_Return_IsTrue_Null_DbTypeDatetime()
        {
            DateTime? dt = null;

            DbType result = Utilities.PropertyToDatabaseType(dt, typeof(DateTime));

            Assert.IsTrue(result == DbType.DateTime);
        }

        [Test]
        public void Should_Return_IsTrue_Null_DbTypeGuid()
        {
            Guid? g = null;

            DbType result = Utilities.PropertyToDatabaseType(g, typeof(Guid));

            Assert.IsTrue(result == DbType.Guid);
        }
    }
}
