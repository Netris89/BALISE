using System.Globalization;

namespace Balise.DTO;

/// <summary>
/// Data transfer object carrying the values collected from the newsletter form.
/// </summary>
public class FormDto
{
    /// <summary>
    /// The first name entered by the user.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// The last name entered by the user.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// The street name of the user's address.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// The house or building number of the user's address.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// The city of the user's address.
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// The email address entered by the user.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The country selected by the user, used for region-specific formatting.
    /// </summary>
    public RegionInfo SelectedCountry { get; set; }

    public bool IsNewsletterSubscribed { get; set; }

    public bool WantsSatisfactionSurvey { get; set; }


    /// <summary>
    /// Initializes a new instance of <see cref="FormDto"/> with the values collected from the form.
    /// </summary>
    /// <param name="firstName">The first name entered by the user.</param>
    /// <param name="lastName">The last name entered by the user.</param>
    /// <param name="address">The street name of the user's address.</param>
    /// <param name="number">The house or building number of the user's address.</param>
    /// <param name="city">The city of the user's address.</param>
    /// <param name="email">The email address entered by the user.</param>
    /// <param name="selectedCountry">The country selected by the user.</param>
    public FormDto(string firstName, string lastName, string address, string number, string city, string email, RegionInfo selectedCountry, bool isNewLetterSubscribed, bool wantsSatisfactionSurvey)
    {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        Number = number;
        City = city;
        Email = email;
        SelectedCountry = selectedCountry;
        IsNewsletterSubscribed = isNewLetterSubscribed;
        WantsSatisfactionSurvey = wantsSatisfactionSurvey;
    }
}
