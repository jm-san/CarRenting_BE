namespace Application.Rents.Dtos;

public class CreateRentDto
{
    public int CustomerId { get; set; }
    public int VehicleId { get; set; }
    public DateTime? RentStartDate { get; set; }
    public DateTime? RentEndDate { get; set; }
    public double TotalPrice { get; set; }
}
