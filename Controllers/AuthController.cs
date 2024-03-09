using System.Security.Claims;
using LittleLemon_API.Dtos;
using LittleLemon_API.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LittleLemon_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            try
            {
                var result = await _userService.RegisterUserAsync(userRegisterDto);
                if (!result)
                {
                    return BadRequest("User already exists or invalid data provided.");
                }

                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequestDto loginRequest)
        {
            try
            {
                var userLoginDto = await _userService.LoginAsync(loginRequest.Username, loginRequest.Password);

                if (userLoginDto == null)
                {
                    return Unauthorized("Invalid username or password.");
                }

                return Ok(userLoginDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _userService.ChangePasswordAsync(userId, changePasswordDto);

                if (!result)
                {
                    return BadRequest("Password change failed. Ensure old password is correct.");
                }

                return Ok("Password changed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            throw new Exception("User ID not found in token");
        }
    }
}
