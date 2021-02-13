
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace ServiceAppApi.Repositories.UnitOfWork
{
    public interface IRepository<T> where T : class
    {
        #region Properties


        /// <summary>
        /// This property maintains a count for the number of items
        /// in this repository.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// This property maintains a db connection for retrieving
        /// the data using dapper
        /// </summary>
        IDbConnection DapperDbConnection { get; }

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
        void Add(T item);
        /// <summary>
        /// This method takes the given item and attachs it to the repository
        /// without committing. Existing items are updated by using this method.
        /// New items should be "added" to the repository using the Add method.
        /// </summary>
        /// <param name="item">
        /// The pre-existing item in the repository that is being updated.
        /// </param>
        /// <param name="setToChanged"></param>
        void Attach(T item, bool setToChanged = false);
        void Detach(T item);

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
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, bool disableProxies = false);

        //todo: find replacement
        //IEnumerable<T> ExecuteSql(string sql);
        //int ExecuteScalarSql(string sql);
        //void ExecuteDirectDatabaseSql(string sql);

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
        IQueryable<T> FindAll(bool disableProxies = false);

        /// <summary>
        /// This method removes the given item from this repository.
        /// </summary>
        /// <param name="item">
        /// The item being removed from this repository.
        /// </param>
        void Remove(T item);

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
        bool Contains(T item);

        //todo: find replacement
        //void SetSpecialTimeout(int seconds);

        #endregion

        #region SqlFunctions
        string WithNoLock { get; set; }
        #endregion
    }
}
