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
    public static bool ValidateHash(string password, string storedHash, string email)
    {
        return _passwordHasher.VerifyHashedPassword(email, storedHash, password) == PasswordVerificationResult.Success;
    }
}