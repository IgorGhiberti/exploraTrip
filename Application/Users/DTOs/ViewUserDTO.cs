using Domain.ValueObjects;

namespace Application.Users.DTOs;
public record ViewUserDTO(Guid Id, string UserName, string Email, bool IsActive);