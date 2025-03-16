using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public class Vehicle
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string Brand { get; set; }
    public string Model { get; set; }
    public string NumberPlate { get; set; }
    public DateTime ManufacturingDate { get; set; }
}
