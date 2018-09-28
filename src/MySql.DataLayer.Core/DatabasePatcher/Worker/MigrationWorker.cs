using Dapper;
using MySql.DataLayer.Core.Connection;
using MySql.DataLayer.Core.DatabasePatcher.MigrationTableEntity;
using MySql.DataLayer.Core.DatabasePatcher.Repository;
using MySql.DataLayer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MySql.DataLayer.Core.DatabasePatcher.Worker
{
    public class MigrationWorker
    {
        private string _connectionString;
        private string _databaseName;

        private MigrationRepository _repository;

        IMySqlConnectionFactory conn;

        /// <summary>
        /// Constructor for Migration Worker to store the connection string and Database Name
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        public MigrationWorker(string connectionString, string databaseName)
        {
            _connectionString = connectionString;
            _databaseName = databaseName;

            conn = new MySqlConnectionFactory(_connectionString, _databaseName);

            _repository = new MigrationRepository(conn);
        }

        /// <summary>
        /// Create the database if not exists.
        /// Database Name parameter is passed in the constructor
        /// </summary>
        /// <returns></returns>
        public bool CreateDatabaseIfNotExist()
        {
            try
            {
                var sqlCreate = $"create database if not exists `{_databaseName}`";

                using (var c = conn.GetAsync("sys").Result)
                {
                    c.ExecuteAsync(sqlCreate).Wait();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        ///  Create the Migration table if not exists.
        ///  Table Name parameter is passed by the entity inside Deployer/MigrationTableEntity/DatabaseMigrationEntity
        /// </summary>
        /// <returns></returns>
        public bool CreateMigrationTableIfNotExist()
        {
            try
            {
                string migrationTableName = Utilities.GetTableName<DatabaseMigrationEntity>();

                StringBuilder sqlCreate = new StringBuilder();
                sqlCreate.Append($"CREATE TABLE IF NOT EXISTS `{_databaseName}`.`{migrationTableName}`");
                sqlCreate.Append("(");
                sqlCreate.Append("`id` CHAR(36) NOT NULL ,");
                sqlCreate.Append("`DateTimeMigration` DATETIME NOT NULL,");
                sqlCreate.Append("`TimestampMigration` BIGINT NOT NULL,");
                sqlCreate.Append("`FileName` VARCHAR(200) NOT NULL,");
                sqlCreate.Append(" PRIMARY KEY (`id`)");
                sqlCreate.Append(")");

                //Execute SQL Query
                using (var c = conn.GetAsync(_databaseName).Result)
                {
                    c.ExecuteAsync(sqlCreate.ToString()).Wait();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Get all records from migration
        /// </summary>
        /// <returns></returns>
        public async Task<List<DatabaseMigrationEntity>> GetCurrentMigrations()
        {
            try
            {
                List<DatabaseMigrationEntity> lstResult = new List<DatabaseMigrationEntity>();

                lstResult = await _repository.GetAllAsync();

                return lstResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Execute the Sql Script Query string with the option to format script replacing all strings '{DATABASENAME}' with the current DatabaseName parameter
        /// </summary>
        /// <param name="scriptQuery"></param>
        /// <param name="formatDatabaseName"></param>
        /// <returns></returns>
        public bool ExecuteScriptQuery(string scriptQuery,bool formatDatabaseName)
        {
            try
            {
                if (formatDatabaseName)
                    scriptQuery = scriptQuery.Replace("{DATABASENAME}", _databaseName);

                //Execute SQL Query
                using (var c = conn.GetAsync(_databaseName).Result)
                {
                    c.ExecuteAsync(scriptQuery).Wait();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Insert in the Migration table the last executed Script.
        /// </summary>
        /// <param name="fileTimeStamp"></param>
        /// <param name="fileName"></param>
        public void AddMigration(Int64 fileTimeStamp, string fileName)
        {
            DatabaseMigrationEntity migration = new DatabaseMigrationEntity
            {
                Id = Guid.NewGuid(),
                DateTimeMigration = DateTime.Now,
                TimestampMigration = fileTimeStamp,
                FileName = fileName
            };

            _repository.CreateAsync(migration).Wait();      
        }
    }
}
