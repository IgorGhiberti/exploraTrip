using Application.DTOs.UserDTOs;
using Domain.Enums;

namespace Application.DTOs.TripDTOs;

public record ViewTripDto(Guid? Id, string? Name, DateTime? StartDate, DateTime? EndDate, List<UserRoleDTO>? UsersRolesDTO, decimal? TripBudget, decimal? TripBudgetAvailable = null);