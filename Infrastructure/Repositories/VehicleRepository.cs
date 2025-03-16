using Domain.Entities;
using Domain.Filters;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly IMongoCollection<Vehicle> _collection;

    public VehicleRepository(MongoDBService mongoDBService)
    {
        _collection = mongoDBService.GetCollection<Vehicle>("Vehicles");
    }

    public async Task<List<Vehicle>> GetAllAsync(VehicleFilter filters)
    {
        if (filters == null)
        {
            return await _collection.Find(v => true).ToListAsync();
        }

        var builder = Builders<Vehicle>.Filter;
        var filterList = new List<FilterDefinition<Vehicle>>();

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

    public async Task<Vehicle> GetByIdAsync(string id) =>
        await _collection.Find(v => v.Id == id).FirstOrDefaultAsync();

    public async Task InsertAsync(Vehicle vehicle) =>
        await _collection.InsertOneAsync(vehicle);

    public async Task UpdateAsync(string id, Vehicle vehicle) =>
        await _collection.ReplaceOneAsync(v => v.Id == id, vehicle);

    public async Task DeleteAsync(string id) =>
        await _collection.DeleteOneAsync(v => v.Id == id);
}
