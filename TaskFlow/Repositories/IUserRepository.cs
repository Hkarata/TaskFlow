using TaskFlow.Models;

namespace TaskFlow.Repositories
{
    public interface IUserRepository
    {
        Task CreateOrUpdateAccount(User user);
        Task DeleteAccount(Guid userId);
        Task<User?> GetAccount(string email);
    }
}
