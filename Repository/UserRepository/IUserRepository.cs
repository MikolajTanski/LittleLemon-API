using LittleLemon_API.Dtos;
using LittleLemon_API.Models;

namespace LittleLemon_API.Repository.UserRepository;

public interface IUserRepository
{
    /// <summary>
    /// Rejestruje nowego użytkownika w systemie.
    /// </summary>
    /// <param name="userRegisterDto">Dane rejestracji użytkownika.</param>
    /// <returns>Flaga wskazująca, czy rejestracja zakończyła się sukcesem.</returns>
    Task<bool> RegisterUserAsync(UserRegisterDto userRegisterDto);

    /// <summary>
    /// Loguje użytkownika do systemu.
    /// </summary>
    /// <param name="userName">Nazwa użytkownika.</param>
    /// <param name="password">Hasło użytkownika.</param>
    /// <returns>Dane użytkownika wraz z tokenami po zalogowaniu; null jeśli logowanie nieudane.</returns>
    Task<UserLoginDto> LoginAsync(string userName, string password);

    /// <summary>
    /// Zmienia hasło istniejącego użytkownika.
    /// </summary>
    /// <param name="userId">ID użytkownika, którego hasło ma zostać zmienione.</param>
    /// <param name="changePasswordDto">Dane wymagane do zmiany hasła.</param>
    /// <returns>Flaga wskazująca, czy zmiana hasła zakończyła się sukcesem.</returns>
    Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
}
