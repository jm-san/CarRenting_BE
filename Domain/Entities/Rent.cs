namespace Domain.Entities;

public class Rent
{
    public int Id { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; }

    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }

    public DateTime RentStartDate { get; set; }
    public DateTime RentEndDate { get; set; }
    public double TotalPrice { get; set; }
    public bool IsActive { get; set; }
}
