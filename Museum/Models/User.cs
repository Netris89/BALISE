using Balise.DTO;
using Balise.Enum;

namespace Balise.Models;

/// <summary>
/// Represents a user of the application, persisted to the database.
/// </summary>
public class User
{
    /// <summary>
    /// The primary key of the user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The street name of the user's address.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// The city of the user's address.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// The user's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The user's first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// The user's last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// The user's full name, combining <see cref="FirstName"/> and <see cref="LastName"/>.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// The house or building number of the user's address.
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// The two-letter ISO region code of the user's country.
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// The role assigned to the user.
    /// </summary>
    public UserRole Role { get; set; }

    /// <summary>
    /// Initializes a new, empty instance of <see cref="User"/>.
    /// </summary>
    public User()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="User"/> from the values collected in the form,
    /// assigning the default <see cref="UserRole.Visitor"/> role.
    /// </summary>
    /// <param name="dto">The form data to build the user from.</param>
    public User(FormDto dto)
    {
        Address = dto.Address;
        City = dto.City;
        Email = dto.Email;
        FirstName = dto.FirstName;
        LastName = dto.LastName;
        Number = dto.Number;
        Country = dto.SelectedCountry.TwoLetterISORegionName;
        Role = UserRole.Visitor;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="User"/> as a copy of an existing user.
    /// </summary>
    /// <param name="user">The user to copy.</param>
    public User(User user)
    {
        Address = user.Address;
        City = user.City;
        Email = user.Email;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Number = user.Number;
        Country = user.Country;
        Role = user.Role;
    }
}
