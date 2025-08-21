using Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Helpers;

public class Cryptography : IPasswordCryptography
{
    private readonly PasswordHasher<string> _passwordHasher = new();
    public string CreateHash(string password, string email)
    {
        string hashedPass = _passwordHasher.HashPassword(email, password);
        return hashedPass;
    }
    public bool ValidateHash(string password, string storedHash, string email)
    {
        return _passwordHasher.VerifyHashedPassword(email, storedHash, password) == PasswordVerificationResult.Success;
    }
}