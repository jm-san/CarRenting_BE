using Domain.Entities;
using Domain.Filters;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class RentRepository : IRentRepository
{
    private readonly IMongoCollection<Rent> _collection;

    public RentRepository(MongoDBService mongoDBService)
    {
        _collection = mongoDBService.GetCollection<Rent>("Rents");
    }

    public async Task<List<Rent>> GetAllAsync(RentFilter filters)
    {
        var builder = Builders<Rent>.Filter;
        var filterList = new List<FilterDefinition<Rent>>();

        var properties = filters.GetType().GetProperties();
        foreach (var prop in properties)
        {
            var value = prop.GetValue(filters);
            if (value != null)
            {
                filterList.Add(builder.Eq(prop.Name, value));
            }
        }

        var combinedFilter = filterList.Any() ? builder.And(filterList) : builder.Empty;
        return await _collection.Find(combinedFilter).ToListAsync();
    }

    public async Task<Rent> GetByIdAsync(string id) =>
        await _collection.Find(v => v.Id == id).FirstOrDefaultAsync();

    public async Task InsertAsync(Rent rent) =>
        await _collection.InsertOneAsync(rent);

    public async Task UpdateAsync(string id, Rent rent) =>
        await _collection.ReplaceOneAsync(v => v.Id == id, rent);

    public async Task DeleteAsync(string id) =>
        await _collection.DeleteOneAsync(v => v.Id == id);
}
