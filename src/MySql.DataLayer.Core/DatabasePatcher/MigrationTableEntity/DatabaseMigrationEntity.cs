using MySql.DataLayer.Core.Attributes.EntityConfig.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySql.DataLayer.Core.DatabasePatcher.MigrationTableEntity
{
    [TableName("_DatabaseMigrations")]
    public class DatabaseMigrationEntity : IDataEntity
    {
        [PK("Id")]
        public Guid Id { get; set; }
        public DateTime DateTimeMigration { get; set; }
        public Int64 TimestampMigration { get; set; }
        public string FileName { get; set; }
    }
}
