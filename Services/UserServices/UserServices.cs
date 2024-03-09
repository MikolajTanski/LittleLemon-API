using LittleLemon_API.Dtos;
using LittleLemon_API.Repository.UserRepository;
using System.Threading.Tasks;

namespace LittleLemon_API.Services.UserServices;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> RegisterUserAsync(UserRegisterDto userRegisterDto)
    {
        var registrationResult = await _userRepository.RegisterUserAsync(userRegisterDto);
        return registrationResult;
    }

    public async Task<UserLoginDto> LoginAsync(string email, string password)
    {

        var userLoginDto = await _userRepository.LoginAsync(email, password);
        
        return userLoginDto;
    }

    public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
    {

        var passwordChangeResult = await _userRepository.ChangePasswordAsync(userId, changePasswordDto);
        return passwordChangeResult;
    }
}