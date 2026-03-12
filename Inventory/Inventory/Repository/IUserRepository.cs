using Inventory.Models.Database;

namespace Inventory.Repositories
{
    public interface IUserRepository
    {
        User GetByUsername(string username);
        bool UsernameExists(string username);
        void AddUser(User user);
        User ValidateUser(string username, string passwordHash);
    }
}