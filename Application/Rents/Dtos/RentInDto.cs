using Application.Costumers.Dtos;
using Application.Vehicles.Dtos;

namespace Application.Rents.Dtos;

public class RentInDto
{
    public string CustomerId { get; set; }
    public CustomerInDto Customer { get; set; }

    public string VehicleId { get; set; }
    public VehicleInDto Vehicle { get; set; }

    public DateTime? RentStartDate { get; set; }
    public DateTime? RentEndDate { get; set; }
    public double TotalPrice { get; set; }
    public bool IsActive { get; set; }
}
