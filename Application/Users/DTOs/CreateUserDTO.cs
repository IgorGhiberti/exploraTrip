using Domain.ValueObjects;

namespace Application.Users
{
    public record CreateUserDTO(string Name, string Email);
}