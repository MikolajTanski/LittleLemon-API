using LittleLemon_API.Models;

namespace LittleLemon_API.Repositories.UserRepositories;

public interface IUserRepository
{
    Task<User> GetUserByEmailAsync(string email);
    Task<bool> RegisterUserAsync(User user);
    Task<bool> UpdateUserAsync(User user);
}
