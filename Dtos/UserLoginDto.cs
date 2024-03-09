namespace LittleLemon_API.Dtos;

public class UserLoginDto
{
    /// <summary>
    /// The username of the logged-in user.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// The JWT token generated for the session.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// An optional refresh token for renewing the session when the JWT token expires.
    /// </summary>
    /// <remarks>
    /// Implement refresh token logic according to your application's security requirements.
    /// </remarks>
    public string RefreshToken { get; set; }
}