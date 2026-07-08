namespace Application.Rents.Dtos;

public class CreateRentDto
{
    public string CustomerId { get; set; }
    public string VehicleId { get; set; }
    public DateTime? RentStartDate { get; set; }
    public DateTime? RentEndDate { get; set; }
    public double TotalPrice { get; set; }
}
