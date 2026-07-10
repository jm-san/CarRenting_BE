using Domain.Entities;
using Domain.Filters;

namespace Infrastructure.Interfaces;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync(CustomerFilter filters);
    Task<Customer?> GetByIdAsync(int id);
    Task InsertAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
}
