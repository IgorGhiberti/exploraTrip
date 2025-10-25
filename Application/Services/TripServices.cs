using Application.Common;
using Application.DTOs.TripDTOs;
using Application.DTOs.UserDTOs;
using Application.Interfaces;
using Domain.DomainResults;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Intfaces;
using Domain.Models;

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
        List<User> users = new List<User>();
        foreach (var userRole in tripDto.UserRoles)
        {
            var user = await _userRepository.GetUserByEmail(userRole.UserEmail);
            if (user == null)
                return ResultData<ViewTripDto>.Error("User not found.");
            users.Add(user);
        }
        
        var startDate = DateTime.SpecifyKind(tripDto.StartDate.Value, DateTimeKind.Unspecified);
        var endDate = DateTime.SpecifyKind(tripDto.EndDate.Value, DateTimeKind.Unspecified);
        var trip = Trip.CreateTrip(tripDto.Name, startDate, endDate, users[0].Email!.Value, tripDto.TripBudget, tripDto.Notes);
        if (!trip.IsSuccess)
            return ResultData<ViewTripDto>.Error(trip.Message);
        await _tripRepository.AddTrip(trip.Data!);
        for (int i = 0; i < users.Count; i++)
        {
            await AddTripParticipant(trip.Data!, users[i], tripDto.UserRoles[i].Role);

            //Envia e-mail aos convidados
            if (tripDto.UserRoles[i].Role != RoleEnum.Owner)
            {
                string subject = "Você agora participa de uma nova viagem!";
                string bodyParticipant = $"Olá {users[i].UserName}!\nAgora você também tem acesso a viagem {tripDto.Name}, boa exploração :)";
                SendEmailHelper.SendEmail(users[i].Email!.Value, bodyParticipant, subject);
            }
        }
        return ResultData<ViewTripDto>.Success(new ViewTripDto(trip.Data!.TripId, trip.Data!.Name, trip.Data!.DateStart, trip.Data!.DateEnd, tripDto.UserRoles, trip.Data!.TripBudget));
    }
    private async Task AddTripParticipant(Trip trip, User user, RoleEnum role)
    {
        TripParticipant tripParticipant = new TripParticipant(trip, user, role);
        await _tripParticipantRepository.AddTripParticipant(tripParticipant);
    }
    public async Task<ResultData<ViewTripDto>> GetTripById(Guid id)
    {
        var tripModel = await _tripRepository.GetTripModelById(id);
        if (tripModel == null)
            return ResultData<ViewTripDto>.Error("Trip not found.");
        List<UserRoleDTO> usersRoleDto = (from tp in tripModel.TripParticipantModels
                                          select new UserRoleDTO(tp.UserEmail!.Value, tp.Role)).ToList();
        return ResultData<ViewTripDto>.Success(new ViewTripDto(id, tripModel.TripName, tripModel.StartDate, tripModel.EndDate, usersRoleDto, tripModel.TripBudget));
    }
    public async Task<ResultData<ViewTripDto>> UpdateTrip(Guid id, UpdateTripDTO tripDto)
    {
        var trip = await GetTripFromRepoById(id);

        if (!trip.IsSuccess)
            return ResultData<ViewTripDto>.Error(trip.Message);

        var resultUpdate = trip.Data!.UpdateTrip(tripDto.TripName, tripDto.startDate, tripDto.endDate, tripDto.TripBudget, tripDto.Notes);

        if (!resultUpdate.IsSuccess)
            return ResultData<ViewTripDto>.Error(resultUpdate.Message);
            
        await _tripRepository.UpdateTrip(trip.Data);

        return ResultData<ViewTripDto>.Success(new ViewTripDto(trip.Data.TripId, trip.Data.Name, trip.Data.DateStart, trip.Data.DateEnd, null, trip.Data.TripBudget));
    }

    public async Task<ResultData<string>> DeleteTrip(Guid id)
    {
        var trip = await GetTripFromRepoById(id);

        if (!trip.IsSuccess)
            return ResultData<string>.Error(trip.Message);

        await _tripRepository.DeleteTrip(trip.Data!);

        List<TripParticipant> tripParticipants = await _tripParticipantRepository.GetTripParticipantsByTripId(id);

        foreach (TripParticipant tripParticipant in tripParticipants)
        {
            await _tripParticipantRepository.DeleteTripParticipant(tripParticipant);
        }
        return ResultData<string>.Success(string.Empty);
    }
    public async Task<ResultData<Trip>> GetTripFromRepoById(Guid id)
    {
        var trip = await _tripRepository.GetTripById(id);

        if (trip == null)
            return ResultData<Trip>.Error("This trip does not exist.");

        return ResultData<Trip>.Success(trip);
    }

    public async Task<ResultData<List<ViewTripDto>>> GetAllTripsByUserEmail(string email)
    {
        var trips = await _tripRepository.GetAllTripsByUserEmail(email);
        
        if (trips.Count == 0)
            return ResultData<List<ViewTripDto>>.Error("No one trip for this user.");
        
        List<ViewTripDto> tripDtos = new List<ViewTripDto>();

        foreach (var trip in trips)
        {
            tripDtos.Add(new ViewTripDto(trip!.TripID, trip.TripName, trip.StartDate, trip.EndDate, null, trip.TripBudget));
        }

        return ResultData<List<ViewTripDto>>.Success(tripDtos);
    }
}