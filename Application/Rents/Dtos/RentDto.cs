using Application.Costumers.Dtos;
using Application.Vehicles.Dtos;

namespace Application.Rents.Dtos;

public class RentDto
{
    public string Id { get; set; }

    public string CustomerId { get; set; }
    public CustomerDto Customer { get; set; }

    public string VehicleId { get; set; }
    public VehicleDto Vehicle { get; set; }

    public DateTime RentStartDate { get; set; }
    public DateTime RentEndDate { get; set; }
    public double TotalPrice { get; set; }
    public bool IsActive { get; set; }
}
