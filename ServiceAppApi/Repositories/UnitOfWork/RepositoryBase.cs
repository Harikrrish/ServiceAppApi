
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace ServiceAppApi.Repositories.UnitOfWork
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        #region Properties

        /// <summary>
        /// This property maintains the unit of work used to track
        /// control, and update changes to this repository(s).
        /// </summary>
        public IUnitOfWork UnitOfWork { get; set; }
        public RepositoryBase(IEnumerable<IUnitOfWork> _unitOfWork)
        {
            UnitOfWork = _unitOfWork.OfType<UnitOfWork>().FirstOrDefault();
        }

        //Sql Functions
        public string WithNoLock { get { return UnitOfWork.CheckDatabaseType() ? $"WITH(NOLOCK)" : ""; } set {; } }

        /// <summary>
        /// This property maintains a count for the number of items 
        /// in this repository.
        /// </summary>
        public int Count
        {
            get { return UnitOfWork.Get<T>().Count(); }
        }
        public IDbConnection DapperDbConnection
        {
            get { return new SqlConnection(UnitOfWork.ConnectionString); }
            //get { return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["KrossDeliveryBaseDataContext"].ConnectionString); }
        }
        #endregion

        #region Methods

        /// <summary>
        /// This method takes the given item and adds it to the repository
        /// without committing. Existing items are updated in the by using
        /// the "Attach" method.  New items should be "added" to the repository
        /// using this method.
        /// </summary>
        /// <param name="item">
        /// The pre-existing item in the repository that is being updated.
        /// </param>
        public void Add(T item)
        {
            UnitOfWork.Add(item);
        }
        /// <summary>
        /// This method takes the given item and attachs it to the repository
        /// without committing. Existing items are updated by using this method.
        /// New items should be "added" to the repository using the Add method.
        /// </summary>
        /// <param name="item">
        /// The pre-existing item in the repository that is being updated.
        /// </param>
        /// <param name="setToChanged"></param>
        public void Attach(T item, bool setToChanged = false)
        {
            UnitOfWork.Attach(item, setToChanged);
        }

        public void Detach(T obj)
        {
            UnitOfWork.Detach(obj);
        }

        /// <summary>
        /// This method takes the given predicate and uses it to locate
        /// repository items that pass the predicate's conditions.
        /// </summary>
        /// <param name="predicate">
        /// The conditions used to find the desired repository items.
        /// </param>
        /// <param name="disableProxies">
        /// An optional parameter that allows control over the use of 
        /// dynamic proxy objects when finding repository items that pass
        /// the given predicate's conditions.
        /// </param>
        /// <returns>
        /// Repository items that pass the given predicate's conditions.
        /// </returns>
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, bool disableProxies = false)
        {
            return UnitOfWork.Get<T>(disableProxies).Where(predicate);
        }

        //public IEnumerable<T> ExecuteSql(string sql)
        //{
        //    return UnitOfWork.ExecuteSQL<T>(sql);
        //}

        //public void ExecuteDirectDatabaseSql(string sql)
        //{
        //    UnitOfWork.GetContext().Database.ExecuteSqlCommand(sql);
        //}

        //public int ExecuteScalarSql(string sql)
        //{
        //    return UnitOfWork.ExecuteScalarSQL(sql);
        //}

        /// <summary>
        /// This method finds all items in the repository with this 
        /// repository's type.
        /// </summary>
        /// <param name="disableProxies">
        /// An optional parameter that allows control over the use of 
        /// dynamic proxy objects when finding all repository items.
        /// </param>
        /// <returns>
        /// All items in the repository with this repository's type.
        /// </returns>
        public IQueryable<T> FindAll(bool disableProxies = false)
        {
            return UnitOfWork.Get<T>(disableProxies);
        }

        /// <summary>
        /// This method removes the given item from this repository.
        /// </summary>
        /// <param name="item">
        /// The item being removed from this repository.
        /// </param>
        public virtual void Remove(T item)
        {
            UnitOfWork.Remove(item);
        }

        /// <summary>
        /// This method is used to determine if this repository
        /// contains the given item or not.
        /// </summary>
        /// <param name="item">
        /// The item being checked for membership in this repository.
        /// </param>
        /// <returns>
        /// True if the given item is a member of this repository, 
        /// false otherwise.
        /// </returns>
        public bool Contains(T item)
        {
            return UnitOfWork.Get<T>().FirstOrDefault(t => t == item) != null;
        }

        //public void SetSpecialTimeout(int seconds)
        //{
        //    ((IObjectContextAdapter)UnitOfWork.GetContext()).ObjectContext.CommandTimeout = seconds;

        //}

        #endregion
    }
}
