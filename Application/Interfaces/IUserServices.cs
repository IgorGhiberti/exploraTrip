using Application.Users;
using Application.Users.DTOs;
using Domain.DomainResults;
using Domain.User;
namespace Application.Interfaces;

public interface IUserServices
{
    Task<ResultData<List<ShowUserDTO>>> GetAll();
    Task<ResultData<string>> AuthenticateUser(LoginUserDTO userDto);
    Task<ResultData<ShowUserDTO>> GetUserById(Guid id);
    Task<ResultData<User>> GetUserRepoById(Guid id);
    Task<ResultData<ShowUserDTO>> ActiveUser(Guid id);
    Task<ResultData<ShowUserDTO>> DisableUser(Guid id);
    Task<ResultData<bool>> IsUserActive(Guid id);
    Task<ResultData<CreateUserDTO>> AddUser(CreateUserDTO userDto, CancellationToken cancellationToken);
    Task<ResultData<ShowUserDTO>> UpdateUser(Guid id, UpdateUserDTO userDto, CancellationToken cancellationToken);
}