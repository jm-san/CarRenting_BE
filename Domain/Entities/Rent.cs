using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public class Rent
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonRepresentation(BsonType.ObjectId)]
    public string CustomerId { get; set; }
    public Customer Customer { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }

    public DateTime RentStartDate { get; set; }
    public DateTime RentEndDate { get; set; }
    public double TotalPrice { get; set; }
    public bool IsActive { get; set; }
}
