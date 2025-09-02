using Application.Common;
using Application.DTOs.TripDTOs;
using Application.DTOs.UserDTOs;
using Application.Interfaces;
using Domain.DomainResults;
using Domain.Entities;
using Domain.Enums;
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
        var resultValidateB = ValidateTripBudgetInfo(tripDto.Budget);
        if (!resultValidateB.IsSuccess)
            return ResultData<ViewTripDto>.Error(resultValidateB.Message);

        var resultValidateDate = ValidateTripDateInfo(tripDto.StartDate, tripDto.EndDate);
        if (!resultValidateDate.IsSuccess)
            return ResultData<ViewTripDto>.Error(resultValidateDate.Message);

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
            if (tripDto.UserRoles[i].Role != RoleEnum.Owner)
            {
                string subject = "Você agora participa de uma nova viagem!";
                string bodyParticipant = $"Olá {users[i].UserName}!\nAgora você também tem acesso a viagem {tripDto.Name}, boa exploração :)";
                SendEmailHelper.SendEmail(users[i].Email!.Value, bodyParticipant, subject);
            }
            await _tripParticipantRepository.AddTripParticipant(tripParticipant);
        }
        return ResultData<ViewTripDto>.Success(new ViewTripDto(trip.TripId, trip.Name, trip.DateStart, trip.DateEnd, tripDto.UserRoles));
    }
    private ResultData<string> ValidateTripBudgetInfo(decimal? budget)
    {
        var resultBudget = Trip.ValidateBudget(budget);
        if (!resultBudget.IsSuccess)
            return ResultData<string>.Error(resultBudget.Message);

        return ResultData<string>.Success(string.Empty);
    }
    private ResultData<string> ValidateTripDateInfo(DateTime dateStart, DateTime dateEnd)
    {
        var resultDateStart = Trip.ValidateDateStart(dateStart);
        if (!resultDateStart.IsSuccess)
            return ResultData<string>.Error(resultDateStart.Message);

        var resultDateEnd = Trip.ValidateEndDate(dateEnd, dateStart);
        if (!resultDateEnd.IsSuccess)
            return ResultData<string>.Error(resultDateEnd.Message);

        return ResultData<string>.Success(string.Empty);
    }
    public async Task<ResultData<ViewTripDto>> GetTripById(Guid id)
    {
        var tripModel = await _tripRepository.GetTripModelById(id);
        if (tripModel == null)
            return ResultData<ViewTripDto>.Error("Trip not found.");
        List<UserRoleDTO> usersRoleDto = (from tp in tripModel.TripParticipantModels
                                          select new UserRoleDTO(tp.UserEmail!.Value, tp.Role)).ToList();
        return ResultData<ViewTripDto>.Success(new ViewTripDto(id, tripModel.TripName, tripModel.StartDate, tripModel.EndDate, usersRoleDto));
    }
    public async Task<ResultData<ViewTripDto>> UpdateTrip(Guid id, UpdateTripDTO tripDto)
    {
        if (tripDto.startDate.HasValue && tripDto.endDate.HasValue)
        {
            var resultValidateDate = ValidateTripDateInfo((DateTime)tripDto.startDate, (DateTime)tripDto.endDate);
            if (!resultValidateDate.IsSuccess)
                return ResultData<ViewTripDto>.Error(resultValidateDate.Message);
        }

        if (tripDto.TripBudget.HasValue)
        {
            var resultValidateBudget = ValidateTripBudgetInfo(tripDto.TripBudget);
            if (!resultValidateBudget.IsSuccess)
                return ResultData<ViewTripDto>.Error(resultValidateBudget.Message);
        }

        var trip = await _tripRepository.GetTripById(id);

        if (trip == null)
            return ResultData<ViewTripDto>.Error("This trip does not exist.");

        trip.UpdateTrip(tripDto.TripName, tripDto.startDate, tripDto.endDate, tripDto.TripBudget, tripDto.Notes);

        await _tripRepository.UpdateTrip(trip);

        return ResultData<ViewTripDto>.Success(new ViewTripDto(trip.TripId, trip.Name, trip.DateStart, trip.DateEnd, null));
    }

    public async Task<ResultData<string>> DeleteTrip(Guid id)
    {
        var trip = await _tripRepository.GetTripById(id);

        if (trip == null)
            return ResultData<string>.Error("This trip does not exist.");

        await _tripRepository.DeleteTrip(trip);

        List<TripParticipant> tripParticipants = await _tripParticipantRepository.GetTripParticipantsByTripId(id);

        foreach (TripParticipant tripParticipant in tripParticipants)
        {
            await _tripParticipantRepository.DeleteTripParticipant(tripParticipant);
        }
        return ResultData<string>.Success(string.Empty);
    }
}