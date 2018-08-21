using System.Collections.Generic;
using System.Threading.Tasks;
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
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> GetAllAsync(MySqlConnection connection, MySqlTransaction transaction = null);
        Task<TEntity> GetAsync(object id);
        Task<TEntity> GetAsync(object id, MySqlConnection connection, MySqlTransaction transaction = null);
        Task<int> CreateAsync(TEntity entity, bool IsPkAutoIncrement = false);
        Task<int> CreateAsync(TEntity entity, MySqlConnection connection, bool IsPkAutoIncrement = false, MySqlTransaction transaction = null);
        Task<int> UpdateAsync(TEntity entity);
        Task<int> UpdateAsync(TEntity entity, MySqlConnection connection, MySqlTransaction transaction = null);
        Task<int> RemoveAsync(object id);
        Task<int> RemoveAsync(object id, MySqlConnection connection, MySqlTransaction transaction = null);
        Task<List<TProcedure>> ExecuteStoredProcedureReturnListAsync<TProcedure>()
            where TProcedure : IDataStoredProcedure;
        Task<List<TProcedure>> ExecuteStoredProcedureReturnListAsync<TProcedure>(MySqlConnection connection, MySqlTransaction transaction = null)
            where TProcedure : IDataStoredProcedure;
        Task<object> ExecuteStoredProcedureReturnObjectAsync<TProcedure>()
            where TProcedure : IDataStoredProcedure;
        Task<object> ExecuteStoredProcedureReturnObjectAsync<TProcedure>(MySqlConnection connection, MySqlTransaction transaction)
            where TProcedure : IDataStoredProcedure;
        Task ExecuteStoredProcedureAsync<TProcedure>()
            where TProcedure : IDataStoredProcedure;
        Task ExecuteStoredProcedureAsync<TProcedure>(MySqlConnection connection, MySqlTransaction transaction = null)
            where TProcedure : IDataStoredProcedure;
        Task<List<TView>> ExecuteView<TView>()
            where TView : IDataView;
        Task<List<TView>> ExecuteView<TView>(MySqlConnection connection, MySqlTransaction transaction = null)
            where TView : IDataView;


    }
}