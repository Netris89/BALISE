using System.Linq;
using System.Net.Mail;
using Balise.DTO;

namespace Balise.Validation;

/// <summary>
/// Provides validation methods for form input data.
/// </summary>
public class FormValidator
{
    /// <summary>
    /// Validates all fields of the specified form and returns the validation result.
    /// </summary>
    /// <param name="dto">Data transfer object containing the form values to validate.</param>
    /// <returns>A result containing the validation status of each form field.</returns>
    internal static FormValidationResult ValidateForm(FormDto dto)
    {
        var result = new FormValidationResult
        {
            IsValidFirstName = IsValidName(dto.FirstName),
            IsValidLastName = IsValidName(dto.LastName),
            IsValidAddress = IsValidAddress(dto.Address),
            IsValidNumber = IsValidNumber(dto.Number),
            IsValidCity = IsValidCity(dto.City),
            IsValidMail = IsValidEmail(dto.Email),
        };

        return result;
    }

    /// <summary>
    /// Validates a name by checking that it is not empty and has an acceptable length.
    /// </summary>
    /// <param name="name">Name to validate.</param>
    /// <returns><see langword="true"/> if the name is valid; otherwise, <see langword="false"/>.</returns>
    private static bool IsValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        if (name.Length < 2 || name.Length > 50)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Validates an email address using the <see cref="MailAddress"/> class.
    /// </summary>
    /// <param name="email">Email address to validate.</param>
    /// <returns><see langword="true"/> if the email address is valid; otherwise, <see langword="false"/>.</returns>
    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;

        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Validates an address by checking that it is not empty and has an acceptable length.
    /// </summary>
    /// <param name="address">Address to validate.</param>
    /// <returns><see langword="true"/> if the address is valid; otherwise, <see langword="false"/>.</returns>
    private static bool IsValidAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            return false;
        }

        if (address.Length < 5 || address.Length > 200)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Validates a number by checking that it is not empty, does not exceed the maximum length,
    /// and contains at least one digit.
    /// </summary>
    /// <param name="number">Number to validate.</param>
    /// <returns><see langword="true"/> if the number is valid; otherwise, <see langword="false"/>.</returns>
    private static bool IsValidNumber(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return false;
        }

        if (number.Length > 20)
        {
            return false;
        }

        if (!number.Any(char.IsDigit))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Validates a city name by checking that it is not empty and has an acceptable length.
    /// </summary>
    /// <param name="city">City name to validate.</param>
    /// <returns><see langword="true"/> if the city name is valid; otherwise, <see langword="false"/>.</returns>
    private static bool IsValidCity(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return false;
        }

        if (city.Length < 2 || city.Length > 100)
        {
            return false;
        }

        return true;
    }
}
