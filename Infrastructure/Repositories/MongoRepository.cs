using Infrastructure.Interfaces;
using Infrastructure.Services;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class MongoRepository<TEntity, TFilter> : IRepository<TEntity, TFilter>
{
    private readonly IMongoCollection<TEntity> _collection;

    public MongoRepository(MongoDBService mongoDBService, string collectionName)
    {
        _collection = mongoDBService.GetCollection<TEntity>(collectionName);
    }

    public async Task<List<TEntity>> GetAllAsync(TFilter filters)
    {
        if (filters == null)
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        var builder = Builders<TEntity>.Filter;
        var filterList = new List<FilterDefinition<TEntity>>();

        foreach (var prop in filters.GetType().GetProperties())
        {
            var value = prop.GetValue(filters);
            if (value != null)
            {
                filterList.Add(builder.Eq(prop.Name, value));
            }
        }

        var combinedFilter = filterList.Count > 0 ? builder.And(filterList) : builder.Empty;
        return await _collection.Find(combinedFilter).ToListAsync();
    }

    public async Task<TEntity> GetByIdAsync(string id) =>
        await _collection.Find(Builders<TEntity>.Filter.Eq("Id", id)).FirstOrDefaultAsync();

    public async Task InsertAsync(TEntity entity) =>
        await _collection.InsertOneAsync(entity);

    public async Task UpdateAsync(string id, TEntity entity) =>
        await _collection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("Id", id), entity);

    public async Task DeleteAsync(string id) =>
        await _collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("Id", id));
}
