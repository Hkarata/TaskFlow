using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Models;

namespace TaskFlow.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        public async Task CreateOrUpdateAccount(User receivedUser)
        {
            var user = await GetAccount(receivedUser.Id);

            if (user == null)
            {
                context.Users.Add(receivedUser);
            }
            else
            {
                context.Users.Update(user);
            }

            await context.SaveChangesAsync();
        }

        public async Task DeleteAccount(Guid userId)
        {
            var user = await GetAccount(userId);

            if (user == null)
                return;

            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }

        public async Task<User?> GetAccount(string email)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        private async Task<User?> GetAccount(Guid userId)
        {
            return await context.Users.FindAsync(userId);
        }

    }
}
