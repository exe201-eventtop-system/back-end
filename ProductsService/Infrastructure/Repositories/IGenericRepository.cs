using Domain.Common;
using Infrastructure.Commons;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public interface IGenericRepository<T, Tid> where T : BaseEntity<Tid> where Tid : struct
    {
        /// <summary>
        ///     Get a full list of all item records in the database.
        /// </summary>
        /// <returns>list of item of type <typeparamref name="T"/></returns>
        Task<List<T>> GetAllAsync();

        /// <summary>
        ///     Query all records of <typeparamref name="T"/> that match the given <paramref name="filter"/> and sort the result by <paramref name="orderBy"/>.
        /// </summary>
        /// <param name="filter">The filtering condition for the query</param>
        /// <param name="orderBy">The ordering system for sorting result list</param>
        /// <returns><see cref="List{T}"/> of items that match the given <paramref name="filter"/> and sorted with <paramref name="orderBy"/>.</returns>
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);

        /// <summary>
        ///     Get an entity using record id.
        /// </summary>
        /// <param name="id">The entity id of type <typeparamref name="Tid"/></param>
        /// <returns>a single instance of <typeparamref name="T"/> if found, else <see cref="null"/> if no record with provided id is found.</returns>
        Task<T?> GetByIdAsync(Tid id);

        /// <summary>
        ///     Add a new instance of type <typeparamref name="T"/> to the <see cref="DbContext"/>.<br/>
        ///     This does not persist changes in the database, 
        ///     invoke <see cref="DbContext.SaveChangesAsync(bool, CancellationToken)"/> or <see cref="DbContext.SaveChanges(bool)"/> to save changes.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> CreateAsync(T id);

        /// <summary>
        ///     Update an <paramref name="entity"/> instance of type <typeparamref name="T"/> or add that instance to the <see cref="DbContext"/> if not added previously.<br/>
        ///     This does not persist changes in the database, 
        ///     invoke <see cref="DbContext.SaveChangesAsync(bool, CancellationToken)"/> or <see cref="DbContext.SaveChanges(bool)"/> to save changes.
        /// </summary>
        /// <param name="entity">the entity of type <typeparamref name="T"/></param>
        /// <returns>the updated entity</returns>
        T Update(T entity);

        /// <summary>
        ///     Remove the <paramref name="entity"/> of type <typeparamref name="T"/> from the <see cref="DbContext"/>.
        ///     This does not persist changes in the database, 
        ///     invoke <see cref="DbContext.SaveChangesAsync(bool, CancellationToken)"/> or <see cref="DbContext.SaveChanges(bool)"/> to save changes.
        /// </summary>
        /// <param name="entity">the item to be removed</param>
        /// <returns>true if successfulyl remove the item, else false</returns>
        bool Remove(T entity);

        /// <summary>
        ///     Remove an <typeparamref name="T"/> entity from the <see cref="DbContext"/> match the provided <paramref name="id"/>.
        ///     This does not persist changes in the database, 
        ///     invoke <see cref="DbContext.SaveChangesAsync(bool, CancellationToken)"/> or <see cref="DbContext.SaveChanges(bool)"/> to save changes.
        /// </summary>
        /// <param name="id">the item to be removed</param>
        /// <returns>true if successfulyl remove the item, else false</returns>
        bool Remove(Tid id);

        /// <summary>
        ///     Query a page of records in the <see cref="DbContext"/>.
        /// </summary>
        /// <param name="page">the current page index of the query</param>
        /// <param name="page_size">the page size of the query</param>
        /// <returns>a <see cref="PaginationResult{T}"/> represents result of the operation</returns>
        Task<PaginationResult<T>> GetPaginatedAsync(int page, int page_size);

        /// <summary>
        ///     Query a page of records in the <see cref="DbContext"/> that match the given <paramref name="filter"/>.
        /// </summary>
        /// <param name="page">the current page index of the query</param>
        /// <param name="page_size">the page size of the query</param>
        /// <paramref name="filter">The filtering condition for the query</param>
        /// <returns>a <see cref="PaginationResult{T}"/> represents result of the operation</returns>
        Task<PaginationResult<T>> GetPaginatedAsync(int page, int page_size, Expression<Func<T, bool>> filter);

        /// <summary>
        ///     Query a page of records in the <see cref="DbContext"/> that match the given <paramref name="filter"/> and sort the result by <paramref name="orderBy"/>.
        /// </summary>
        /// <param name="page">the current page index of the query</param>
        /// <param name="page_size">the page size of the query</param>
        /// <param name="filter">The filtering condition for the query</param>
        /// <param name="orderBy">The ordering system for sorting result list</param>
        /// <returns>a <see cref="PaginationResult{T}"/> represents result of the operation</returns>
        Task<PaginationResult<T>> GetPaginatedAsync(int page, int page_size, Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
    }
}
