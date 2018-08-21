namespace MySql.DataLayer.Core.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MySql.Data.MySqlClient;
    using MySql.DataLayer.Core.Connection;
    using Dapper;

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

        public  Task<int> CreateAsync(TEntity entity, bool IsPkAutoIncrement = false)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> CreateAsync(TEntity entity, MySqlConnection connection, bool IsPkAutoIncrement = false, MySqlTransaction transaction = null)
        {
            throw new System.NotImplementedException();
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

        public Task<List<TEntity>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<TEntity>> GetAllAsync(MySqlConnection connection, MySqlTransaction transaction = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<TEntity> GetAsync(object id)
        {
            throw new System.NotImplementedException();
        }

        public Task<TEntity> GetAsync(object id, MySqlConnection connection, MySqlTransaction transaction = null)
        {
            throw new System.NotImplementedException();
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