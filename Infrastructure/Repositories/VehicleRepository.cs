using Domain.Entities;
using Domain.Filters;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly CarRentingDbContext _context;

    public VehicleRepository(CarRentingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Vehicle>> GetAllAsync(VehicleFilter filters)
    {
        var query = _context.Vehicles.AsQueryable();

        if (filters != null)
        {
            if (!string.IsNullOrEmpty(filters.Brand))
                query = query.Where(v => v.Brand == filters.Brand);
            if (!string.IsNullOrEmpty(filters.Model))
                query = query.Where(v => v.Model == filters.Model);
            if (!string.IsNullOrEmpty(filters.NumberPlate))
                query = query.Where(v => v.NumberPlate == filters.NumberPlate);
        }

        return await query.ToListAsync();
    }

    public async Task<Vehicle?> GetByIdAsync(int id) =>
        await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);

    public async Task InsertAsync(Vehicle vehicle)
    {
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Vehicle vehicle) =>
        await _context.SaveChangesAsync();

    public async Task DeleteAsync(int id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);
        if (vehicle == null)
            return;

        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();
    }
}
