using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;

namespace Museum.ViewModels;

public class FormViewModel : ViewModelBase
{
    private string? firstName;
    private string? lastName;
    private string? address;
    private string? number;
    private string? city;
    private string? email;
    private bool isNewletterChecked;
    private bool isSatisfactionChecked;
    private RegionInfo? selectedCountry;

    public string? FirstName
    {
        get => firstName;
        set => this.RaiseAndSetIfChanged(ref firstName, value);
    }

    public string? LastName
    {
        get => lastName;
        set => this.RaiseAndSetIfChanged(ref lastName, value);
    }

    public string? Address
    {
        get => address;
        set => this.RaiseAndSetIfChanged(ref address, value);
    }

    public string? Number
    {
        get => number;
        set => this.RaiseAndSetIfChanged(ref number, value);
    }

    public string? City
    {
        get => city;
        set => this.RaiseAndSetIfChanged(ref city, value);
    }

    public string? Email
    {
        get => email;
        set => this.RaiseAndSetIfChanged(ref email, value);
    }

    public bool IsNewletterChecked
    {
        get => isNewletterChecked;
        set => this.RaiseAndSetIfChanged(ref isNewletterChecked, value);
    }

    public bool IsSatisfactionChecked
    {
        get => isSatisfactionChecked;
        set => this.RaiseAndSetIfChanged(ref isSatisfactionChecked, value);
    }

    public List<RegionInfo> Countries { get; }

    public RegionInfo? SelectedCountry
    {
        get => selectedCountry;
        set => this.RaiseAndSetIfChanged(ref selectedCountry, value);
    }

    public ICommand CancelCommand { get; }

    public FormViewModel()
    {
        Countries = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                               .Select(culture => new RegionInfo(culture.Name))
                               .DistinctBy(region => region.TwoLetterISORegionName)
                               .OrderBy(region => region.DisplayName)
                               .ToList();

        CancelCommand = ReactiveCommand.Create(ClearForm);
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
}
