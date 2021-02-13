

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ServiceAppApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceAppApi.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        // some of the ef core migration issues can be found here http://blog.devart.com/migrating-entity-framework-6-projects-to-entity-framework-core-1-entity-framework-7.html
        #region Fields

        protected readonly ServicesAppDataContext Context;

        #endregion

        #region Constructors

        /// <summary>
        /// This unit of work requires a datastore context from the data
        /// provider.  In this implementation, the datastore context is
        /// and EntityFramework context.
        /// </summary>
        /// <param name="context">
        /// The database context that this unit of work refers to for
        /// data operations.
        /// </param>
        public UnitOfWork(ServicesAppDataContext context)
        {
            Context = context;
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }

            GC.SuppressFinalize(Context);
        }

        public DbContext GetContext()
        {
            return Context;
        }

        //public T Create<T>() where T : class
        //{
        //    return ((IObjectContextAdapter)Context).ObjectContext.CreateObject<T>();
        //}

        public void Reload<T>(T entity) where T : class
        {
            Context.Entry<T>(entity).Reload();
        }

        public void Refresh<T>(T entity, bool clientWins = false) where T : class
        {
            //var objectContext = ((IObjectContextAdapter)Context).ObjectContext;
            //if (clientWins)
            //    objectContext.Refresh(RefreshMode.ClientWins, entity);
            //else
            //    objectContext.Refresh(RefreshMode.StoreWins, entity);
        }

        /// <summary>
        /// This method adds the given object to the Set of that type
        /// of object maintained by the current datastore context.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object being added to the current datastore
        /// context's Set.
        /// </typeparam>
        /// <param name="obj">
        /// The object being added to the current datastore context's Set.
        /// </param>
        public void Add<T>(T obj) where T : class
        {
            Context.Set<T>().Add(obj);
        }

        /// <summary>
        /// This method takes an object and attaches it to the current
        /// datastore context.  By default, the object is marked as
        /// modified within the context.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object that is attached to the current
        /// datastore context.
        /// </typeparam>
        /// <param name="obj">
        /// The object being attached to the current datastore context.
        /// </param>
        /// <param name="setToChanged">
        /// A flag that determines if the datastore context should
        /// regard the object that is being attached as "modified".
        /// </param>
        public void Attach<T>(T obj, bool setToChanged = false) where T : class
        {
            if (setToChanged)
            {
                Context.Entry(obj).State = EntityState.Modified;
            }
            else
            {
                Context.Set<T>().Attach(obj);
            }
        }

        /// <summary>
        /// This method detaches the given object from the current
        /// datastore context.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object being detached from the current
        /// datastore context.
        /// </typeparam>
        /// <param name="obj">
        /// The object being detached from the current datastore context.
        /// </param>
        public void Detach<T>(T obj) where T : class
        {
            Context.Entry(obj).State = EntityState.Detached;
        }

        /// <summary>
        /// This method tells the current datastore context to save all
        /// changes and synchronize with the datastore.
        /// </summary>
        public void Commit()
        {
            Context.SaveChanges();
        }

        /// <summary>
        /// This method gets the current datastore context's Set of the 
        /// given type of object.  In this implementation, if proxies are
        /// disabled, they are only disabled for this call, and then 
        /// immediately set back to enabled.
        /// </summary>
        /// <typeparam name="T">
        /// The type fo the Set being retrieved from the current datastore
        /// context.
        /// </typeparam>
        /// <param name="disableProxies">
        /// A flag determining if proxies should be enabled or not when
        /// the Set being requested is returned.  By default, this
        /// implementation has proxies enabled.
        /// </param>
        /// <returns>
        /// The Set of the given type maintained by the current datastore 
        /// context.
        /// </returns>
        public IQueryable<T> Get<T>(bool disableProxies = false) where T : class
        {
            try
            {
                //todo: find alternate to proxy creation. As of now ProxyCreationEnabled is not available https://stackoverflow.com/questions/36222699/toggling-proxycreation-in-ef7-under-new-configuration
                //if (disableProxies)
                //    ((IObjectContextAdapter)Context).ObjectContext.ContextOptions.ProxyCreationEnabled = false;
                //if (Context.Database.IsSqlite())
                //{
                //    return Context.Set<T>().Local.AsQueryable();
                //}
                //else
                //{
                return Context.Set<T>();
                //}
            }
            finally
            {
                //if (disableProxies)
                //    ((IObjectContextAdapter)Context).ObjectContext.ContextOptions.ProxyCreationEnabled = true;
            }
        }

        /// <summary>
        /// This method removes the given object from the Set of the
        /// object's type that is maintained by the current datastore
        /// context.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object being removed from the current datatstore
        /// context's Set of that type.
        /// </typeparam>
        /// <param name="obj">
        /// The object being removed from the current datatstore context's 
        /// Set of the given type.
        /// </param>
        /// <returns>
        /// A boolean determining if the object removal was successful or not.
        /// </returns>
        public bool Remove<T>(T obj) where T : class
        {
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T> value = Context.Set<T>().Remove(obj);
            return Context.ChangeTracker.Entries<T>().Any(entry => (entry.Entity == value) && ((entry.State & EntityState.Deleted) == EntityState.Deleted));
        }

        #endregion

        #region SQL and Stored Procedure Methods and Properties

        public string ConnectionString => Context.Database.GetDbConnection().ConnectionString;//return Context.Database.Connection.ConnectionString;

        /// <summary>
        /// This method takes the given parameters and executes the
        /// stored procedure in the current datastore context that
        /// has the given name.
        /// </summary>
        /// <param name="procedureName">
        /// The name of the stored procedure being invoked.
        /// </param>
        /// <param name="values">
        /// The parameter values with which the stored procedure is
        /// invoked.
        /// </param>
        //public void ExecuteStoredProcedure(string procedureName, params object[] values)
        //{
        //    ((IObjectContextAdapter)Context).ObjectContext.ExecuteStoreCommand(procedureName, values);
        //}

        //public IEnumerable<T> ExecuteSQL<T>(string sql) where T : class
        //{
        //    return Context.Query<T>().FromSql(sql).ToList();
        //}

        //Todo: Need to find alternate for this
        //public int ExecuteScalarSQL(string sql)
        //{
        //    IEnumerable<int> retVal = Context.Database.SqlQuery<int>(sql, new object[] { });
        //    return retVal.First();
        //}

        //We can use EF CORE FromSQL but it will load only related entities 
        //http://www.talkingdotnet.com/how-to-execute-stored-procedure-in-entity-framework-core/
        //https://stackoverflow.com/questions/28599404/how-to-run-stored-procedures-in-entity-framework-core
        //public T ExecuteStoreQuery<T>(string procedureName, IDictionary<string, object> values) where T : class
        //{
        //    List<SqlParameter> parameterList = new List<SqlParameter>();

        //    if (values != null && values.Any())
        //    {
        //        foreach (string key in values.Keys)
        //        {
        //            parameterList.Add(new SqlParameter(key, values[key]));
        //        }

        //        string parameters = string.Join(",", parameterList.Select(a => a.Value));

        //        return Context.Query<T>().FromSql("EXECUTE {0} {1}", procedureName, parameters).FirstOrDefault();
        //    }

        //    return Context.Query<T>().FromSql("EXECUTE {0}", procedureName).FirstOrDefault();
        //    //return  ((IObjectContextAdapter)Context).ObjectContext.ExecuteStoreQuery<T>(procedureName, parameterList.ToArray());
        //}

        //public void ExecuteSQL(string sql)
        //{
        //    Context.Database.ExecuteSqlCommand(sql);
        //}

        #endregion

        #region CheckDatabaseType
        //Check database is real db or test db(Sql lite)

        public bool CheckDatabaseType()
        {
            bool isRealDb = false;
            //if (!Context.Database.IsSqlite())
            //{
            isRealDb = true;
            //}
            return isRealDb;
        }

        #endregion
    }
}
