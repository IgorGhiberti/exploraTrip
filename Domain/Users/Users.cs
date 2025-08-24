using Domain.Common;
using Domain.ValueObjects;

namespace Domain.User;

public class User : BaseEntity
{
    public User(string email, string userName, string hashPassword, bool active = true)
    {
        Id = Guid.NewGuid();
        Email = Email.Create(email).Data;
        UserName = userName;
        HashPassword = hashPassword;
        Active = active;
    }
    public void ActivateUser()
    {
        Active = true;
    }
    public void DisableUser()
    {
        Active = false;
    }
    public void UpdateUser(string? email, string? userName)
    {
        if (!string.IsNullOrEmpty(email))
        {
            Email = Email.Create(email).Data;
        }
        if (!string.IsNullOrEmpty(userName))
        {
            UserName = userName;
        }
    }
    public void UpdateHashPassword(string hashPassword)
    {
        HashPassword = hashPassword;
    }
    //Pro entity
    private User() { }
    public Guid Id { get; init; }
    public string HashPassword { get; private set; } = string.Empty;
    public Email? Email { get; private set; }
    public string UserName { get; private set; } = string.Empty;
    public bool Active { get; private set; }
}
