using Domain.Entities;
using Domain.Interfaces;

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
        try
        {
           _context.Add(tripParticipant);
            await _context.SaveChangesAsync(); 
        }
        catch (System.Exception ex)
        {
            
            throw new Exception(ex.Message);
        }
        
    }
}