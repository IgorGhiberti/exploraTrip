using Application.Users.DTOs;

namespace Application.Users;

public interface IUserServices
{
    Task<List<ShowUserDTO>> GetAll();
    Task<ShowUserDTO> GetUserById(Guid id);
    Task ActiveUser(Guid id);
    Task AddUser(CreateUserDTO userDto, CancellationToken cancellationToken);
    Task UpdateUser(Guid id, UpdateUserDTO userDto, CancellationToken cancellationToken);
    Task<bool> IsUserActive(Guid id);
    Task DisableUser(Guid id);
    Task<string> GetLoginInfo(Guid id);
}