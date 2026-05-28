using System;
using System.Globalization;

namespace Museum.DTO;

public class FormDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string Number { get; set; }
    public string City { get; set; }
    public string Email { get; set; }
    public RegionInfo SelectedCountry { get; set; }

    public FormDto(string firstName, string lastName, string address, string number, string city, string email, RegionInfo selectedCountry)
    {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        Number = number;
        City = city;
        Email = email;
        SelectedCountry = selectedCountry;
    }
}
