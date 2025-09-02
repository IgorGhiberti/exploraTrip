using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class TripParticipantRepository : ITripParticipantRepository
{
    private readonly ApplicationDBContext _context;
    public TripParticipantRepository(ApplicationDBContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task AddTripParticipant(TripParticipant tripParticipant)
    {
        _context.Add(tripParticipant);
        await _context.SaveChangesAsync();
    }
    public async Task<List<TripParticipant>> GetTripParticipantsByTripId(Guid id)
    {
        var tripParticipants = _context.TripParticipants.Where(tp => tp.TripId == id);
        return await tripParticipants.ToListAsync();
    }
    public async Task DeleteTripParticipant(TripParticipant tripParticipant)
    {
        _context.Remove(tripParticipant);
        await _context.SaveChangesAsync();
    }
}