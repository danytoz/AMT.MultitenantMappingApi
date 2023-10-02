using System.Linq.Expressions;

namespace AMT.GenericRepository
{
    public interface IRepository<T, TKey> where T : BaseModel<TKey>
    {
        Task<T> GetByIdAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task DeleteByIdAsync(object id);
        Task SoftDeleteByIdAsync(object id);
        void SoftDelete(T entity);
        Task SaveChangesAsync();
        Task DeleteManyAsync(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? top = null,
            int? skip = null,
            params string[] includeProperties);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null);
    }
}