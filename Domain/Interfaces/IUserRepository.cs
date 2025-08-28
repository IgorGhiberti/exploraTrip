using Domain.Entities;
namespace Domain.Intfaces;
public interface IUserRepository
{
    Task AddUser(User user);
    Task<List<User>> GetAll();
    Task<User?> GetUserById(Guid id);
    Task UpdateUser(User user);
    Task<User?> GetUserByEmail(string email);
}