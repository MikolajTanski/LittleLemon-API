using LittleLemon_API.Dtos;

namespace LittleLemon_API.Services.UserServices;

public interface IUserService
{
    Task<bool> RegisterUserAsync(UserRegisterDto userRegisterDto);
    Task<UserLoginDto> LoginAsync(string email, string password);
    Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
}
