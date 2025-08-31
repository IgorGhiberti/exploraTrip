using Domain.Enums;

namespace Application.DTOs.UserDTOs;

public record UserRoleDTO(string UserEmail, RoleEnum Role);