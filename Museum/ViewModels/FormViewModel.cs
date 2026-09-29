using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Balise.DTO;
using Balise.Services;
using Balise.Validation;
using ReactiveUI;

namespace Balise.ViewModels;

/// <summary>
/// Backs the registration form view, holding field values, their validation
/// error flags, and the commands to clear or submit the form.
/// </summary>
public class FormViewModel : ViewModelBase
{
    private string firstName = string.Empty;
    private string lastName = string.Empty;
    private string address = string.Empty;
    private string number = string.Empty;
    private string city = string.Empty;
    private string email = string.Empty;
    private bool lastNameError = true;
    private bool firstNameError = true;
    private bool addressError = true;
    private bool numberError = true;
    private bool cityError = true;
    private bool emailError = true;
    private bool isNewletterChecked;
    private bool isSatisfactionChecked;
    private RegionInfo? selectedCountry;
    private FormService service;

    /// <summary>
    /// The first name entered in the form.
    /// </summary>
    public string FirstName
    {
        get => firstName;
        set
        {
            this.RaiseAndSetIfChanged(ref firstName, value);
            this.RaisePropertyChanged(nameof(IsFormCompleted));
        }
    }

    /// <summary>
    /// The last name entered in the form.
    /// </summary>
    public string LastName
    {
        get => lastName;
        set
        {
            this.RaiseAndSetIfChanged(ref lastName, value);
            this.RaisePropertyChanged(nameof(IsFormCompleted));
        }
    }

    /// <summary>
    /// The street address entered in the form.
    /// </summary>
    public string Address
    {
        get => address;
        set
        {
            this.RaiseAndSetIfChanged(ref address, value);
            this.RaisePropertyChanged(nameof(IsFormCompleted));
        }
    }

    /// <summary>
    /// The house or building number entered in the form.
    /// </summary>
    public string Number
    {
        get => number;
        set
        {
            this.RaiseAndSetIfChanged(ref number, value);
            this.RaisePropertyChanged(nameof(IsFormCompleted));
        }
    }

    /// <summary>
    /// The city entered in the form.
    /// </summary>
    public string City
    {
        get => city;
        set
        {
            this.RaiseAndSetIfChanged(ref city, value);
            this.RaisePropertyChanged(nameof(IsFormCompleted));
        }
    }

    /// <summary>
    /// The email address entered in the form.
    /// </summary>
    public string Email
    {
        get => email;
        set
        {
            this.RaiseAndSetIfChanged(ref email, value);
            this.RaisePropertyChanged(nameof(IsFormCompleted));
        }
    }

    /// <summary>
    /// Indicates whether the last name field failed validation.
    /// </summary>
    public bool LastNameError
    {
        get => lastNameError;
        set
        {
            this.RaiseAndSetIfChanged(ref lastNameError, value);
        }
    }

    /// <summary>
    /// Indicates whether the first name field failed validation.
    /// </summary>
    public bool FirstNameError
    {
        get => firstNameError;
        set
        {
            this.RaiseAndSetIfChanged(ref firstNameError, value);
        }
    }

    /// <summary>
    /// Indicates whether the address field failed validation.
    /// </summary>
    public bool AddressError
    {
        get => addressError;
        set
        {
            this.RaiseAndSetIfChanged(ref addressError, value);
        }
    }

    /// <summary>
    /// Indicates whether the number field failed validation.
    /// </summary>
    public bool NumberError
    {
        get => numberError;
        set
        {
            this.RaiseAndSetIfChanged(ref numberError, value);
        }
    }

    /// <summary>
    /// Indicates whether the city field failed validation.
    /// </summary>
    public bool CityError
    {
        get => cityError;
        set
        {
            this.RaiseAndSetIfChanged(ref cityError, value);
        }
    }

    /// <summary>
    /// Indicates whether the email field failed validation.
    /// </summary>
    public bool EmailError
    {
        get => emailError;
        set
        {
            this.RaiseAndSetIfChanged(ref emailError, value);
        }
    }

    /// <summary>
    /// Indicates whether the user opted in to the newsletter.
    /// </summary>
    public bool IsNewletterChecked
    {
        get => isNewletterChecked;
        set
        {
            this.RaiseAndSetIfChanged(ref isNewletterChecked, value);
            this.RaisePropertyChanged(nameof(IsFormCompleted));
        }
    }

    /// <summary>
    /// Indicates whether the user opted in to the satisfaction survey.
    /// </summary>
    public bool IsSatisfactionChecked
    {
        get => isSatisfactionChecked;
        set
        {
            this.RaiseAndSetIfChanged(ref isSatisfactionChecked, value);
            this.RaisePropertyChanged(nameof(IsFormCompleted));
        }
    }

    /// <summary>
    /// Indicates whether all required fields have been filled in.
    /// </summary>
    public bool IsFormCompleted => /* !string.IsNullOrWhiteSpace(FirstName)
                                && !string.IsNullOrWhiteSpace(LastName)
                                && !string.IsNullOrWhiteSpace(Address)
                                && !string.IsNullOrWhiteSpace(Number)
                                && !string.IsNullOrWhiteSpace(City)
                                && SelectedCountry != null
                                &&  */!string.IsNullOrWhiteSpace(Email)
                                && (IsSatisfactionChecked || IsNewletterChecked);

    /// <summary>
    /// The list of countries available for selection, one per ISO region.
    /// </summary>
    public List<RegionInfo> Countries { get; }

    /// <summary>
    /// The country currently selected in the form.
    /// </summary>
    public RegionInfo? SelectedCountry
    {
        get => selectedCountry;
        set
        {
            this.RaiseAndSetIfChanged(ref selectedCountry, value);
            this.RaisePropertyChanged(nameof(IsFormCompleted));
        }
    }

    /// <summary>
    /// Command that clears all fields in the form.
    /// </summary>
    public ICommand CancelCommand { get; }

    /// <summary>
    /// Command that validates and submits the form.
    /// </summary>
    public ICommand AcceptCommand { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="FormViewModel"/>.
    /// </summary>
    /// <param name="formService">The service used to validate and submit the form.</param>
    public FormViewModel(FormService formService)
    {
        Countries = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                               .Select(culture => new RegionInfo(culture.Name))
                               .DistinctBy(region => region.TwoLetterISORegionName)
                               .OrderBy(region => region.DisplayName)
                               .ToList();

        CancelCommand = ReactiveCommand.Create(ClearForm);
        AcceptCommand = ReactiveCommand.Create(ValidateForm);
        service = formService;
    }

    /// <summary>
    /// Resets all fields and checkboxes to their default, empty state.
    /// </summary>
    public void ClearForm()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Address = string.Empty;
        Number = string.Empty;
        City = string.Empty;
        Email = string.Empty;
        SelectedCountry = null;
        IsNewletterChecked = false;
        IsSatisfactionChecked = false;
    }

    /// <summary>
    /// Builds a <see cref="FormDto"/> from the current field values, validates it
    /// through the form service, and updates the error flags accordingly.
    /// </summary>
    public async Task ValidateForm()
    {
        FormDto dto = new FormDto(FirstName, LastName, Address, Number, City, Email, SelectedCountry!, IsNewletterChecked, IsSatisfactionChecked);

        FormValidationResult results = await service.ValidateUser(dto);

        LastNameError = results.IsValidLastName;
        FirstNameError = results.IsValidFirstName;
        AddressError = results.IsValidAddress;
        NumberError = results.IsValidNumber;
        CityError = results.IsValidCity;
        EmailError = results.IsValidMail;

        ClearForm();
    }
}
