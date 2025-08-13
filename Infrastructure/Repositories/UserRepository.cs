using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.User;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public List<User> users = new();
        public void ActiveUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public void AddUser(User user)
        {
            users.Add(user);
        }

        public void DisableUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserActive(Guid id)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}