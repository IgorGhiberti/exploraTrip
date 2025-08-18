using Domain.Common;
using Domain.ValueObjects;

namespace Domain.User;

public class User : BaseEntity
{
    public User(string email, string userName, string hashPassword, bool active = true)
    {
        Id = Guid.NewGuid();
        Email = Email.Create(email);
        UserName = userName;
        HashPassword = hashPassword;
        Active = active;
    }
    public void ActivateUser()
    {
        Active = true;
    }
    public void DesactiveUser()
    {
        Active = false;
    }
    //Pro entity
    private User() { }
    public Guid Id { get; init; }
    public string HashPassword { get; private set; } = string.Empty;
    public Email? Email { get; private set; }
    public string UserName { get; private set; } = string.Empty;
    public bool Active { get; private set; }
}
