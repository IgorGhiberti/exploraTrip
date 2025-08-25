using System.Net;
using System.Net.Mail;
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
        await _userRepository.UpdateUser(user.Data);
        return ResultData<ViewUserDTO>.Success(new ViewUserDTO(user.Data!.Id, user.Data!.UserName, user.Data.Email!.Value, user.Data.Active));
    }
    public async Task<ResultData<ViewUserDTO>> DisableUser(Guid id)
    {
        var user = await GetUserRepoById(id);
        if (!user.IsSuccess)
            return ResultData<ViewUserDTO>.Error(user.Message);
        user.Data!.DisableUser();
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
        User newUser = new User(dtoWithHash.EmailVal, dtoWithHash.Name, dtoWithHash.Password);
        await _userRepository.AddUser(newUser);
        ViewUserDTO viewDto = new(newUser.Id, dtoWithHash.Name, dtoWithHash.EmailVal, true);
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
        await _userRepository.UpdateUser(user.Data);
        return ResultData<ViewUserDTO>.Success(new ViewUserDTO(user.Data!.Id, user.Data!.UserName, user.Data.Email!.Value, user.Data.Active));
    }
    public async Task<ResultData<string>> UpdatePassword(Guid id, UpdatePasswordDTO userDto, CancellationToken cancellationToken)
    {
        var user = await GetUserRepoById(id);
        if (!user.IsSuccess)
            return ResultData<string>.Error(user.Message);

        bool isOldPasswordCorrect = _passwordHelper.ValidateHash(userDto.OldPassword, user.Data!.HashPassword, userDto.Email);

        if (!isOldPasswordCorrect)
            return ResultData<string>.Error("A senha antiga não está correta.");

        string newPasswordHash = _passwordHelper.CreateHash(userDto.Password, userDto.Email);
        user.Data.UpdateHashPassword(newPasswordHash);
        await _userRepository.UpdateUser(user.Data);
        return ResultData<string>.Success("Senha alterada com sucesso!");
    }
    private void SendEmailRegister(string userEmail, string userName)
    {
        using (SmtpClient smtp = new SmtpClient())
        {
            smtp.Host = "";
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("igu.ghiberti@gmail.com", "myeu wqtw glhg beol");

            using (MailMessage msg = new MailMessage())
            {
                msg.From = new MailAddress("igu.ghiberti@gmail.com", "Explora Trip");
                msg.To.Add(new MailAddress(userEmail));
                msg.Subject = "Cadastro Explora Trip";
                Random randomNumber = new Random();
                msg.Body = $"Olá {userName}! \n Por favor, para confirmar o seu cadastro, confirme o seguinte código na página de cadastro {randomNumber}";
                smtp.Send(msg);
            }
        }
    }
}