namespace Application.Interfaces;

public interface IPasswordCryptography
{
    string CreateHash(string password, string email);
    bool ValidateHash(string password, string storedHash, string email);
}