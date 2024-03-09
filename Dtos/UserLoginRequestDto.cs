namespace LittleLemon_API.Dtos;

/// <summary>
/// Data transfer object for receiving user login requests.
/// </summary>
public class UserLoginRequestDto
{
    /// <summary>
    /// The email or username of the user attempting to log in.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// The password of the user attempting to log in.
    /// </summary>
    public string Password { get; set; }
}