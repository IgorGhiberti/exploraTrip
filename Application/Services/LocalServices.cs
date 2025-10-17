using Application.DTOs.LocalDTOs;
using Application.Interfaces;
using Domain.DomainResults;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

internal class LocalServices : ILocalService
{
    private readonly ILocalRepository _localRepository;
    private readonly ITripServices _tripServices;
    private readonly ITripRepository _tripRepository;

    public LocalServices(ILocalRepository localRepository, ITripServices tripServices, ITripRepository  tripRepository)
    {
        _localRepository = localRepository;
        _tripServices = tripServices;
        _tripRepository = tripRepository;
    }

    public async Task<ResultData<ViewLocalDTO>> AddTrip(CreateLocalDTO localDto)
    {
        var result = await _tripServices.GetTripById(localDto.TripId);
        if (!result.IsSuccess)
            return ResultData<ViewLocalDTO>.Error(result.Message);

        var trip = await _tripRepository.GetTripById(localDto.TripId);
        var local = Local.CreateLocal(localDto.LocalName, localDto.DateStart, localDto.DateEnd, trip!, localDto.LocalBudget, localDto.Notes);
        
        if (!local.IsSuccess)
            return ResultData<ViewLocalDTO>.Error(local.Message);

        await _localRepository.AddLocal(local.Data);

        return ResultData<ViewLocalDTO>.Success(new ViewLocalDTO(local.Data.LocalId, local.Data.LocalName,  local.Data.DateStart, local.Data.DateEnd, local.Data.Notes, local.Data.LocalBudget));
    }

    public async Task<ResultData<ViewLocalDTO>> Update(UpdateLocalDTO localDto)
    {
        var local = await _localRepository.GetLocalById(localDto.LocalId);
        if (local == null)
            return ResultData<ViewLocalDTO>.Error("Local not found");
        
        var result = await _tripServices.GetTripById(localDto.TripId);
        if (!result.IsSuccess)
            return ResultData<ViewLocalDTO>.Error(result.Message);
        
        var trip = await _tripRepository.GetTripById(localDto.TripId);
        
        local.UpdateLocal(localDto.LocalName, localDto.DateStart, localDto.DateEnd, trip!, localDto.LocalBudget, localDto.Notes);

        await _localRepository.UpdateLocal(local);
        return ResultData<ViewLocalDTO>.Success(new ViewLocalDTO(local.LocalId, local.LocalName,  local.DateStart, local.DateEnd, local.Notes, local.LocalBudget));
    }

    public async Task<ResultData<List<ViewLocalDTO>>> ListLocalByTrip(Guid tripId)
    {
        var localsByTrip = await _localRepository.GetAllLocalsByTrip(tripId);
        var result = from l in localsByTrip
            select new ViewLocalDTO(l.LocalId, l.LocalName, l.DateStart, l.DateEnd, l.Notes, l.LocalBudget);
        return ResultData<List<ViewLocalDTO>>.Success(result.ToList());
    }

    public async Task<ResultData<bool>> DeleteTrip(Guid localId)
    {
        var local = await _localRepository.GetLocalById(localId);
        if (local == null)
            return ResultData<bool>.Error("Local not found");
        
        await _localRepository.DeleteLocal(local);
        
        return ResultData<bool>.Success(true);
    }
}