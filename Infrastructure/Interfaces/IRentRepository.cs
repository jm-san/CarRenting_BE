using Domain.Entities;
using Domain.Filters;

namespace Infrastructure.Interfaces;

public interface IRentRepository
{
    Task<List<Rent>> GetAllAsync(RentFilter filters);
    Task<Rent> GetByIdAsync(string id);
    Task InsertAsync(Rent rent);
    Task UpdateAsync(string id, Rent rent);
    Task DeleteAsync(string id);
}
