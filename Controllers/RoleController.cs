using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LittleLemon_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpPost("create")]
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

        [HttpPost("addUserToRole")]
        public async Task<IActionResult> AddUserToRole([FromServices] UserManager<IdentityUser> userManager, string email, string roleName)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                return BadRequest("Role does not exist.");
            }

            var result = await userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok($"User {email} added to role {roleName} successfully.");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("removeUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole([FromServices] UserManager<IdentityUser> userManager, string email, string roleName)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok($"User {email} removed from role {roleName} successfully.");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("changeUserRole")]
        public async Task<IActionResult> ChangeUserRole([FromServices] UserManager<IdentityUser> userManager, string email, string newRole)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var currentRoles = await userManager.GetRolesAsync(user);
            var removeResult = await userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeResult.Succeeded)
            {
                return BadRequest("Failed to remove user roles.");
            }

            if (!await _roleManager.RoleExistsAsync(newRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(newRole));
            }

            var addResult = await userManager.AddToRoleAsync(user, newRole);
            if (addResult.Succeeded)
            {
                return Ok($"User {email} role changed to {newRole} successfully.");
            }

            return BadRequest(addResult.Errors);
        }
    }
}
