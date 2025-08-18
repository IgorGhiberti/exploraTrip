using Microsoft.AspNetCore.Identity;

namespace WebApi.Helpers;

public static class Cryptography
{
    private static readonly PasswordHasher<string> _passwordHasher = new();
    public static string CreateHash(string password, string email)
    {
        string hashedPass = _passwordHasher.HashPassword(email, password);
        return hashedPass;
    }
    public static bool ValidateHash(string password, string email)
    {
        var hashedPassword = CreateHash(password, email);
        return _passwordHasher.VerifyHashedPassword(email, hashedPassword, password) == PasswordVerificationResult.Success;
    }
}