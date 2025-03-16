using Domain.Entities;
using Domain.Filters;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IMongoCollection<Customer> _collection;

    public CustomerRepository(MongoDBService mongoDBService)
    {
        _collection = mongoDBService.GetCollection<Customer>("Customers");
    }

    public async Task<List<Customer>> GetAllAsync(CustomerFilter filters)
    {
        var builder = Builders<Customer>.Filter;
        var filterList = new List<FilterDefinition<Customer>>();

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

    public async Task<Customer> GetByIdAsync(string id) =>
        await _collection.Find(v => v.Id == id).FirstOrDefaultAsync();

    public async Task InsertAsync(Customer customer) =>
        await _collection.InsertOneAsync(customer);

    public async Task UpdateAsync(string id, Customer customer) =>
        await _collection.ReplaceOneAsync(v => v.Id == id, customer);

    public async Task DeleteAsync(string id) =>
        await _collection.DeleteOneAsync(v => v.Id == id);
}
