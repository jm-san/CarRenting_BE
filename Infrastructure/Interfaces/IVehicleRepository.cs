using Domain.Entities;
using Domain.Filters;

namespace Infrastructure.Interfaces;

public interface IVehicleRepository
{
    Task<List<Vehicle>> GetAllAsync(VehicleFilter filters);
    Task<Vehicle?> GetByIdAsync(int id);
    Task InsertAsync(Vehicle vehicle);
    Task UpdateAsync(Vehicle vehicle);
    Task DeleteAsync(int id);
}
