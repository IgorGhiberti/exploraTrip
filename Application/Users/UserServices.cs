using System.Threading.Tasks;
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

    public async Task<List<User>> GetAll()
    {
        return await _userRepository.GetAll();
    }

    public async Task AddUser(CreateUserDTO userDto)
    {
        if (Email.Create(userDto.Email) is not Email email)
        {
            throw new ArgumentException("Email inválido");
        }

        User newUser = new User(new Guid(), email, userDto.Name);
        _userRepository.AddUser(newUser);
        await _unitOfWork.SaveChangesAsync();
    }
}