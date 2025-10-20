using Application.DTOs.LocalDTOs;
using Application.DTOs.TripDTOs;
using Domain.DomainResults;

namespace Application.Interfaces;

public interface ILocalService
{
    Task<ResultData<ViewLocalDTO>> AddLocal(CreateLocalDTO localDto);
    Task<ResultData<ViewLocalDTO>> UpdateLocal(UpdateLocalDTO localDto);
    Task<ResultData<List<ViewLocalDTO>>> ListLocalByTrip(Guid tripId);
    Task<ResultData<bool>> DeleteLocal(Guid localId);   
}