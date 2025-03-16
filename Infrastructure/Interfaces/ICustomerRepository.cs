using Domain.Entities;
using Domain.Filters;

namespace Infrastructure.Interfaces;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync(CustomerFilter filters);
    Task<Customer> GetByIdAsync(string id);
    Task InsertAsync(Customer customer);
    Task UpdateAsync(string id, Customer customer);
    Task DeleteAsync(string id);
}
