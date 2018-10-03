namespace MySql.DataLayer.Core.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MySql.Data.MySqlClient;
    using MySql.DataLayer.Core.Connection;
    using Dapper;
    using System.Linq;
    using System.Text;
    using System;
    using System.Reflection;
    using MySql.DataLayer.Core.Attributes.EntityConfig.Table;
    using MySql.DataLayer.Core.Utils;
    using System.Data;

    /// <summary>
    /// Implementation of IMySqlDataRepository with the basic CRUD operations using Dapper Framework.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseMySqlRepository<TEntity> : IMySqlDataRepository<TEntity>
        where TEntity : IDataEntity
    {
        private readonly IMySqlConnectionFactory _connectionFactory;

        /// <summary>
        /// The default constructor for BaseMySqlRepository.
        /// </summary>
        /// <param name="connectionFactory"></param>
        public BaseMySqlRepository(IMySqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        /// <summary>
        /// Provides the insert statement.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="IsPkAutoIncrement"></param>
        /// <returns></returns>
        public virtual async Task<int> CreateAsync(TEntity entity, bool IsPkAutoIncrement = false)
        {
            int result = 0;
            using (var connection = await _connectionFactory.GetAsync())
            {
                result = await CreateAsync(entity, connection, IsPkAutoIncrement);
            }

            return result;

        }
        /// <summary>
        /// Provides the insert statement.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="connection"></param>
        /// <param name="IsPkAutoIncrement"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<int> CreateAsync(TEntity entity, MySqlConnection connection, bool IsPkAutoIncrement = false, MySqlTransaction transaction = null)
        {
            var _parameters = new List<QueryParameter>();
            StringBuilder _sql = new StringBuilder();
            string tableName = Utilities.GetTableName<TEntity>();
            List<string> columns = new List<string>();
            DynamicParameters dapperParameters = new DynamicParameters();

            var properties = Utilities.GetProperties<TEntity>();

            properties.ForEach(prop =>
            {
                if (IsPkAutoIncrement)
                {
                    if (!Utilities.IsKeyProperty(prop))
                    {
                        columns.Add(Utilities.GetColumnName<TEntity>(prop).Replace($"{tableName}.", ""));
                        var paramName = Utilities.GetColumnName<TEntity>(prop).Replace($"{tableName}.", "").Replace("`", "");

                        var itemValue = prop.GetValue(entity);
                        if (prop.GetValue(entity) == null)
                            itemValue = null;

                        _parameters.Add(new QueryParameter { ParameterName = $"@{paramName}", ParameterValue = itemValue });
                    }
                }
                else
                {
                    columns.Add(Utilities.GetColumnName<TEntity>(prop).Replace($"{tableName}.", ""));
                    var paramName = Utilities.GetColumnName<TEntity>(prop).Replace($"{tableName}.", "").Replace("`", "");

                    var itemValue = prop.GetValue(entity);
                    if (prop.GetValue(entity) == null)
                        itemValue = null;

                    _parameters.Add(new QueryParameter { ParameterName = $"@{paramName}", ParameterValue = itemValue });
                }
            });

            _sql.Append($"insert into {tableName} ({String.Join(",", columns)}) values ({String.Join(",", _parameters.Select(p => p.ParameterName))})");

            _parameters.ForEach(p => dapperParameters.Add(p.ParameterName, p.ParameterValue));

            return await connection.ExecuteAsync(_sql.ToString(), dapperParameters, transaction);

        }
        /// <summary>
        /// Provides the Select operation to get a list for mapped data.
        /// </summary>
        /// <param name="conditionsSearch"></param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetAllAsync(ConditionSearch[] conditionsSearch = null)
        {
            using (var connection = await _connectionFactory.GetAsync())
            {
                List<TEntity> result = new List<TEntity>();

                StringBuilder sql = new StringBuilder();

                string tableName = Utilities.GetTableName<TEntity>();

                (DynamicParameters parameters, string conditionsQuery) = Utilities.GetConditionsToSearch(conditionsSearch);

                sql.Append($"SELECT * FROM {tableName} {conditionsQuery}");

                result = await connection
                            .QueryAsync<TEntity>
                            (sql.ToString(), parameters) as List<TEntity>;

                return result;
            }
        }
        /// <summary>
        ///   Provides the Select operation to get a list for unmapped data.
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="conditionsSearch"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public virtual async Task<List<TResult>> GetAllAsync<TResult>(ColumnTable[] columns, ConditionSearch[] conditionsSearch = null)
            where TResult : class
        {
            using (var connection = await _connectionFactory.GetAsync())
            {
                List<TResult> result = new List<TResult>();

                StringBuilder sql = new StringBuilder();

                string columnsToSearch = Utilities.GetColumnsToSearch(columns);

                string tableName = Utilities.GetTableName<TEntity>();

                (DynamicParameters parameters, string conditionsQuery) = Utilities.GetConditionsToSearch(conditionsSearch);

                sql.Append($"SELECT {columnsToSearch} FROM {tableName} {conditionsQuery}");

                result = await connection
                            .QueryAsync<TResult>
                            (sql.ToString(), parameters) as List<TResult>;

                return result;
            }
        }
        /// <summary>
        /// Provides the Select operation to geta single mapped data.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetAsync(object id)
        {
            TEntity result = default(TEntity);
            using (var connection = await _connectionFactory.GetAsync())
            {
                StringBuilder sql = new StringBuilder();
                DynamicParameters parameters = new DynamicParameters();

                sql.Append($"Select * From {Utilities.GetTableName<TEntity>()}");

                string pkColumnName = Utilities.GetPkColumnName<TEntity>(false);

                if (string.IsNullOrEmpty(pkColumnName))
                    throw new NotImplementedException(
                             $"ITable: {typeof(TEntity)} does not implement {typeof(PKAttribute)}."
                         );

                sql.Append($" Where {pkColumnName} = @{Utilities.GetPkColumnName<TEntity>(false)}");
                sql.Append($" Limit 1");

                parameters.Add(pkColumnName, id);

                result = await connection.QueryFirstOrDefaultAsync<TEntity>(sql.ToString(), parameters);
            }

            return result;
        }
        /// <summary>
        /// Provides the delete statement.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<int> RemoveAsync(object id)
        {
            int result = 0;
            using (var connection = await _connectionFactory.GetAsync())
            {
                result = await RemoveAsync(id, connection);
            }

            return result;
        }
        /// <summary>
        /// Provides the delete statement.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<int> RemoveAsync(object id, MySqlConnection connection, MySqlTransaction transaction = null)
        {
            StringBuilder sql = new StringBuilder();

            string pkColumnName = Utilities.GetPkColumnName<TEntity>(false);

            if (string.IsNullOrEmpty(pkColumnName))
                throw new NotImplementedException(
                         $"ITable: {typeof(TEntity)} does not implement {typeof(PKAttribute)}."
                     );

            string tableName = Utilities.GetTableName<TEntity>();

            sql.Append($"Delete from {tableName} Where {pkColumnName} = @{Utilities.GetPkColumnName<TEntity>(false)}");


            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(pkColumnName, id);


            return await connection.ExecuteAsync(sql.ToString(), parameters, transaction);
        }
        /// <summary>
        /// Provides the delete statement by the Entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<int> RemoveAsync(TEntity entity)
        {
            int result = 0;
            using (var connection = await _connectionFactory.GetAsync())
            {
                result = await RemoveAsync(entity, connection);
            }

            return result;
        }
        /// <summary>
        /// Provides the delete statement by the Entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<int> RemoveAsync(TEntity entity, MySqlConnection connection, MySqlTransaction transaction = null)
        {
            StringBuilder sql = new StringBuilder();

            string pkColumnName = Utilities.GetPkColumnName<TEntity>(false);
            object id = Utilities.GetPropertyValue(entity, pkColumnName) ;

            if (string.IsNullOrEmpty(pkColumnName))
                throw new NotImplementedException(
                         $"ITable: {typeof(TEntity)} does not implement {typeof(PKAttribute)}."
                     );

            string tableName = Utilities.GetTableName<TEntity>();

            sql.Append($"Delete from {tableName} Where {pkColumnName} = @{pkColumnName}");

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(pkColumnName, id);

            return await connection.ExecuteAsync(sql.ToString(), parameters, transaction);
        }
        /// <summary>
        /// Provides the update statement.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            int result = 0;
            using (var connection = await _connectionFactory.GetAsync())
            {
                result = await UpdateAsync(entity, connection);
            }

            return result;
        }
        /// <summary>
        /// Provides the update statement.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(TEntity entity, MySqlConnection connection, MySqlTransaction transaction = null)
        {
            string pkColumnName = Utilities.GetPkColumnName<TEntity>(false);

            if (string.IsNullOrEmpty(pkColumnName))
                throw new NotImplementedException(
                         $"ITable: {typeof(TEntity)} does not implement {typeof(PKAttribute)}."
                     );

            var _parameters = new List<QueryParameter>();
            StringBuilder _sql = new StringBuilder();
            string tableName = Utilities.GetTableName<TEntity>();
            List<string> columns = new List<string>();
            DynamicParameters dapperParameters = new DynamicParameters();

            var properties = Utilities.GetProperties<TEntity>();

            object id = null;

            properties.ForEach(prop =>
            {

                if (!Utilities.IsKeyProperty(prop))
                {
                    columns.Add(Utilities.GetColumnName<TEntity>(prop).Replace($"{tableName}.", ""));
                    var paramName = Utilities.GetColumnName<TEntity>(prop).Replace($"{tableName}.", "").Replace("`", "");

                    var itemValue = prop.GetValue(entity);
                    if (prop.GetValue(entity) == null)
                        itemValue = null;

                    _parameters.Add(new QueryParameter { ParameterName = $"@{paramName}", ParameterValue = itemValue });
                }
                else
                {
                    id = prop.GetValue(entity);
                }
            });

            _sql.Append($"UPDATE {tableName} set ");
            bool first = true;
            foreach (var item in _parameters)
            {
                if (first)
                {
                    _sql.Append($"{item.ParameterName.Replace("@", "")} = {item.ParameterName} ");
                }
                else
                {
                    _sql.Append($", {item.ParameterName.Replace("@", "")} = {item.ParameterName} ");
                }

                first = false;
            }

            _sql.Append($" Where {pkColumnName} = @{Utilities.GetPkColumnName<TEntity>(false)}");

            dapperParameters.Add(pkColumnName, id);

            _parameters.ForEach(p => dapperParameters.Add(p.ParameterName, p.ParameterValue));

            return await connection.ExecuteAsync(_sql.ToString(), dapperParameters, transaction);
        }

        /// <summary>
        /// Provides the execution of a StoredProcedure to get a list for mapped data
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        public virtual async Task<List<TResult>> ExecuteStoredProcedureAsync<TResult>(QueryParameter[] queryParameters = null)
            where TResult : IDataStoredProcedure
        {
            List<TResult> result = new List<TResult>();

            using (var connection = await _connectionFactory.GetAsync())
            {
                result = await ExecuteStoredProcedureAsync<TResult>(queryParameters, connection);
            }

            return result;
        }
        /// <summary>
        /// Provides the execution of a StoredProcedure to get a list for mapped data, with option to pass the transaction
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryParameters"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<List<TResult>> ExecuteStoredProcedureAsync<TResult>(QueryParameter[] queryParameters, MySqlConnection connection, MySqlTransaction transaction = null)
            where TResult : IDataStoredProcedure
        {
            List<TResult> result = new List<TResult>();

            StringBuilder sql = new StringBuilder();

            sql.Append(Utilities.GetStoredProcedureName<TResult>());

            DynamicParameters parameters = new DynamicParameters();

            if (queryParameters != null)
            {
                foreach (var c in queryParameters)
                {
                    if (c.ParameterValue != null)
                    {
                        var dbType = Utilities.PropertyToDatabaseType(c.ParameterValue);

                        parameters.Add(c.ParameterName, c.ParameterValue, dbType);
                    }
                    else
                        parameters.Add(c.ParameterName);
                }
            }

            result = await connection
                                 .QueryAsync<TResult>
                                 (sql.ToString(), parameters, transaction, null, CommandType.StoredProcedure) as List<TResult>;

            return result;
        }
        /// <summary>
        /// Provides the execution of a StoredProcedure to get the count of affected rows
        /// </summary>
        /// <typeparam name="TStoredProcedure"></typeparam>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        public virtual async Task<int> ExecuteStoredProcedureReturnAffectedRowsAsync<TResult>(QueryParameter[] queryParameters = null)
            where TResult : IDataStoredProcedure
        {
            int result = 0;

            using (var connection = await _connectionFactory.GetAsync())
            {
                result = await ExecuteStoredProcedureReturnAffectedRowsAsync<TResult>(queryParameters, connection);
            }

            return result;
        }
        /// <summary>
        /// Provides the execution of a StoredProcedure to get the count of affected rows, with option to pass the transaction
        /// </summary>
        /// <typeparam name="TStoredProcedure"></typeparam>
        /// <param name="queryParameters"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<int> ExecuteStoredProcedureReturnAffectedRowsAsync<TResult>(QueryParameter[] queryParameters, MySqlConnection connection, MySqlTransaction transaction = null)
           where TResult : IDataStoredProcedure
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(Utilities.GetStoredProcedureName<TResult>());

            DynamicParameters parameters = new DynamicParameters();

            if (queryParameters != null)
            {
                foreach (var c in queryParameters)
                {
                    if (c.ParameterValue != null)
                    {
                        var dbType = Utilities.PropertyToDatabaseType(c.ParameterValue);

                        parameters.Add(c.ParameterName, c.ParameterValue, dbType);
                    }
                    else
                        parameters.Add(c.ParameterName);
                }
            }

            return await connection
                                 .ExecuteAsync
                                 (sql.ToString(), parameters, transaction, null, CommandType.StoredProcedure);             
        }

        /// <summary>
        ///  Provides the execution of a View to get a list for mapped data
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="columns"></param>
        /// <param name="conditionsSearch"></param>
        /// <returns></returns>
        public virtual async Task<List<TResult>> ExecuteViewAsync<TResult>(ColumnTable[] columns = null, ConditionSearch[] conditionsSearch = null)
            where TResult : IDataView
        {
            List<TResult> result = new List<TResult>();

            using (var connection = await _connectionFactory.GetAsync())
            {
                result = await ExecuteViewAsync<TResult>(columns, conditionsSearch, connection);
            }

            return result;
        }
        /// <summary>
        /// Provides the execution of a View to get a list for mapped data, with option to pass the transaction
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="columns"></param>
        /// <param name="conditionsSearch"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<List<TResult>> ExecuteViewAsync<TResult>(ColumnTable[] columns, ConditionSearch[] conditionsSearch, MySqlConnection connection, MySqlTransaction transaction = null)
            where TResult : IDataView
        {
            List<TResult> result = new List<TResult>();

            StringBuilder sql = new StringBuilder();

            string columnsToSearch = Utilities.GetColumnsToSearch(columns);

            string viewName = Utilities.GetViewName<TResult>();

            (DynamicParameters parameters, string conditionsQuery) = Utilities.GetConditionsToSearch(conditionsSearch);

            sql.Append($"SELECT {columnsToSearch} FROM {viewName} {conditionsQuery}");

            result = await connection
                        .QueryAsync<TResult>
                        (sql.ToString(), parameters) as List<TResult>;

            return result;
        }
    }
}