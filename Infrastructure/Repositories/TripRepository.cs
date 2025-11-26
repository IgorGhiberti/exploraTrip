using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Reflection.Metadata.Ecma335;

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

    public async Task<TripModel?> GetTripModelById(Guid id)
    {
        var tripResult = await (from tp in _context.TripParticipants
                                join t in _context.Trips on tp.TripId equals t.TripId
                                where t.TripId == id
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
                                                             }).OrderBy(trp => trp.Role).ToList(),
                                    TripLocalModels = (from local in t.Locals
                                                         select new LocalModel()
                                                         {
                                                             LocalID = local.LocalId,
                                                             LocalName = local.LocalName,
                                                             LocalBudget = local.LocalBudget ?? 0,
                                                         }).ToList(),
                                }).FirstOrDefaultAsync();
        return tripResult;
    }

    public async Task UpdateTrip(Trip trip)
    {
        try
        {
            _context.Update(trip);
            await _context.SaveChangesAsync();
        } catch (Exception ex)
        {
            var error = ex.Message;
        }
        
    }
    public async Task<Trip?> GetTripById(Guid id)
    {
        return await _context.Trips.SingleOrDefaultAsync(t => t.TripId == id);
    }

    public async Task<List<TripModel?>> GetAllTripsByUserEmail(string email)
    {
        var emailResult = Email.Create(email).Data;
        var tripsResult = await (from tp in _context.TripParticipants
                                join t in _context.Trips on tp.TripId equals t.TripId
                                join u in _context.Users on tp.UserId equals u.Id
                                where u.Email == emailResult
                                select new TripModel()
                                {
                                    TripID = t.TripId,
                                    TripName = t.Name,
                                    StartDate = t.DateStart,
                                    EndDate = t.DateEnd,
                                    TripBudget = t.TripBudget
                                }).ToListAsync();
        return tripsResult;
    }
}