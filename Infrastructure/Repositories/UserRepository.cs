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
        public async Task ActiveUser(User user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task AddUser(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task DisableUser(User user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserById(Guid id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task UpdateUser(User user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
