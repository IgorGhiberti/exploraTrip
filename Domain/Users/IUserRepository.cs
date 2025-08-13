namespace Domain.User
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        Task<User> GetUserById(Guid id);
        void AddUser(User user);
        void UpdateUser(User user);
        void DisableUser(Guid id);
        Task<bool> IsUserActive(Guid id);
        void ActiveUser(Guid id);
    }
}