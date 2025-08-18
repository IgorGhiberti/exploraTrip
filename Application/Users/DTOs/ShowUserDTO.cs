using Domain.ValueObjects;

namespace Application.Users.DTOs;
public record ShowUserDTO(string UserName, string Email, bool IsActive);