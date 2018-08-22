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

        public async Task<int> CreateAsync(TEntity entity, bool IsPkAutoIncrement = false)
        {
            int result = 0;
            using (var connection = await _connectionFactory.GetAsync())
            {
                result = await CreateAsync(entity, connection, IsPkAutoIncrement);
            }

            return result;

        }

        public async Task<int> CreateAsync(TEntity entity, MySqlConnection connection, bool IsPkAutoIncrement = false, MySqlTransaction transaction = null)
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

            _sql.Append($"insert into {tableName} ({String.Join(",", columns)}) values ({String.Join(",", _parameters.Select(p => p.ParameterName))})");

            _parameters.ForEach(p => dapperParameters.Add(p.ParameterName, p.ParameterValue));

            return await connection.ExecuteAsync(_sql.ToString(), dapperParameters, transaction);

        }

        public Task ExecuteStoredProcedureAsync<TProcedure>() where TProcedure : IDataStoredProcedure
        {
            throw new System.NotImplementedException();
        }

        public Task ExecuteStoredProcedureAsync<TProcedure>(MySqlConnection connection, MySqlTransaction transaction = null) where TProcedure : IDataStoredProcedure
        {
            throw new System.NotImplementedException();
        }

        public Task<List<TProcedure>> ExecuteStoredProcedureReturnListAsync<TProcedure>() where TProcedure : IDataStoredProcedure
        {
            throw new System.NotImplementedException();
        }

        public Task<List<TProcedure>> ExecuteStoredProcedureReturnListAsync<TProcedure>(MySqlConnection connection, MySqlTransaction transaction = null) where TProcedure : IDataStoredProcedure
        {
            throw new System.NotImplementedException();
        }

        public Task<object> ExecuteStoredProcedureReturnObjectAsync<TProcedure>() where TProcedure : IDataStoredProcedure
        {
            throw new System.NotImplementedException();
        }

        public Task<object> ExecuteStoredProcedureReturnObjectAsync<TProcedure>(MySqlConnection connection, MySqlTransaction transaction) where TProcedure : IDataStoredProcedure
        {
            throw new System.NotImplementedException();
        }

        public Task<List<TView>> ExecuteView<TView>() where TView : IDataView
        {
            throw new System.NotImplementedException();
        }

        public Task<List<TView>> ExecuteView<TView>(MySqlConnection connection, MySqlTransaction transaction = null) where TView : IDataView
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<TEntity>> GetAllAsync(ConditionSearch[] conditionsSearch = null)
        {
            List<TEntity> result = new List<TEntity>();
            using (var connection = await _connectionFactory.GetAsync())
            {

                StringBuilder sql = new StringBuilder();

                if (conditionsSearch == null)
                {

                    sql.Append($"Select * From {Utilities.GetTableName<TEntity>()}");


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

                    sql.Append($"Select * From {Utilities.GetTableName<TEntity>()}");


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

        public async Task<List<TResult>> GetAllAsync<TResult>(ColumnTable[] columns, ConditionSearch[] conditionsSearch = null)
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
                            sql.Append($"Select {String.Join<ColumnTable>(",", columns)} From {Utilities.GetTableName<TEntity>()}");
                        }

                        else
                            throw new Exception("The columns parameter it not filled.");
                    }
                    else
                        sql.Append($"Select * From {Utilities.GetTableName<TEntity>()}");

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
                            sql.Append($"Select {string.Join<ColumnTable>(",", columns)} From {Utilities.GetTableName<TEntity>()}");
                        else
                            throw new Exception("The columns parameter it not filled.");
                    }
                    else
                        sql.Append($"Select * From {Utilities.GetTableName<TEntity>()}");


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
        public async Task<TEntity> GetAsync(object id)
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


        public Task<int> RemoveAsync(object id)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> RemoveAsync(object id, MySqlConnection connection, MySqlTransaction transaction = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> UpdateAsync(TEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> UpdateAsync(TEntity entity, MySqlConnection connection, MySqlTransaction transaction = null)
        {
            throw new System.NotImplementedException();
        }


    }
}