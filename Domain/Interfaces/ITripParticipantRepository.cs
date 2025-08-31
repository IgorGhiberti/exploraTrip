using Domain.Entities;

namespace Domain.Interfaces;

public interface ITripParticipantRepository
{
    Task AddTripParticipant(TripParticipant tripParticipant);
}