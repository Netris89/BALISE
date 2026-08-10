namespace Balise.Validation;

/// <summary>
/// Represents the validation result of all fields in a form.
/// </summary>
public class FormValidationResult
{
    /// <summary>
    /// Indicates whether the first name is valid.
    /// </summary>
    public bool IsValidFirstName { get; set; }
    
    /// <summary>
    /// Indicates whether the last name is valid.
    /// </summary>
    public bool IsValidLastName { get; set; }

    /// <summary>
    /// Indicates whether the address is valid.
    /// </summary>
    public bool IsValidAddress { get; set; }

    /// <summary>
    /// Indicates whether the number is valid.
    /// </summary>
    public bool IsValidNumber { get; set; }

    /// <summary>
    /// Indicates whether the city is valid.
    /// </summary>
    public bool IsValidCity { get; set; }

    /// <summary>
    /// Indicates whether the email address is valid.
    /// </summary>
    public bool IsValidMail { get; set; }

    /// <summary>
    /// Indicates whether all form fields are valid.
    /// </summary>
    public bool IsValid =>
                IsValidFirstName &&
                IsValidLastName &&
                IsValidAddress &&
                IsValidNumber &&
                IsValidCity &&
                IsValidMail;

    /// <summary>
    /// Initializes a new instance of the <see cref="FormValidationResult"/> class.
    /// </summary>
    public FormValidationResult()
    {
    }
}
