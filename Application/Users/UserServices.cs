using System.Threading.Tasks;
using Application.Users.DTOs;
using Domain.Common;
using Domain.User;
using Domain.ValueObjects;

namespace Application.Users;

public class UserServices
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UserServices(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    private Email CreateEmailUser(string emailUserVal)
    {
        if (Email.Create(emailUserVal) is not Email email)
        {
            throw new ArgumentException("Email inválido");
        }
        return email;
    }
    public async Task<List<User>> GetAll()
    {
        return await _userRepository.GetAll();
    }

    public async Task GetUserById(Guid id)
    {
        await _userRepository.GetUserById(id);
    }

    public async Task ActiveUser()
    {
        
    }

    public async Task AddUser(CreateUserDTO userDto, CancellationToken cancellationToken)
    {
        User newUser = new User(new Guid(), CreateEmailUser(userDto.Email), userDto.Name, true);
        _userRepository.AddUser(newUser);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUser(UpdateUserDTO userDto, CancellationToken cancellationToken)
    {

    }
}