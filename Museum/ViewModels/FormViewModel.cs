using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Museum.DTO;
using Museum.Services;
using Museum.Validation;
using ReactiveUI;

namespace Museum.ViewModels;

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

    public string FirstName
    {
        get => firstName;
        set
        {
            this.RaiseAndSetIfChanged(ref firstName, value);
            this.RaisePropertyChanged(nameof(IsFormCompleted));
        }
    }

    public string LastName
    {
        get => lastName;
        set
        {
            this.RaiseAndSetIfChanged(ref lastName, value);
            this.RaisePropertyChanged(nameof(IsFormCompleted));
        }
    }

    public string Address
    {
        get => address;
        set
        {
            this.RaiseAndSetIfChanged(ref address, value);
            this.RaisePropertyChanged(nameof(IsFormCompleted));
        }
    }

    public string Number
    {
        get => number;
        set
        {
            this.RaiseAndSetIfChanged(ref number, value);
            this.RaisePropertyChanged(nameof(IsFormCompleted));
        }
    }

    public string City
    {
        get => city;
        set
        {
            this.RaiseAndSetIfChanged(ref city, value);
            this.RaisePropertyChanged(nameof(IsFormCompleted));
        }
    }

    public string Email
    {
        get => email;
        set
        {
            this.RaiseAndSetIfChanged(ref email, value);
            this.RaisePropertyChanged(nameof(IsFormCompleted));
        }
    }

    public bool LastNameError
    {
        get => lastNameError;
        set
        {
            this.RaiseAndSetIfChanged(ref lastNameError, value);
        }
    }

    public bool FirstNameError
    {
        get => firstNameError;
        set
        {
            this.RaiseAndSetIfChanged(ref firstNameError, value);
        }
    }

    public bool AddressError
    {
        get => addressError;
        set
        {
            this.RaiseAndSetIfChanged(ref addressError, value);
        }
    }

    public bool NumberError
    {
        get => numberError;
        set
        {
            this.RaiseAndSetIfChanged(ref numberError, value);
        }
    }

    public bool CityError
    {
        get => cityError;
        set
        {
            this.RaiseAndSetIfChanged(ref cityError, value);
        }
    }

    public bool EmailError
    {
        get => emailError;
        set
        {
            this.RaiseAndSetIfChanged(ref emailError, value);
        }
    }

    public bool IsNewletterChecked
    {
        get => isNewletterChecked;
        set
        {
            this.RaiseAndSetIfChanged(ref isNewletterChecked, value);
            this.RaisePropertyChanged(nameof(IsFormCompleted));
        }
    }

    public bool IsSatisfactionChecked
    {
        get => isSatisfactionChecked;
        set
        {
            this.RaiseAndSetIfChanged(ref isSatisfactionChecked, value);
            this.RaisePropertyChanged(nameof(IsFormCompleted));
        }
    }

    public bool IsFormCompleted => !string.IsNullOrWhiteSpace(FirstName)
                                && !string.IsNullOrWhiteSpace(LastName)
                                && !string.IsNullOrWhiteSpace(Address)
                                && !string.IsNullOrWhiteSpace(Number)
                                && !string.IsNullOrWhiteSpace(City)
                                && !string.IsNullOrWhiteSpace(Email)
                                && SelectedCountry != null;

    public List<RegionInfo> Countries { get; }

    public RegionInfo? SelectedCountry
    {
        get => selectedCountry;
        set
        {
            this.RaiseAndSetIfChanged(ref selectedCountry, value);
            this.RaisePropertyChanged(nameof(IsFormCompleted));
        }
    }

    public ICommand CancelCommand { get; }
    public ICommand AcceptCommand { get; }

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

    public void ValidateForm()
    {
        FormDto dto = new FormDto(FirstName, LastName, Address, Number, City, Email, SelectedCountry!);

        FormValidationResult results = service.ValidateUser(dto);

        LastNameError = results.IsValidLastName;
        FirstNameError = results.IsValidFirstName;
        AddressError = results.IsValidAddress;
        NumberError = results.IsValidNumber;
        CityError = results.IsValidCity;
        EmailError = results.IsValidMail;
    }
}
