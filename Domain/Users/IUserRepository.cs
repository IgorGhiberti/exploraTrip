namespace Domain.User
{
    public interface IUserRepository
    {
        Task ActiveUser(User user);
        Task AddUser(User user);
        Task DisableUser(User user);
        Task<List<User>> GetAll();
        Task<User> GetUserById(Guid id);
        Task UpdateUser(User user);
    }
}