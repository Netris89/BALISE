using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Balise.Models;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using ReactiveUI;

namespace Balise.ViewModels;

/// <summary>
/// ViewModel responsible for museum map display, floor navigation,
/// space selection and point of interest interaction.
/// </summary>
public class MapViewModel : ViewModelBase
{
    private Bitmap image;
    private Museum museum;
    private List<Space> spaces;
    private Space? selectedSpace;
    private PointOfInterest? selectedPointOfInterest;

    private static readonly GeometryFactory Factory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 999001);

    /// <summary>
    /// Currently selected space displayed in the side information panel.
    /// </summary>
    public Space? SelectedSpace
    {
        get => selectedSpace;
        set
        {
            selectedSpace = value;
            this.RaisePropertyChanged(nameof(SelectedSpace));
        }
    }

    /// <summary>
    /// Currently selected point of interest displayed in the popup.
    /// </summary>
    public PointOfInterest? SelectedPointOfInterest
    {
        get => selectedPointOfInterest;
        set
        {
            selectedPointOfInterest = value;
            this.RaisePropertyChanged(nameof(SelectedPointOfInterest));
        }
    }

    /// <summary>
    /// Current floor map image displayed in the view.
    /// </summary>
    public Bitmap Image
    {
        get => image;
        set => this.RaiseAndSetIfChanged(ref image, value);
    }

    /// <summary>
    /// List of all floors available in the museum.
    /// </summary>
    public List<Floor> Floors { get; set; }

    /// <summary>
    /// Currently displayed floor.
    /// </summary>
    public Floor CurrentFloor { get; private set; }

    /// <summary>
    /// Spaces (rooms/areas) displayed for the current floor.
    /// </summary>
    public List<Space> Spaces
    {
        get => spaces;
        set => this.RaiseAndSetIfChanged(ref spaces, value);
    }

    /// <summary>
    /// Command used to change the displayed floor.
    /// </summary>
    public ReactiveCommand<Floor, Unit> ChangeFloorCommand { get; }

    /// <summary>
    /// Command used to select a point of interest.
    /// </summary>
    public ReactiveCommand<PointOfInterest, Unit> SelectingPoiCommand { get; }

    /// <summary>
    /// Activator for ReactiveUI lifecycle management.
    /// </summary>
    public ViewModelActivator Activator => new ViewModelActivator();

    /// <summary>
    /// Initializes the ViewModel with a sample museum structure.
    /// (Currently hardcoded for testing purposes.)
    /// </summary>
    public MapViewModel()
    {
        // Points of interest (test data)
        var poi1 = new PointOfInterest(
            "Statue A",
            "Statue en marbre",
            Factory.CreatePoint(new Coordinate(120, 120))
        );

        var poi2 = new PointOfInterest(
            "Statue B",
            "Statue en bronze",
            Factory.CreatePoint(new Coordinate(180, 160))
        );

        var poi3 = new PointOfInterest(
            "Vitrine C",
            "Objets anciens",
            Factory.CreatePoint(new Coordinate(350, 300))
        );

        // Spaces (rooms containing POIs and defined by polygons)
        var space1 = new Space(
            "Salle Romaine",
            "Une salle contenant des artéfacts romains",
            new List<PointOfInterest> { poi1, poi2 },
            Factory.CreatePolygon(new[]
            {
                new Coordinate(400, 300),
                new Coordinate(550, 300),
                new Coordinate(550, 500),
                new Coordinate(400, 500),
                new Coordinate(400, 300)
            })
        );

        var space2 = new Space(
            "Salle Médiévale",
            "Une salle contenant des artéfacts médiévaux",
            new List<PointOfInterest> { poi3 },
            Factory.CreatePolygon(new[]
            {
                new Coordinate(300, 200),
                new Coordinate(450, 200),
                new Coordinate(450, 400),
                new Coordinate(300, 400),
                new Coordinate(300, 200)
            })
        );

        // Floors (each floor has its own map and spaces)
        var floor0 = new Floor(
            0,
            new Bitmap(AssetLoader.Open(new Uri("avares://Balise/Assets/REZ_GC.png"))),
            new List<Space> { space1 }
        );

        var floor1 = new Floor(
            1,
            new Bitmap(AssetLoader.Open(new Uri("avares://Balise/Assets/GC_1.png"))),
            new List<Space> { space2 }
        );

        var floor2 = new Floor(
            2,
            new Bitmap(AssetLoader.Open(new Uri("avares://Balise/Assets/GC_2.png"))),
            new List<Space>()
        );

        // Museum root structure
        museum = new Museum(
            "Musée Test",
            new List<Floor> { floor0, floor1, floor2 }
        );

        Floors = museum.Floors;
        CurrentFloor = museum.Floors.ElementAt(0);

        spaces = CurrentFloor.Spaces;
        image = CurrentFloor.MapImage;

        // Commands
        ChangeFloorCommand = ReactiveCommand.Create<Floor>(ChangeFloor);
        SelectingPoiCommand = ReactiveCommand.Create<PointOfInterest>(SelectPointOfInterest);
    }

    /// <summary>
    /// Changes the current floor and resets UI state (image, spaces, selections).
    /// </summary>
    /// <param name="floor">Floor to display.</param>
    public void ChangeFloor(Floor floor)
    {
        CurrentFloor = floor;
        Image = floor.MapImage;
        Spaces = floor.Spaces;

        SelectedSpace = null;
        SelectedPointOfInterest = null;
    }

    /// <summary>
    /// Selects a point of interest for display in the popup.
    /// </summary>
    /// <param name="pointOfInterest">Selected POI.</param>
    public void SelectPointOfInterest(PointOfInterest pointOfInterest)
    {
        SelectedPointOfInterest = pointOfInterest;
    }
}
