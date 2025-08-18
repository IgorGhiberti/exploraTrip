namespace Domain.User
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        Task<User> GetUserById(Guid id);
        Task AddUser(User user);
        Task UpdateUser(User user);
        Task DisableUser(Guid id);
        Task<bool> IsUserActive(Guid id);
        Task ActiveUser(Guid id);
    }
}