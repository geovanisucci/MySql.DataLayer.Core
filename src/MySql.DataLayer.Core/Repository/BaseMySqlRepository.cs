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

        private readonly string _databaseName;

        /// <summary>
        /// The default constructor for BaseMySqlRepository.
        /// </summary>
        /// <param name="connectionFactory"></param>
        public BaseMySqlRepository(IMySqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _databaseName = _connectionFactory.GetDatabaseName();
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
                            itemValue = DBNull.Value;

                        _parameters.Add(new QueryParameter { ParameterName = $"@{paramName}", ParameterValue = itemValue });
                    }
                }
                else
                {
                    columns.Add(Utilities.GetColumnName<TEntity>(prop).Replace($"{tableName}.", ""));
                    var paramName = Utilities.GetColumnName<TEntity>(prop).Replace($"{tableName}.", "").Replace("`", "");

                    var itemValue = prop.GetValue(entity);
                    if (prop.GetValue(entity) == null)
                        itemValue = DBNull.Value;

                    _parameters.Add(new QueryParameter { ParameterName = $"@{paramName}", ParameterValue = itemValue });
                }
            });

            _sql.Append($"insert into {_databaseName}.{tableName} ({String.Join(",", columns)}) values ({String.Join(",", _parameters.Select(p => p.ParameterName))})");

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
            List<TEntity> result = new List<TEntity>();
            using (var connection = await _connectionFactory.GetAsync())
            {

                StringBuilder sql = new StringBuilder();

                if (conditionsSearch == null)
                {

                    sql.Append($"Select * From {_databaseName}.{Utilities.GetTableName<TEntity>()}");


                    result = await connection
                                        .QueryAsync<TEntity>
                                        (sql.ToString()) as List<TEntity>;

                    return result;
                }
                else
                {
                    if (!conditionsSearch.Any())
                        throw new Exception("The condition search parameter it not filled.");

                    DynamicParameters parameters = new DynamicParameters();

                    sql.Append($"Select * From {_databaseName}.{Utilities.GetTableName<TEntity>()}");


                    bool first = true;

                    foreach (var c in conditionsSearch)
                    {
                        if (first)
                        {
                            if (c.Condition != Condition.Where)
                            {
                                throw new Exception("The first condition needs to be Where.");
                            }
                        }
                        else
                        {
                            if (c.Condition == Condition.Where)
                            {
                                throw new Exception("The condition needs to be And/Or.");
                            }
                        }

                        sql.Append($"{Utilities.ConditionToString(c.Condition)} {c.ParameterName}{Utilities.OperatorToString(c.Operator)}@{c.ParameterName}");

                        parameters.Add(c.ParameterName, c.ParameterValue);

                        first = false;
                    }

                    result = await connection
                                .QueryAsync<TEntity>
                                (sql.ToString(), parameters) as List<TEntity>;
                }
            }

            return result;
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
            List<TResult> result = new List<TResult>();
            using (var connection = await _connectionFactory.GetAsync())
            {
                StringBuilder sql = new StringBuilder();

                if (conditionsSearch == null)
                {
                    if (columns != null)
                    {

                        if (columns.Count() > 0)
                        {
                            sql.Append($"Select {String.Join<ColumnTable>(",", columns)} From {_databaseName}.{Utilities.GetTableName<TEntity>()}");
                        }

                        else
                            throw new Exception("The columns parameter it not filled.");
                    }
                    else
                        sql.Append($"Select * From {_databaseName}.{Utilities.GetTableName<TEntity>()}");

                    var s = sql.ToString();

                    result = await connection
                                        .QueryAsync<TResult>
                                        (sql.ToString()) as List<TResult>;

                    return result;
                }
                else
                {
                    if (!conditionsSearch.Any())
                        throw new Exception("The condition search parameter it not filled.");

                    DynamicParameters parameters = new DynamicParameters();

                    if (columns != null)
                    {
                        if (columns.Count() > 0)
                            sql.Append($"Select {string.Join<ColumnTable>(",", columns)} From {_databaseName}.{Utilities.GetTableName<TEntity>()}");
                        else
                            throw new Exception("The columns parameter it not filled.");
                    }
                    else
                        sql.Append($"Select * From {_databaseName}.{Utilities.GetTableName<TEntity>()}");


                    bool first = true;

                    foreach (var c in conditionsSearch)
                    {
                        if (first)
                        {
                            if (c.Condition != Condition.Where)
                            {
                                throw new Exception("The first condition needs to be Where.");
                            }
                        }
                        else
                        {
                            if (c.Condition == Condition.Where)
                            {
                                throw new Exception("The condition needs to be And/Or.");
                            }
                        }

                        sql.Append($"{Utilities.ConditionToString(c.Condition)} {c.ParameterName} {Utilities.OperatorToString(c.Operator)} @{c.ParameterName}");

                        parameters.Add(c.ParameterName, c.ParameterValue);

                        first = false;
                    }

                    result = await connection
                                .QueryAsync<TResult>
                                (sql.ToString(), parameters) as List<TResult>;
                }
            }

            return result;
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

                sql.Append($"Select * From {_databaseName}.{Utilities.GetTableName<TEntity>()}");

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

            sql.Append($"Delete from {_databaseName}.{tableName} Where {pkColumnName} = @{Utilities.GetPkColumnName<TEntity>(false)}");


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
                        itemValue = DBNull.Value;

                    _parameters.Add(new QueryParameter { ParameterName = $"@{paramName}", ParameterValue = itemValue });
                }
                else
                {
                    id = prop.GetValue(entity);
                }
            });

            _sql.Append($"UPDATE {_databaseName}.{tableName} set ");
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
        public virtual async Task<List<TResult>> ExecuteStoredProcedure<TResult>(QueryParameter[] queryParameters = null)
            where TResult : IDataStoredProcedure
        {
            List<TResult> result = new List<TResult>();

            using (var connection = await _connectionFactory.GetAsync())
            {
                StringBuilder sql = new StringBuilder();

                sql.Append($"CALL {_databaseName}.{Utilities.GetStoredProcedureName<TResult>()}");


                if (queryParameters != null)
                {
                    bool first = true;

                    foreach (var c in queryParameters)
                    {
                        if (first)                        
                           sql.Append($"('{c.ParameterValue}'");
                        
                        else                     
                            sql.Append($",'{c.ParameterValue}'");
                        
                        first = false;
                    }

                    sql.Append($");");
                }

                result = await connection
                                     .QueryAsync<TResult>
                                     (sql.ToString()) as List<TResult>;

                return result;
            }
        }
    }
}