using System.Threading.Tasks;
using System.Windows.Input;
using Balise.Models;
using Balise.Repositories;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using ReactiveUI;

namespace Balise.ViewModels;

/// <summary>
/// ViewModel responsible for editing space information.
/// </summary>
public class SpaceEditorViewModel : ViewModelBase
{
    private string name = string.Empty;
    private string nameDe = string.Empty;
    private string nameEn = string.Empty;
    private string nameNl = string.Empty;
    private string description = string.Empty;
    private string descriptionDe = string.Empty;
    private string descriptionEn = string.Empty;
    private string descriptionNl = string.Empty;
    private Polygon boundary = Polygon.Empty;
    private readonly Space selectedSpace;
    private readonly SpaceRepository repository;

    /// <summary>
    /// Command used to save the current space changes.
    /// </summary>
    public ICommand SaveCommand { get; }

    /// <summary>
    /// Name of the space being edited.
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
    /// Description of the space being edited.
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

    public Polygon Boundary
    {
        get => boundary;
        set
        {
            this.RaiseAndSetIfChanged(ref boundary, value);
            this.RaisePropertyChanged(nameof(Point1X));
            this.RaisePropertyChanged(nameof(Point1Y));
            this.RaisePropertyChanged(nameof(Point2X));
            this.RaisePropertyChanged(nameof(Point2Y));
            this.RaisePropertyChanged(nameof(Point3X));
            this.RaisePropertyChanged(nameof(Point3Y));
            this.RaisePropertyChanged(nameof(Point4X));
            this.RaisePropertyChanged(nameof(Point4Y));
        }
    }

