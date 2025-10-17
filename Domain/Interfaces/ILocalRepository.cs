using Domain.Entities;

namespace Domain.Interfaces;

public interface ILocalRepository
{
    Task AddLocal(Local local);
    Task UpdateLocal(Local local);
    Task DeleteLocal(Local local);
    Task<List<Local>> GetAllLocalsByTrip(Guid tripId);
    Task<Local?> GetLocalById(Guid localId);
}