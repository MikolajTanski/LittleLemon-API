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

        public AccountController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }


        [HttpPost("addRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                return BadRequest("Role does not exist.");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok($"User {email} added to role {roleName} successfully.");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("removeRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveUserFromRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok($"User {email} removed from role {roleName} successfully.");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("changeRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeUserRole(string email, string newRole)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeResult.Succeeded)
            {
                return BadRequest("Failed to remove user roles.");
            }

            if (!await _roleManager.RoleExistsAsync(newRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(newRole));
            }

            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            if (addResult.Succeeded)
            {
                return Ok($"User {email} role changed to {newRole} successfully.");
            }

            return BadRequest(addResult.Errors);
        }
        
        [HttpPost("createRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest("Role name should not be empty.");
            }

            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (roleExist)
            {
                return BadRequest("Role already exists.");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded)
            {
                return Ok($"Role {roleName} created successfully.");
            }

            return BadRequest(result.Errors);
        }
        
        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser(string email, string password, string username)
        {
            // Sprawdzenie, czy nazwa użytkownika już istnieje
            var userNameExists = await _userManager.FindByNameAsync(username);
            if (userNameExists != null)
            {
                return BadRequest("Username already exists.");
            }

            // Sprawdzenie, czy e-mail użytkownika już istnieje
            var userEmailExists = await _userManager.FindByEmailAsync(email);
            if (userEmailExists != null)
            {
                return BadRequest("Email already exists.");
            }

            var user = new IdentityUser { UserName = username, Email = email };
            var result = await _userManager.CreateAsync(user, password);

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
        

        [HttpGet("currentRole")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetCurrentRole(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count > 0)
            {
                return Ok(new { roles = roles });
            }
            else
            {
                return Ok("User has no roles assigned.");
            }
        }
    }