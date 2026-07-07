using Balise.DTO;
using Balise.Enum;

namespace Balise.Models;

public class User
{
    public int Id { get; set; }
    public string Auth0Id { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }

    public User()
    {
    }

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

    public User(User user)
    {
        Address = user.Address;
        City = user.City;
        Email = user.Email;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Number = user.Number;
        Country = user.Country;
        Password = user.Password;
        Role = user.Role;
    }
}
