using Domain.Entities;
using Domain.Filters;

namespace Infrastructure.Interfaces;

public interface IRentRepository
{
    Task<List<Rent>> GetAllAsync(RentFilter filters);
    Task<Rent?> GetByIdAsync(int id);
    Task InsertAsync(Rent rent);
    Task UpdateAsync(Rent rent);
    Task DeleteAsync(int id);
}
