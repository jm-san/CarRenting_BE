using Domain.Entities;
using Domain.Filters;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly CarRentingDbContext _context;

    public CustomerRepository(CarRentingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllAsync(CustomerFilter filters)
    {
        var query = _context.Customers.AsQueryable();

        if (filters != null)
        {
            if (!string.IsNullOrEmpty(filters.Name))
                query = query.Where(c => c.Name == filters.Name);
            if (!string.IsNullOrEmpty(filters.LastName))
                query = query.Where(c => c.LastName == filters.LastName);
            if (!string.IsNullOrEmpty(filters.DNI))
                query = query.Where(c => c.DNI == filters.DNI);
            if (!string.IsNullOrEmpty(filters.Telephone))
                query = query.Where(c => c.Telephone == filters.Telephone);
        }

        return await query.ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id) =>
        await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);

    public async Task InsertAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer) =>
        await _context.SaveChangesAsync();

    public async Task DeleteAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            return;

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }
}
