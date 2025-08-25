namespace Application.Interfaces;

public interface ICache
{
    void StoreRandomNumber(int value);
    int? GetRandomNumber();
    public void StoreEmail(string email);
    public string? GetUserEmail();
}