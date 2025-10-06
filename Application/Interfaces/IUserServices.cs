using Application.Enum;
using Application.Users;
using Application.Users.DTOs;
using Domain.DomainResults;
using Domain.Entities;
namespace Application.Interfaces;

public interface IUserServices
{
    Task<ResultData<List<ViewUserDTO>>> GetAll();
    Task<ResultData<string>> AuthenticateUser(LoginUserDTO userDto);
    Task<ResultData<ViewUserDTO>> GetUserById(Guid id);
    Task<ResultData<User>> GetUserFromRepoById(Guid id);
    Task<ResultData<ViewUserDTO>> ActiveUser(Guid id);
    Task<ResultData<ViewUserDTO>> DisableUser(Guid id);
    Task<ResultData<bool>> IsUserActive(Guid id);
    Task<ResultData<ViewUserDTO>> AddUser(CreateUserDTO userDto, CancellationToken cancellationToken);
    Task<ResultData<ViewUserDTO>> UpdateUser(Guid id, UpdateUserDTO userDto, CancellationToken cancellationToken);
    Task<ResultData<string>> UpdatePassword(Guid id, UpdatePasswordDTO userDto, CancellationToken cancellationToken);
    Task<ResultData<bool>> ConfirmEmailCode(string userEmail, int code, OperationEnum operationEnum);
    Task<ResultData<string>> ForgetPassword(string email);
    Task<ResultData<ViewUserDTO>> ResetPassword(UpdatePasswordDTO userDto);
    Task<ResultData<ViewUserDTO>> ShowUserByEmail(string email);
}