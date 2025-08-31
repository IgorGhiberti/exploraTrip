using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class TripRepository : ITripRepository
{
    private readonly ApplicationDBContext _context;
    public TripRepository(ApplicationDBContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task AddTrip(Trip trip)
    {
        try
        {
            _context.Add(trip);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            
            throw new Exception(ex.Message);
        }
        
    }

    public async Task DeleteTrip(Trip trip)
    {
        _context.Remove(trip);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Trip>> GetAllTrips()
    {
        return await _context.Trips.ToListAsync();
    }

    public async Task<Trip?> GetTripById(Guid id)
    {
        return await _context.Trips.SingleOrDefaultAsync(t => t.TripId == id);
    }

    public async Task UpdateTrip(Trip trip)
    {
        _context.Update(trip);
        await _context.SaveChangesAsync();
    }
}