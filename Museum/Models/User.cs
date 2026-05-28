using Museum.DTO;

namespace Museum.Models;

public class User
{
    public int Id { get; set; }
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

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
    }
}
