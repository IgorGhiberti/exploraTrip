using Domain.Entities;
using Domain.Models;

namespace Domain.Interfaces;

public interface ITripRepository
{
    Task AddTrip(Trip trip);
    Task<List<Trip>> GetAllTrips();
    Task<TripModel?> GetTripById(Guid id);
    Task UpdateTrip(Trip trip);
    Task DeleteTrip(Trip trip);
}