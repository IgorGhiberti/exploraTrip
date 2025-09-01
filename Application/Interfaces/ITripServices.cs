using Application.DTOs.TripDTOs;
using Domain.DomainResults;
using Domain.Entities;

namespace Application.Interfaces;

public interface ITripServices
{
    Task<ResultData<ViewTripDto>> AddTrip(CreateTripDTO tripDto);
    Task<ResultData<ViewTripDto>> GetTripById(Guid id);
}