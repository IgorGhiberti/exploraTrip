using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class LocalRepository : ILocalRepository
{
    private readonly ApplicationDBContext _context;

    public LocalRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public async Task AddLocal(Local local)
    {
        _context.Locals.Add(local);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateLocal(Local local)
    {
        _context.Update(local);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteLocal(Local local)
    {
        _context.Remove(local);
        await _context.SaveChangesAsync();
    }

    public async Task<Local?> GetLocalById(Guid localId)
    {
        return await _context.Locals.SingleOrDefaultAsync(l => l.LocalId == localId);
    }

    public Task<List<Local>> GetAllLocalsByTrip(Guid tripId)
    {
        return _context.Locals.Where(l => l.TripId == tripId).ToListAsync();
    }
}