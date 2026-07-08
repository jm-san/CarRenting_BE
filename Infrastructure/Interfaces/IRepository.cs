namespace Infrastructure.Interfaces;

public interface IRepository<TEntity, TFilter>
{
    Task<List<TEntity>> GetAllAsync(TFilter filters);
    Task<TEntity> GetByIdAsync(string id);
    Task InsertAsync(TEntity entity);
    Task UpdateAsync(string id, TEntity entity);
    Task DeleteAsync(string id);
}
