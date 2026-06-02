namespace Balise.Validation;

public class FormValidationResult
{
    public bool IsValidFirstName { get; set; }
    public bool IsValidLastName { get; set; }
    public bool IsValidAddress { get; set; }
    public bool IsValidNumber { get; set; }
    public bool IsValidCity { get; set; }
    public bool IsValidMail { get; set; }
    public bool IsValid =>
                IsValidFirstName &&
                IsValidLastName &&
                IsValidAddress &&
                IsValidNumber &&
                IsValidCity &&
                IsValidMail;

    public FormValidationResult()
    {
    }
}
