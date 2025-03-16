using Infrastructure.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Services;

public class MongoDBService
{
    private readonly IMongoDatabase _database;

    public MongoDBService(IOptions<MongoDBSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        _database = client.GetDatabase("CarRentingDb");
    }

    public IMongoCollection<T> GetCollection<T>(string collection) =>
        _database.GetCollection<T>(collection);
    
}
