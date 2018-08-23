using MySql.DataLayer.Core.Connection;
using MySql.DataLayer.Core.Repository;
using MySql.DataLayer.Core.UnitTest.GivenRepository.Entities;

namespace MySql.DataLayer.Core.UnitTest.GivenRepository
{
    public class Repository: BaseMySqlRepository<FooEntity>
    {
        public Repository(IMySqlConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
    }
}