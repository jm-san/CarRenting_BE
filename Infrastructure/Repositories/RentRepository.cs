using Domain.Entities;
using Domain.Filters;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RentRepository : IRentRepository
{
    private readonly CarRentingDbContext _context;

    public RentRepository(CarRentingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Rent>> GetAllAsync(RentFilter filters)
    {
        var query = _context.Rents
            .Include(r => r.Customer)
            .Include(r => r.Vehicle)
            .AsQueryable();

        if (filters != null)
        {
            if (filters.CustomerId.HasValue)
                query = query.Where(r => r.CustomerId == filters.CustomerId.Value);
            if (filters.VehicleId.HasValue)
                query = query.Where(r => r.VehicleId == filters.VehicleId.Value);
            if (filters.IsActive.HasValue)
                query = query.Where(r => r.IsActive == filters.IsActive.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Rent?> GetByIdAsync(int id) =>
        await _context.Rents
            .Include(r => r.Customer)
            .Include(r => r.Vehicle)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task InsertAsync(Rent rent)
    {
        _context.Rents.Add(rent);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Rent rent) =>
        await _context.SaveChangesAsync();

    public async Task DeleteAsync(int id)
    {
        var rent = await _context.Rents.FindAsync(id);
        if (rent == null)
            return;

        _context.Rents.Remove(rent);
        await _context.SaveChangesAsync();
    }
}
