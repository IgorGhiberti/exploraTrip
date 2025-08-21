using Application.Interfaces;
using Application.Users.DTOs;
using Domain.DomainResults;
using Domain.User;
using Domain.ValueObjects;

namespace Application.Users;

internal class UserServices : IUserServices
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordCryptography _passwordHelper;
    public UserServices(IUserRepository userRepository, IPasswordCryptography passwordHelper)
    {
        _userRepository = userRepository;
        _passwordHelper = passwordHelper;
    }

    public async Task<ResultData<List<ShowUserDTO>>> GetAll()
    {
        List<User> users = await _userRepository.GetAll();
        if (!users.Any())
            return ResultData<List<ShowUserDTO>>.Error("Nenhum usuário cadastrado.");
        var result = from u in users
                     select new ShowUserDTO(u.UserName, u.Email!.Value, u.Active);
        return ResultData<List<ShowUserDTO>>.Success(result.ToList());
    }
    public async Task<ResultData<string>> AuthenticateUser(LoginUserDTO userDto)
    {
        var resultUserRepo = await GetUserRepoById(userDto.Id);
        if (!resultUserRepo.IsSuccess)
            return ResultData<string>.Error(resultUserRepo.Message);
        string storedHash = resultUserRepo.Data!.HashPassword;
        bool isCorrectPassword = _passwordHelper.ValidateHash(userDto.Password, storedHash, userDto.Email);
        return isCorrectPassword ? ResultData<string>.Success("Usuário autenticado com sucesso!") : ResultData<string>.Error("Falha na autenticação");
    }

    public async Task<ResultData<ShowUserDTO>> GetUserById(Guid id)
    {
        var user = await GetUserRepoById(id);
        if(!user.IsSuccess)
            return ResultData<ShowUserDTO>.Error(user.Message);
        return ResultData<ShowUserDTO>.Success(new ShowUserDTO(user.Data!.UserName, user.Data.Email!.Value, user.Data.Active));
    }
    public async Task<ResultData<User>> GetUserRepoById(Guid id)
    {
        User user = await _userRepository.GetUserById(id);
        if (user == null)
        {
            return ResultData<User>.Error("Usuário não existe.");
        }
        return ResultData<User>.Success(user);
    }

    public async Task<ResultData<ShowUserDTO>> ActiveUser(Guid id)
    {
        var user = await GetUserRepoById(id);
        if (!user.IsSuccess)
            return ResultData<ShowUserDTO>.Error(user.Message);
        user.Data!.ActivateUser();
        return ResultData<ShowUserDTO>.Success(new ShowUserDTO(user.Data!.UserName, user.Data.Email!.Value, user.Data.Active));
    }
    public async Task<ResultData<ShowUserDTO>> DisableUser(Guid id)
    {
        var user = await GetUserRepoById(id);
        if (!user.IsSuccess)
            return ResultData<ShowUserDTO>.Error(user.Message);
        user.Data!.DisableUser();
        return ResultData<ShowUserDTO>.Success(new ShowUserDTO(user.Data!.UserName, user.Data.Email!.Value, user.Data.Active));
    }
    public async Task<ResultData<bool>> IsUserActive(Guid id)
    {
        var user = await GetUserRepoById(id);
        if (!user.IsSuccess)
            return ResultData<bool>.Error(user.Message);
        return ResultData<bool>.Success(user.Data!.Active);
    }
    public async Task<ResultData<CreateUserDTO>> AddUser(CreateUserDTO userDto, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(userDto.EmailVal);
        if (!emailResult.IsSuccess)
        {
            return ResultData<CreateUserDTO>.Error(emailResult.Message);
        }
        string hashPassword = _passwordHelper.CreateHash(userDto.Password, userDto.EmailVal);
        var dtoWithHash = userDto with { Password = hashPassword };
        User newUser = new User(dtoWithHash.EmailVal, dtoWithHash.Name, dtoWithHash.Password);
        CreateUserDTO dtoWithId = dtoWithHash with { Id = newUser.Id };
        await _userRepository.AddUser(newUser);
        return ResultData<CreateUserDTO>.Success(dtoWithId);
    }

    public async Task<ResultData<ShowUserDTO>> UpdateUser(Guid id, UpdateUserDTO userDto, CancellationToken cancellationToken)
    {
        var user = await GetUserRepoById(id);
        if (!user.IsSuccess)
            return ResultData<ShowUserDTO>.Error(user.Message);
        user.Data!.UpdateUser(userDto.EmailVal, userDto.Name, userDto.Password);
        await _userRepository.UpdateUser(user.Data);
        return ResultData<ShowUserDTO>.Success(new ShowUserDTO(user.Data!.UserName, user.Data.Email!.Value, user.Data.Active));
    }
}