    public double Point1X
    {
        get => boundary?.ExteriorRing?.Coordinates.Length > 0 ? boundary.ExteriorRing.Coordinates[0].X : 0;
        set
        {
            var factory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 999001);
            Boundary = factory.CreatePolygon(new[]
            {
                new Coordinate(value, Point1Y),
                new Coordinate(Point2X, Point2Y),
                new Coordinate(Point3X, Point3Y),
                new Coordinate(Point4X, Point4Y),
                new Coordinate(value, Point1Y)
            });
            this.RaisePropertyChanged(nameof(Point1X));
        }
    }

    public double Point1Y
    {
        get => boundary?.ExteriorRing?.Coordinates.Length > 0 ? boundary.ExteriorRing.Coordinates[0].Y : 0;
        set
        {
            var factory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 999001);
            Boundary = factory.CreatePolygon(new[]
            {
                new Coordinate(Point1X, value),
                new Coordinate(Point2X, Point2Y),
                new Coordinate(Point3X, Point3Y),
                new Coordinate(Point4X, Point4Y),
                new Coordinate(Point1X, value)
            });
            this.RaisePropertyChanged(nameof(Point1Y));
        }
    }

    public double Point2X
    {
        get => boundary?.ExteriorRing?.Coordinates.Length > 1 ? boundary.ExteriorRing.Coordinates[1].X : 0;
        set
        {
            var factory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 999001);
            Boundary = factory.CreatePolygon(new[]
            {
                new Coordinate(Point1X, Point1Y),
                new Coordinate(value, Point2Y),
                new Coordinate(Point3X, Point3Y),
                new Coordinate(Point4X, Point4Y),
                new Coordinate(Point1X, Point1Y)
            });
            this.RaisePropertyChanged(nameof(Point2X));
        }
    }

    public double Point2Y
    {
        get => boundary?.ExteriorRing?.Coordinates.Length > 1 ? boundary.ExteriorRing.Coordinates[1].Y : 0;
        set
        {
            var factory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 999001);
            Boundary = factory.CreatePolygon(new[]
            {
                new Coordinate(Point1X, Point1Y),
                new Coordinate(Point2X, value),
                new Coordinate(Point3X, Point3Y),
                new Coordinate(Point4X, Point4Y),
                new Coordinate(Point1X, Point1Y)
            });
            this.RaisePropertyChanged(nameof(Point2Y));
        }
    }

    public double Point3X
    {
        get => boundary?.ExteriorRing?.Coordinates.Length > 2 ? boundary.ExteriorRing.Coordinates[2].X : 0;
        set
        {
            var factory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 999001);
            Boundary = factory.CreatePolygon(new[]
            {
                new Coordinate(Point1X, Point1Y),
                new Coordinate(Point2X, Point2Y),
                new Coordinate(value, Point3Y),
                new Coordinate(Point4X, Point4Y),
                new Coordinate(Point1X, Point1Y)
            });
            this.RaisePropertyChanged(nameof(Point3X));
        }
    }

    public double Point3Y
    {
        get => boundary?.ExteriorRing?.Coordinates.Length > 2 ? boundary.ExteriorRing.Coordinates[2].Y : 0;
        set
        {
            var factory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 999001);
            Boundary = factory.CreatePolygon(new[]
            {
                new Coordinate(Point1X, Point1Y),
                new Coordinate(Point2X, Point2Y),
                new Coordinate(Point3X, value),
                new Coordinate(Point4X, Point4Y),
                new Coordinate(Point1X, Point1Y)
            });
            this.RaisePropertyChanged(nameof(Point3Y));
        }
    }

    public double Point4X
    {
        get => boundary?.ExteriorRing?.Coordinates.Length > 3 ? boundary.ExteriorRing.Coordinates[3].X : 0;
        set
        {
            var factory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 999001);
            Boundary = factory.CreatePolygon(new[]
            {
                new Coordinate(Point1X, Point1Y),
                new Coordinate(Point2X, Point2Y),
                new Coordinate(Point3X, Point3Y),
                new Coordinate(value, Point4Y),
                new Coordinate(Point1X, Point1Y)
            });
            this.RaisePropertyChanged(nameof(Point4X));
        }
    }

    public double Point4Y
    {
        get => boundary?.ExteriorRing?.Coordinates.Length > 3 ? boundary.ExteriorRing.Coordinates[3].Y : 0;
        set
        {
            var factory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 999001);
            Boundary = factory.CreatePolygon(new[]
            {
                new Coordinate(Point1X, Point1Y),
                new Coordinate(Point2X, Point2Y),
                new Coordinate(Point3X, Point3Y),
                new Coordinate(Point4X, value),
                new Coordinate(Point1X, Point1Y)
            });
            this.RaisePropertyChanged(nameof(Point4Y));
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpaceEditorViewModel"/> class.
    /// </summary>
    /// <param name="space">Space to edit.</param>
    /// <param name="spaceRepository">Repository used to persist space changes.</param>
    public SpaceEditorViewModel(Space space, SpaceRepository spaceRepository)
    {
        selectedSpace = space;

        Name = selectedSpace.Name;
        NameDe = selectedSpace.NameDe;
        NameEn = selectedSpace.NameEn;
        NameNl = selectedSpace.NameNl;
        Description = selectedSpace.Description;
        DescriptionDe = selectedSpace.DescriptionDe;
        DescriptionEn = selectedSpace.DescriptionEn;
        DescriptionNl = selectedSpace.DescriptionNl;
        Boundary = selectedSpace.Boundary;
        
        repository = spaceRepository;

        SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync);
    }

    /// <summary>
    /// Saves the current space information to the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation.</returns>
    public async Task SaveAsync()
    {
        selectedSpace.Name = Name;
        selectedSpace.NameDe = NameDe;
        selectedSpace.NameEn = NameEn;
        selectedSpace.NameNl = NameNl;
        selectedSpace.Description = Description;
        selectedSpace.DescriptionDe = DescriptionDe;
        selectedSpace.DescriptionEn = DescriptionEn;
        selectedSpace.DescriptionNl = DescriptionNl;
        selectedSpace.Boundary = Boundary;

        await repository.UpdateAsync(selectedSpace);
    }
}
