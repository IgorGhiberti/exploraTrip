using Domain.Common;
using Domain.ValueObjects;

namespace Domain.User
{

    public class User : BaseEntity
    {
        public User(Guid id, Email email, string userName)
        {
            Id = id;
            Email = email;
            UserName = userName;
        }
        public User()
        {

        }
        public Guid Id { get; private set; }
        public Email? Email { get; private set; }
        public string UserName { get; private set; } = string.Empty;
        public bool Active { get; private set; } = true;
    }
}