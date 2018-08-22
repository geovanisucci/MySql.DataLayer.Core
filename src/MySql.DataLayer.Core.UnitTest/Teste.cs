namespace MySql.DataLayer.Core.UnitTest
{
    using NUnit.Framework;
    using MySql.DataLayer.Core.Repository;
    using MySql.DataLayer.Core;
    using MySql.DataLayer.Core.Connection;
    using MySql.DataLayer.Core.Attributes.EntityConfig.Table;
    using System.Collections.Generic;
    using System;

    public class Teste
    {
        [Test]
        public void Should_Return_Expected_Column_Name()
        {
            IMySqlConnectionFactory conn = new MySqlConnectionFactory("Server=10.222.0.170;DataBase=aresdb;Uid=root;Pwd=mudar@123;Pooling=True;Allow User Variables=true");
            var  r = new Repository(conn);

            List<ColumnTable> c = new List<ColumnTable>();
            c.Add(new ColumnTable("id_state").As("Id"));
            c.Add(new ColumnTable("name"));
            var t = r.GetAllAsync<FooBar>(c.ToArray());

            var t2 = r.GetAllAsync();
            List<ConditionSearch> cond = new List<ConditionSearch>();
            cond.Add(new ConditionSearch()
            {
                Condition = Condition.Where,
                Operator = ConditionOperator.Equal,
                ParameterName = "id_state",
                ParameterValue = 1

            });
              cond.Add(new ConditionSearch()
            {
                Condition = Condition.Where,
                Operator = ConditionOperator.Equal,
                ParameterName = "initials",
                ParameterValue = "SP"

            });
            var t3 = r.GetAllAsync(cond.ToArray());

            var ppp = r.GetAsync(1);

            Foo f = new Foo();
            f.Id = 2;
            f.id_country =1;
            f.initials = "RJ";
            f.name = "Rio";

            r.CreateAsync(f, true).Wait();
        
        }
    }

    [TableName("states")]
    public class Foo : IDataEntity
    {
        [ColumnName("id_state")]
        [PK("id_state")]
        public int Id { get; set; }
        public string name { get; set; }
        public string initials { get; set; }
        public int id_country { get; set; }

    }
     public class FooBar : IDataEntity
    {
        public int Id { get; set; }
        public string name { get; set; }
      
    }

    public class Repository : BaseMySqlRepository<Foo>
    {
        public Repository(IMySqlConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
    }
}