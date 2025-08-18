using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;
        public UserRepository(ApplicationDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task ActiveUser(Guid id)
        {
            //TODO: pegar usuário por ID e ativá-lo deve ser feito no service.
            var user = await GetUserById(id);
            user.ActivateUser();
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task AddUser(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task DisableUser(Guid id)
        {
            //TODO: pegar usuário por ID e ativá-lo deve ser feito no service.
            var user = await GetUserById(id);
            user.DesactiveUser();
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserById(Guid id)
        {
            //Se o usuário não existir, retornar null. E quem trata isso deve ser
            //o services
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("Usuário não existe");
            }
            return user;
        }

        public async Task<bool> IsUserActive(Guid id)
        {
            var user = await GetUserById(id);
            return user.Active;
        }

        public async Task UpdateUser(User user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}