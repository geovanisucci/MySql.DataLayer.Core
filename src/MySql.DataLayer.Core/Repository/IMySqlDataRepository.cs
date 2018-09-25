using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;

namespace MySql.DataLayer.Core.Repository
{
    /// <summary>
    /// The MySql Repository interface that contains the basic CRUD operations
    /// and call stored procedures.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IMySqlDataRepository<TEntity>
        where TEntity : IDataEntity
    { 
        /// <summary>
        /// Provides the Select operation to get a list for unmapped data.
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="conditionsSearch"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        Task<List<TResult>> GetAllAsync<TResult>(ColumnTable[] columns, ConditionSearch[] conditionsSearch = null)
            where TResult : class;
        /// <summary>
        ///  Provides the Select operation to get a list for mapped data.
        /// </summary>
        /// <param name="conditionsSearch"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetAllAsync(ConditionSearch[] conditionsSearch = null);
        /// <summary>
        ///  Provides the Select operation to geta single mapped data.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetAsync(object id);
        /// <summary>
        /// Provides the insert statement.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="IsPkAutoIncrement"></param>
        /// <returns></returns>
        Task<int> CreateAsync(TEntity entity, bool IsPkAutoIncrement = false);
        /// <summary>
        /// Provides the insert statement.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="connection"></param>
        /// <param name="IsPkAutoIncrement"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        Task<int> CreateAsync(TEntity entity, MySqlConnection connection, bool IsPkAutoIncrement = false, MySqlTransaction transaction = null);
        /// <summary>
        /// Provides the update statement.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(TEntity entity);
        /// <summary>
        /// Provides the update statement.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(TEntity entity, MySqlConnection connection, MySqlTransaction transaction = null);
        /// <summary>
        /// Provides the delete statement.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> RemoveAsync(object id);
        /// <summary>
        /// Provides the delete statement.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        Task<int> RemoveAsync(object id, MySqlConnection connection, MySqlTransaction transaction = null);
           
        /// <summary>
        /// Provides the execution of a StoredProcedure to get a list for mapped data
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        Task<List<TResult>> ExecuteStoredProcedureAsync<TResult>(QueryParameter[] queryParameters = null)
                    where TResult : IDataStoredProcedure;
        /// <summary>
        /// Provides the execution of a StoredProcedure to get a list for mapped data, with option to pass the transaction
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryParameters"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        Task<List<TResult>> ExecuteStoredProcedureAsync<TResult>(QueryParameter[] queryParameters, MySqlConnection connection, MySqlTransaction transaction = null)
                    where TResult : IDataStoredProcedure;
        /// <summary>
        /// Provides the execution of a StoredProcedure to get the count of affected rows
        /// </summary>
        /// <typeparam name="TStoredProcedure"></typeparam>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        Task<int> ExecuteStoredProcedureReturnAffectedRowsAsync<TStoredProcedure>(QueryParameter[] queryParameters = null)
                            where TStoredProcedure : IDataStoredProcedure;
        /// <summary>
        /// Provides the execution of a StoredProcedure to get the count of affected rows, with option to pass the transaction
        /// </summary>
        /// <typeparam name="TStoredProcedure"></typeparam>
        /// <param name="queryParameters"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        Task<int> ExecuteStoredProcedureReturnAffectedRowsAsync<TStoredProcedure>(QueryParameter[] queryParameters, MySqlConnection connection, MySqlTransaction transaction = null)
                                where TStoredProcedure : IDataStoredProcedure;

        /// <summary>
        ///  Provides the execution of a View to get a list for mapped data
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="columns"></param>
        /// <param name="conditionsSearch"></param>
        /// <returns></returns>
        Task<List<TResult>> ExecuteViewAsync<TResult>(ColumnTable[] columns = null, ConditionSearch[] conditionsSearch = null)
            where TResult : IDataView;
        /// <summary>
        /// Provides the execution of a View to get a list for mapped data, with option to pass the transaction
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="columns"></param>
        /// <param name="conditionsSearch"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        Task<List<TResult>> ExecuteViewAsync<TResult>(ColumnTable[] columns, ConditionSearch[] conditionsSearch, MySqlConnection connection, MySqlTransaction transaction = null)
            where TResult : IDataView;
    }
}