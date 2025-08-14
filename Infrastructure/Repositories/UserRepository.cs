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
            var user = users.SingleOrDefault(u => u.Id == id);
            int index = users.IndexOf(user);
            users[index] = new User(user.Id, user.Email, user.UserName, true);
        }

        public void AddUser(User user)
        {
            users.Add(user);
        }

        public void DisableUser(Guid id)
        {
            var user = users.SingleOrDefault(u => u.Id == id);
            int index = users.IndexOf(user);
            users[index] = new User(user.Id, user.Email, user.UserName, false);
        }

        public async Task<List<User>> GetAll()
        {
            return await Task.FromResult(users);
        }

        public async Task<User> GetUserById(Guid id)
        {
            var user = users.SingleOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("Usuário não existe");
            }
            return await Task.FromResult(user);
        }

        public async Task<bool> IsUserActive(Guid id)
        {
            var user = await GetUserById(id);
            return user.Active;
        }

        public void UpdateUser(User user)
        {
            int index = users.IndexOf(user);
            users[index] = user;
        }
    }
}