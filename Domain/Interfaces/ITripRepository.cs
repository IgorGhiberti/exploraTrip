using Domain.Entities;

namespace Domain.Interfaces;

public interface ITripRepository
{
    Task AddTrip(Trip trip);
    Task<List<Trip>> GetAllTrips();
    Task<Trip?> GetTripById(Guid id);
    Task UpdateTrip(Trip trip);
    Task DeleteTrip(Trip trip);
}