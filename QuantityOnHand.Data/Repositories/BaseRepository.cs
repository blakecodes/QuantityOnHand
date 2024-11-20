
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace QuantityOnHand.Data.Repositories;

public class BaseRepository<TEntity>(ApplicationDbContext context) : IBaseRepository<TEntity>
    where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    /// <summary>
    ///     Gets an item by its ID from the database.
    /// </summary>
    /// <param name="id">The ID of the item to retrieve.</param>
    /// <returns>The item with the specified ID, or null if no item is found.</returns>
    public async Task<TEntity?> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    ///     Gets a page of items from the database.
    /// </summary>
    /// <param name="filter">An expression to filter the items.</param>
    /// <param name="orderBy">An expression to order the items.</param>
    /// <param name="pageNumber">The page number to retrieve. Defaults to 1.</param>
    /// <param name="pageSize">The number of items per page. Defaults to 10.</param>
    /// <returns>A list of items.</returns>
    public async Task<IEnumerable<TEntity>> GetPageAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}