using System.Linq;
using System.Net.Mail;
using Balise.DTO;

namespace Balise.Validation;

public class FormValidator
{
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
