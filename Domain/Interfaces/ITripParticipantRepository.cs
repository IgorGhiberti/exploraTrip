using Domain.Entities;

namespace Domain.Interfaces;

public interface ITripParticipantRepository
{
    Task AddTripParticipant(TripParticipant tripParticipant);
    Task<List<TripParticipant>> GetTripParticipantsByTripId(Guid id);
    Task DeleteTripParticipant(TripParticipant tripParticipant);
}