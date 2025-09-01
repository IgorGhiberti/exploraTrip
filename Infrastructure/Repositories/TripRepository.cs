using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

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
        _context.Add(trip);
        await _context.SaveChangesAsync();
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

    public async Task<TripModel?> GetTripById(Guid id)
    {
        var tripResult = await (from tp in _context.TripParticipants
                                join t in _context.Trips on tp.TripId equals id
                                join u in _context.Users on tp.UserId equals u.Id
                                select new TripModel
                                {
                                    TripName = t.Name,
                                    StartDate = t.DateStart,
                                    EndDate = t.DateEnd,
                                    TripBudget = t.TripBudget,
                                    Notes = t.Notes,
                                    TripParticipantModels = (from tripParticipant in t.TripParticipants
                                                             select new TripParticipantModel()
                                                             {
                                                                 UserName = tripParticipant.User.UserName,
                                                                 UserEmail = tripParticipant.User.Email,
                                                                 Role = tripParticipant.Role
                                                             }).OrderBy(trp => trp.Role).ToList()
                                }).FirstOrDefaultAsync();
        return tripResult;
    }

    public async Task UpdateTrip(Trip trip)
    {
        _context.Update(trip);
        await _context.SaveChangesAsync();
    }
}