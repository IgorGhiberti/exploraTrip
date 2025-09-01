using Application.DTOs.TripDTOs;
using Application.Interfaces;
using Domain.DomainResults;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Intfaces;

namespace Application.Services;

internal class TripServices : ITripServices
{
    private readonly IUserRepository _userRepository;
    private readonly ITripRepository _tripRepository;
    private readonly ITripParticipantRepository _tripParticipantRepository;

    public TripServices(IUserRepository userRepository, ITripRepository tripRepository, ITripParticipantRepository tripParticipantRepository)
    {
        _userRepository = userRepository;
        _tripRepository = tripRepository;
        _tripParticipantRepository = tripParticipantRepository;
    }
    public async Task<ResultData<ViewTripDto>> AddTrip(CreateTripDTO tripDto)
    {
        var resultValidate = ValidateTripInfo(tripDto.Budget, tripDto.StartDate, tripDto.EndDate);
        if (!resultValidate.IsSuccess)
            return ResultData<ViewTripDto>.Error(resultValidate.Message);
        List<User> users = new List<User>();
        foreach (var userRole in tripDto.UserRoles)
        {
            var user = await _userRepository.GetUserByEmail(userRole.UserEmail);
            if (user == null)
                return ResultData<ViewTripDto>.Error("User not found.");
            users.Add(user);
        }
        var trip = new Trip(tripDto.Name, tripDto.StartDate.ToUniversalTime(), tripDto.EndDate.ToUniversalTime(), users[0].Email!.Value, tripDto.Budget, tripDto.Notes);
        await _tripRepository.AddTrip(trip);
        for (int i = 0; i < users.Count; i++)
        {
            var tripParticipant = new TripParticipant(trip, users[i], tripDto.UserRoles[i].Role);
            await _tripParticipantRepository.AddTripParticipant(tripParticipant);
        }
        return ResultData<ViewTripDto>.Success(new ViewTripDto(trip.TripId, trip.Name, trip.DateStart, trip.DateEnd, tripDto.UserRoles));
    }

    private ResultData<string> ValidateTripInfo(decimal? budget, DateTime dateStart, DateTime dateEnd)
    {
        var resultBudget = Trip.ValidateBudget(budget);
        if (!resultBudget.IsSuccess)
            return ResultData<string>.Error(resultBudget.Message);

        var resultDateStart = Trip.ValidateDateStart(dateStart);
        if (!resultDateStart.IsSuccess)
            return ResultData<string>.Error(resultDateStart.Message);

        var resultDateEnd = Trip.ValidateEndDate(dateEnd, dateStart);
        if (!resultDateEnd.IsSuccess)
            return ResultData<string>.Error(resultDateEnd.Message);

        return ResultData<string>.Success(string.Empty);
    }
}