using System.Threading.Tasks;
using Application.Common;
using Application.Enum;
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
    private readonly ICache _cache;
    public UserServices(IUserRepository userRepository, IPasswordCryptography passwordHelper, ICache cache)
    {
        _userRepository = userRepository;
        _passwordHelper = passwordHelper;
        _cache = cache;
    }

    public async Task<ResultData<List<ViewUserDTO>>> GetAll()
    {
        List<User> users = await _userRepository.GetAll();
        if (!users.Any())
            return ResultData<List<ViewUserDTO>>.Error("Nenhum usuário cadastrado.");
        var result = from u in users
                     select new ViewUserDTO(u.Id, u.UserName, u.Email!.Value, u.Active);
        return ResultData<List<ViewUserDTO>>.Success(result.ToList());
    }
    public async Task<ResultData<string>> AuthenticateUser(LoginUserDTO userDto)
    {
        var resultUserRepo = await GetUserByEmail(userDto.Email);
        if (!resultUserRepo.IsSuccess)
            return ResultData<string>.Error(resultUserRepo.Message);
        string storedHash = resultUserRepo.Data!.HashPassword;
        bool isCorrectPassword = _passwordHelper.ValidateHash(userDto.Password, storedHash, userDto.Email);
        return isCorrectPassword ? ResultData<string>.Success("Usuário autenticado com sucesso!") : ResultData<string>.Error("Falha na autenticação");
    }

    public async Task<ResultData<ViewUserDTO>> GetUserById(Guid id)
    {
        var user = await GetUserRepoById(id);
        if (!user.IsSuccess)
            return ResultData<ViewUserDTO>.Error(user.Message);
        return ResultData<ViewUserDTO>.Success(new ViewUserDTO(user.Data!.Id, user.Data!.UserName, user.Data.Email!.Value, user.Data.Active));
    }
    public async Task<ResultData<User>> GetUserRepoById(Guid id)
    {
        User? user = await _userRepository.GetUserById(id);
        if (user == null)
        {
            return ResultData<User>.Error("Usuário não existe.");
        }
        return ResultData<User>.Success(user);
    }

    public async Task<ResultData<ViewUserDTO>> ActiveUser(Guid id)
    {
        var user = await GetUserRepoById(id);
        if (!user.IsSuccess)
            return ResultData<ViewUserDTO>.Error(user.Message);
        user.Data!.ActivateUser();
        user.Data!.UpdatePropertiesByAndDate(user.Data.Email!.Value);
        await _userRepository.UpdateUser(user.Data);
        return ResultData<ViewUserDTO>.Success(new ViewUserDTO(user.Data!.Id, user.Data!.UserName, user.Data.Email!.Value, user.Data.Active));
    }
    public async Task<ResultData<ViewUserDTO>> DisableUser(Guid id)
    {
        var user = await GetUserRepoById(id);
        if (!user.IsSuccess)
            return ResultData<ViewUserDTO>.Error(user.Message);
        user.Data!.DisableUser();
        user.Data!.UpdatePropertiesByAndDate(user.Data.Email!.Value);
        await _userRepository.UpdateUser(user.Data);
        return ResultData<ViewUserDTO>.Success(new ViewUserDTO(user.Data!.Id, user.Data!.UserName, user.Data.Email!.Value, user.Data.Active));
    }
    public async Task<ResultData<bool>> IsUserActive(Guid id)
    {
        var user = await GetUserRepoById(id);
        if (!user.IsSuccess)
            return ResultData<bool>.Error(user.Message);
        return ResultData<bool>.Success(user.Data!.Active);
    }
    public async Task<ResultData<ViewUserDTO>> AddUser(CreateUserDTO userDto, CancellationToken cancellationToken)
    {
        var user = await GetUserByEmail(userDto.EmailVal);
        if (user.IsSuccess)
            return ResultData<ViewUserDTO>.Error("Usuário já cadastrado no sistema.");
        var emailResult = Email.Create(userDto.EmailVal);
        if (!emailResult.IsSuccess)
        {
            return ResultData<ViewUserDTO>.Error(emailResult.Message);
        }
        string hashPassword = _passwordHelper.CreateHash(userDto.Password, userDto.EmailVal);
        var dtoWithHash = userDto with { Password = hashPassword };
        // Inativo até confirmar o código
        User newUser = new User(dtoWithHash.EmailVal, dtoWithHash.Name, dtoWithHash.Password, dtoWithHash.EmailVal, false);
        await _userRepository.AddUser(newUser);
        // Enviar e-mail de confirmação
        int randomCode = new Random().Next(100, 999);
        string emailBody = $"Seja bem vindo! \n Segue o código para confirmação {randomCode}";
        string emailSubject = $"Confirmação de cadastro";
        SendEmailHelper.SendEmail(userDto.EmailVal, emailBody, emailSubject);
        // Salva em cache o randomCode
        _cache.StoreRandomNumber(randomCode);
        ViewUserDTO viewDto = new(newUser.Id, dtoWithHash.Name, dtoWithHash.EmailVal, false);
        return ResultData<ViewUserDTO>.Success(viewDto);
    }
    public async Task<ResultData<User>> GetUserByEmail(string email)
    {
        var user = await _userRepository.GetUserByEmail(email);
        if (user == null)
            return ResultData<User>.Error("Email não cadastrado no sistema.");
        return ResultData<User>.Success(user);
    }

    public async Task<ResultData<ViewUserDTO>> UpdateUser(Guid id, UpdateUserDTO userDto, CancellationToken cancellationToken)
    {
        var user = await GetUserRepoById(id);
        if (!user.IsSuccess)
            return ResultData<ViewUserDTO>.Error(user.Message);

        user.Data!.UpdateUser(userDto.EmailVal, userDto.Name);
        user.Data!.UpdatePropertiesByAndDate(userDto.EmailVal!);
        await _userRepository.UpdateUser(user.Data);
        return ResultData<ViewUserDTO>.Success(new ViewUserDTO(user.Data!.Id, user.Data!.UserName, user.Data.Email!.Value, user.Data.Active));
    }
    public async Task<ResultData<string>> UpdatePassword(Guid id, UpdatePasswordDTO userDto, CancellationToken cancellationToken)
    {
        var user = await GetUserRepoById(id);
        if (!user.IsSuccess)
            return ResultData<string>.Error(user.Message);

        if (!string.IsNullOrEmpty(userDto.OldPassword))
        {
            bool isOldPasswordCorrect = _passwordHelper.ValidateHash(userDto.OldPassword, user.Data!.HashPassword, userDto.Email);

            if (!isOldPasswordCorrect)
                return ResultData<string>.Error("A senha antiga não está correta.");
        }

        string newPasswordHash = _passwordHelper.CreateHash(userDto.Password, userDto.Email);
        user.Data!.UpdateHashPassword(newPasswordHash);
        user.Data!.UpdatePropertiesByAndDate(userDto.Email);
        await _userRepository.UpdateUser(user.Data);
        return ResultData<string>.Success("Senha alterada com sucesso!");
    }
    public async Task<ResultData<bool>> ConfirmEmailCode(string userEmail, int code, OperationEnum operationEnum)
    {
        int? codeByCache = _cache.GetRandomNumber();
        bool equalCode = codeByCache == code;
        var user = await GetUserByEmail(userEmail);
        if (!user.IsSuccess)
            return ResultData<bool>.Error(user.Message);
        if (equalCode && operationEnum == OperationEnum.ActiveOperation)
        {
            await ActiveUser(user.Data!.Id);
            return ResultData<bool>.Success(true);
        }
        if (equalCode && operationEnum == OperationEnum.UpdateOperation)
        {
            _cache.StoreEmail(userEmail);
            return ResultData<bool>.Success(true);
        }
        return ResultData<bool>.Error("Código inválido.");
    }
    public async Task<ResultData<string>> ForgetPassword(string email)
    {
        var user = await GetUserByEmail(email);
        if (!user.IsSuccess)
            return ResultData<string>.Error(user.Message);

        // Enviar e-mail de confirmação
        int randomCode = new Random().Next(100, 999);
        string emailBody = $"Seja bem vindo! \nSegue o código para confirmação {randomCode}";
        string emailSubject = $"Troca de senha";
        SendEmailHelper.SendEmail(email, emailBody, emailSubject);
        // Salva em cache o randomCode
        _cache.StoreRandomNumber(randomCode);

        return ResultData<string>.Success("Código enviado com sucesso!");
    }
    public async Task<ResultData<ViewUserDTO>> ResetPassword(UpdatePasswordDTO userDto)
    {
        string? userEmail = _cache.GetUserEmail();
        if (userEmail == null)
            return ResultData<ViewUserDTO>.Error("Código inválido ou expirado.");
        var user = await GetUserByEmail(userEmail);
        if (!user.IsSuccess)
            return ResultData<ViewUserDTO>.Error(user.Message);
        await UpdatePassword(user.Data!.Id, userDto, CancellationToken.None);
        return ResultData<ViewUserDTO>.Success(new ViewUserDTO(user.Data!.Id, user.Data!.UserName, userEmail, true));
    }
}