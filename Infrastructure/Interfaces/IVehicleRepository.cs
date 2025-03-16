using Domain.Entities;
using Domain.Filters;

namespace Infrastructure.Interfaces;

public interface IVehicleRepository
{
    Task<List<Vehicle>> GetAllAsync(VehicleFilter filters);
    Task<Vehicle> GetByIdAsync(string id);
    Task InsertAsync(Vehicle vehicle);
    Task UpdateAsync(string id, Vehicle vehicle);
    Task DeleteAsync(string id);
}
