namespace Domain.Filters
{
    public class RentFilter
    {
        public int? CustomerId { get; set; }
        public int? VehicleId { get; set; }
        public bool? IsActive { get; set; }

    }
}
