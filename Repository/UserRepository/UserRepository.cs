using LittleLemon_API.Data;
using LittleLemon_API.Dtos;
using LittleLemon_API.Helpers;
using LittleLemon_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LittleLemon_API.Repository.UserRepository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtHelper _jwtHelper;

    public UserRepository(ApplicationDbContext context, IJwtHelper jwtHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
    }

    public async Task<bool> RegisterUserAsync(UserRegisterDto userRegisterDto)
    {
        var existingUserByEmail = await _context.Users
            .AnyAsync(u => u.Email == userRegisterDto.Email);
        var existingUserByUsername = await _context.Users
            .AnyAsync(u => u.UserName == userRegisterDto.UserName);

        if (existingUserByEmail || existingUserByUsername)
        {
            return false; // User with this email or username already exists.
        }

        var hashedPassword = HashPassword(userRegisterDto.Password); // Assume HashPassword implementation.
        var user = new User
        {
            Email = userRegisterDto.Email,
            UserName = userRegisterDto.UserName,
            PasswordHash = hashedPassword
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<UserLoginDto> LoginAsync(string userName, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName);

        if (user == null || !VerifyPassword(password, user.PasswordHash)) // Assume VerifyPassword implementation.
        {
            return null; // User not found or incorrect password.
        }

        // Generating JWT token and possibly refresh token
        var token = _jwtHelper.GenerateJwtToken(user.UserName, user.Role);
         var refreshToken = _jwtHelper.GenerateRefreshToken(); 

        var userLoginDto = new UserLoginDto
        {
            UserName = user.UserName,
            Token = token, 
            RefreshToken = refreshToken,
        };

        return userLoginDto;
    }

    public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
    {
        var user = await _context.Users.FindAsync(userId);
        
        if (user == null || !VerifyPassword(changePasswordDto.OldPassword, user.PasswordHash))
        {
            return false;
        }

        user.PasswordHash = HashPassword(changePasswordDto.NewPassword);
        await _context.SaveChangesAsync();
        return true;
    }

    private string HashPassword(string password)
    {
        return password;
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        return password == hashedPassword;
    }
}
