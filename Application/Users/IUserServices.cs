using Application.Users.DTOs;

namespace Application.Users;

public interface IUserServices
{
    Task<List<ShowUserDTO>> GetAll();
    Task<ShowUserDTO> GetUserById(Guid id);
    Task ActiveUser(Guid id);
    Task AddUser(CreateUserDTO userDto, CancellationToken cancellationToken);
    Task UpdateUser(UpdateUserDTO userDto, CancellationToken cancellationToken);
    Task<bool> IsUserActive(Guid id);
    Task DisableUser(Guid id);
}