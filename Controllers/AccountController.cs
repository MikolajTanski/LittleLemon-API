using LittleLemon_API.Models.Dto;
using LittleLemon_API.Services.EmailServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LittleLemon_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IEmailService _emailService;

    public AccountController(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<IdentityUser> signInManager,
        IEmailService emailService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _emailService = emailService;
    }

    [HttpPost("registerUser")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand registerUserCommand)
    {
        var userNameExists = await _userManager.FindByNameAsync(registerUserCommand.username);
        if (userNameExists != null)
        {
            return BadRequest("Username already exists.");
        }

        var userEmailExists = await _userManager.FindByEmailAsync(registerUserCommand.email);
        if (userEmailExists != null)
        {
            return BadRequest("Email already exists.");
        }

        var user = new IdentityUser { UserName = registerUserCommand.username, Email = registerUserCommand.email };
        var result = await _userManager.CreateAsync(user, registerUserCommand.password);

        if (result.Succeeded)
        {
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            await _userManager.AddToRoleAsync(user, "User");

            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok(new { message = "User registered successfully" });
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand loginCommand)
    {
        var user = await _userManager.FindByEmailAsync(loginCommand.email);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        var result = await _signInManager.PasswordSignInAsync(user.UserName, loginCommand.password, isPersistent: false, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            return Ok("User logged in successfully.");
        }

        return BadRequest("Invalid login attempt.");
    }

    [HttpPost("resetPassword")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> ResetPassword(string email, string code, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return BadRequest("Invalid request");
        }

        var result = await _userManager.ResetPasswordAsync(user, code, newPassword);
        if (result.Succeeded)
        {
            return Ok("Password has been reset successfully.");
        }

        return BadRequest("Failed to reset password.");
    }

    [HttpPost("forgotPassword")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        {
            return Ok("If your email is valid, we have sent a password reset link.");
        }

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

        await _emailService.SendEmailAsync(email, "Reset Password", $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

        return Ok("If your email is valid, we have sent a password reset link.");
    }
}