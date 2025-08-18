using System.Threading.Tasks;
using Application.Users.DTOs;
using Domain.Common;
using Domain.User;
using Domain.ValueObjects;

namespace Application.Users;

internal class UserServices : IUserServices
{
    private readonly IUserRepository _userRepository;
    public UserServices(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<ShowUserDTO>> GetAll()
    {
        List<User> users = await _userRepository.GetAll();
        var result = from u in users
                     select new ShowUserDTO(u.UserName, u.Email!.Value, u.Active);
        return result.ToList();
    }
    public async Task<string> GetLoginInfo(Guid id)
    {
        var user = await _userRepository.GetUserById(id);
        return user.HashPassword;
    }

    public async Task<ShowUserDTO> GetUserById(Guid id)
    {
        User user = await _userRepository.GetUserById(id);
        return new ShowUserDTO(user.UserName, user.Email!.Value, user.Active);
    }

    public async Task ActiveUser(Guid id)
    {
        await _userRepository.ActiveUser(id);
    }
    public async Task DisableUser(Guid id)
    {
        await _userRepository.DisableUser(id);
    }
    public async Task<bool> IsUserActive(Guid id)
    {
        return await _userRepository.IsUserActive(id);
    }
    public async Task AddUser(CreateUserDTO userDto, CancellationToken cancellationToken)
    {
        User newUser = new User(userDto.EmailVal, userDto.Name, userDto.Password);
        await _userRepository.AddUser(newUser);
    }

    public async Task UpdateUser(Guid id, UpdateUserDTO userDto, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserById(id);
        if (user == null)
        {
            throw new Exception("Usuário não existe, não é possível atualizar.");
        }
        user.UpdateUser(userDto.EmailVal, userDto.Name, userDto.Password);
        await _userRepository.UpdateUser(user);
    }
}