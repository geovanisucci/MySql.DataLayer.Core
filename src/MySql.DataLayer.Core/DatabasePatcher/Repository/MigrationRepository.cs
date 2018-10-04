using MySql.DataLayer.Core.Connection;
using MySql.DataLayer.Core.DatabasePatcher.MigrationTableEntity;
using MySql.DataLayer.Core.Repository;

namespace MySql.DataLayer.Core.DatabasePatcher.Repository
{
    public class MigrationRepository : BaseMySqlRepository<DatabaseMigrationEntity>
    {
        public MigrationRepository(IMySqlConnectionFactory connectionFactory) : base(connectionFactory)
        {

        }
    }
}
