using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace LittleLemon_API.Helpers;

public interface IJwtHelper
{
    /// <summary>
    /// Generates a JWT token for a given username and role.
    /// </summary>
    /// <param name="username">The username for which the token is generated.</param>
    /// <param name="userRole">The role of the user.</param>
    /// <returns>A JWT token string.</returns>
    string GenerateJwtToken(string username, string userRole);

    /// <summary>
    /// Generates a secure, random refresh token.
    /// </summary>
    /// <returns>A refresh token string.</returns>
    string GenerateRefreshToken();

    /// <summary>
    /// Validates an expired JWT token and returns the principal if successful.
    /// </summary>
    /// <param name="token">The JWT token to validate.</param>
    /// <returns>The claims principal extracted from the token.</returns>
    /// <exception cref="SecurityTokenException">Thrown if the token is invalid.</exception>
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}