using Application.DTOs.LocalDTOs;
using Application.DTOs.TripDTOs;
using Domain.DomainResults;

namespace Application.Interfaces;

public interface ILocalService
{
    Task<ResultData<ViewLocalDTO>> AddTrip(CreateLocalDTO localDto);
    Task<ResultData<ViewLocalDTO>> Update(UpdateLocalDTO localDto);
    Task<ResultData<List<ViewLocalDTO>>> ListLocalByTrip(Guid tripId);
    Task<ResultData<bool>> DeleteTrip(Guid localId);   
}