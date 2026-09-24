using System.Threading.Tasks;
using System.Windows.Input;
using Balise.Models;
using Balise.Repositories;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using ReactiveUI;

namespace Balise.ViewModels;

/// <summary>
/// ViewModel responsible for editing point of interest information.
/// </summary>
public class PointOfInterestEditorViewModel : ViewModelBase
{
    private readonly PointOfInterestRepository repository;
    private readonly PointOfInterest selectedPointOfInterest;
    private string name = string.Empty;
    private string nameDe = string.Empty;
    private string nameEn = string.Empty;
    private string nameNl = string.Empty;
    private string description = string.Empty;
    private string descriptionDe = string.Empty;
    private string descriptionEn = string.Empty;
    private string descriptionNl = string.Empty;
    private Point position = Point.Empty;

    /// <summary>
    /// Command used to save the current point of interest changes.
    /// </summary>
    public ICommand SaveCommand { get; }

    /// <summary>
    /// Name of the point of interest being edited.
    /// </summary>
    public string Name
    {
        get => name;
        set
        {
            this.RaiseAndSetIfChanged(ref name, value);
        }
    }
    public string NameDe
    {
        get => nameDe;
        set
        {
            this.RaiseAndSetIfChanged(ref nameDe, value);
        }
    }
    public string NameEn
    {
        get => nameEn;
        set
        {
            this.RaiseAndSetIfChanged(ref nameEn, value);
        }
    }
    public string NameNl
    {
        get => nameNl;
        set
        {
            this.RaiseAndSetIfChanged(ref nameNl, value);
        }
    }

    /// <summary>
    /// Description of the point of interest being edited.
    /// </summary>
    public string Description
    {
        get => description;
        set
        {
            this.RaiseAndSetIfChanged(ref description, value);
        }
    }

    public string DescriptionDe
    {
        get => descriptionDe;
        set
        {
            this.RaiseAndSetIfChanged(ref descriptionDe, value);
        }
    }

    public string DescriptionEn
    {
        get => descriptionEn;
        set
        {
            this.RaiseAndSetIfChanged(ref descriptionEn, value);
        }
    }

    public string DescriptionNl
    {
        get => descriptionNl;
        set
        {
            this.RaiseAndSetIfChanged(ref descriptionNl, value);
        }
    }

    public Point Position
    {
        get => position;
        set
        {
            this.RaiseAndSetIfChanged(ref position, value);
        }
    }

    public double PositionX
    {
        get => position?.X ?? 0;
        set
        {
            var factory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 999001);
            Position = factory.CreatePoint(new Coordinate(value, PositionY));
            this.RaisePropertyChanged(nameof(PositionX));
        }
    }

    public double PositionY
    {
        get => position?.Y ?? 0;
        set
        {
            var factory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 999001);
            Position = factory.CreatePoint(new Coordinate(PositionX, value));
            this.RaisePropertyChanged(nameof(PositionY));
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PointOfInterestEditorViewModel"/> class.
    /// </summary>
    /// <param name="pointOfInterest">Point of interest to edit.</param>
    /// <param name="pointOfInterestRepository">Repository used to persist point of interest changes.</param>
    public PointOfInterestEditorViewModel(PointOfInterest pointOfInterest, PointOfInterestRepository pointOfInterestRepository)
    {
        selectedPointOfInterest = pointOfInterest;

        Name = selectedPointOfInterest.Name;
        NameDe = selectedPointOfInterest.NameDe;
        NameEn = selectedPointOfInterest.NameEn;
        NameNl = selectedPointOfInterest.NameNl;
        Description = selectedPointOfInterest.Description;
        DescriptionDe = selectedPointOfInterest.DescriptionDe;
        DescriptionEn = selectedPointOfInterest.DescriptionEn;
        DescriptionNl = selectedPointOfInterest.DescriptionNl;
        Position = selectedPointOfInterest.Position;

        repository = pointOfInterestRepository;

        SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync);
    }

    /// <summary>
    /// Saves the current point of interest information to the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation.</returns>
    public async Task SaveAsync()
    {
        selectedPointOfInterest.Name = Name;
        selectedPointOfInterest.NameDe = NameDe;
        selectedPointOfInterest.NameEn = NameEn;
        selectedPointOfInterest.NameNl = NameNl;
        selectedPointOfInterest.Description = Description;
        selectedPointOfInterest.DescriptionDe = DescriptionDe;
        selectedPointOfInterest.DescriptionEn = DescriptionEn;
        selectedPointOfInterest.DescriptionNl = DescriptionNl;
        selectedPointOfInterest.Position = Position;

        await repository.UpdateAsync(selectedPointOfInterest);
    }
}
