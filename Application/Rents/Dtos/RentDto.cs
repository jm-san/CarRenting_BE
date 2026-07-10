using Application.Costumers.Dtos;
using Application.Vehicles.Dtos;

namespace Application.Rents.Dtos;

public class RentDto
{
    public int Id { get; set; }

    public int CustomerId { get; set; }
    public CustomerDto Customer { get; set; }

    public int VehicleId { get; set; }
    public VehicleDto Vehicle { get; set; }

    public DateTime RentStartDate { get; set; }
    public DateTime RentEndDate { get; set; }
    public double TotalPrice { get; set; }
    public bool IsActive { get; set; }
}
