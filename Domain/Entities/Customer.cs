using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public class Customer
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string Name { get; set; }
    public string LastName { get; set; }
    public string DNI { get; set; }
    public string Telephone { get; set; }
    public DateTime Birthdate { get; set; }
}
