using Application.DTOs.UserDTOs;
using Domain.Enums;

namespace Application.DTOs.TripDTOs;

public record CreateTripDTO(string Name, DateTime? StartDate, DateTime? EndDate, List<UserRoleDTO> UserRoles, decimal? Budget, string[]? Notes);