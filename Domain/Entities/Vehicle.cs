namespace Domain.Entities;

public class Vehicle
{
    public int Id { get; set; }

    public string Brand { get; set; }
    public string Model { get; set; }
    public string NumberPlate { get; set; }
    public DateTime ManufacturingDate { get; set; }
}
