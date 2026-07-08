namespace Application.Vehicles.Dtos;

public class CreateVehicleDto
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public string NumberPlate { get; set; }
    public DateTime ManufacturingDate { get; set; }
}
