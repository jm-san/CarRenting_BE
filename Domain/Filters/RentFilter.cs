namespace Domain.Filters
{
    public class RentFilter
    {
        public string? CustomerId { get; set; }
        public string? VehicleId { get; set; }
        public bool? IsActive { get; set; }

    }
}
