using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AMT.GenericRepository.EfCore
{
    public class EfRepository<T, TKey> : IRepository<T, TKey> where T : BaseModel<TKey>, new()
    {
        private readonly DbContext dbContext;
        private readonly DbSet<T> dbSet;

        public EfRepository(DbContext dbContext) 
        { 
            this.dbContext = dbContext; 
            this.dbSet = dbContext.Set<T>();
        }

        public async Task AddAsync(T entity) => await dbSet.AddAsync(entity);

        public void Delete(T entity) => dbSet.Remove(entity);

        public async Task DeleteByIdAsync(object id)
        {
            var entity = await GetByIdAsync(id);
            Delete(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await dbSet.ToListAsync();

        public async Task<T> GetByIdAsync(object id) => await dbSet.FindAsync(id);

        public void SoftDelete(T entity)
        {
            entity.DeletedOnUtc = DateTime.UtcNow;
            entity.IsDeleted = true;
            dbContext.Update(entity);
        }

        public async Task SoftDeleteByIdAsync(object id)
        {
            var entity = await GetByIdAsync(id);
            SoftDelete(entity);
        }

        public void Update(T entity)
        {
            dbContext.Update(entity);
        }

        public async Task SaveChangesAsync() => await dbContext.SaveChangesAsync();

        public async Task DeleteManyAsync(Expression<Func<T, bool>> filter)
        {
            var entities = dbSet.Where(filter).ToList();
            entities.Select(x=> x.IsDeleted = true).ToList();
            entities.Select(x=>x.DeletedOnUtc= DateTime.UtcNow);
        }

        public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? top = null,
            int? skip = null,
            params string[] includeProperties)
        {
            IQueryable<T> query = dbSet;

            if(filter != null)
            {
                query = query.Where(filter);
            }

            if(includeProperties.Length > 0)
            {
                query = includeProperties.Aggregate(query, (theQuery, theInclude) => theQuery.Include(theInclude));
            }

            if(orderBy != null)
            {
                query = orderBy(query);
            }

            if(skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if(top.HasValue)
            {
                query = query.Take(top.Value);
            }
            return await query.ToListAsync();

        }

        public Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null)
        {
            return dbSet.FirstOrDefaultAsync(filter);
        }
    }
}
