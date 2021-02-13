
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ServiceAppApi.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Commit();

        #region CRUD Methods

        DbContext GetContext();

        //T Create<T>() where T : class;


        void Add<T>(T obj) where T : class;
        void Attach<T>(T obj, bool setToChanged = false) where T : class;
        bool Remove<T>(T item) where T : class;
        void Detach<T>(T item) where T : class;

        IQueryable<T> Get<T>(bool disableProxies = false) where T : class;
        void Reload<T>(T entity) where T : class;
        void Refresh<T>(T entity, bool clientWins = false) where T : class;

        #endregion

        #region Database and Stored Procedure Methods and Properties

        string ConnectionString { get; }
        //int ExecuteScalarSQL(string sql);
        //IEnumerable<T> ExecuteSQL<T>(string sql) where T : class;
        void Dispose();

        //void ExecuteStoredProcedure(string procedureName, params object[] values);
        //Todo: find alternative for ObjectResult<T>
        //T ExecuteStoreQuery<T>(string procedureName, IDictionary<string, object> values) where T : class;
        //IEnumerable<T> ExecuteSQL<T>(string sql);
        //int ExecuteScalarSQL(string sql);

        #endregion

        #region CheckDatabaseType

        //Check database is real db or test db(Sql lite)
        bool CheckDatabaseType();

        #endregion
    }
}
