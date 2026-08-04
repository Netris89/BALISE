using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Balise.Models;
using Balise.Repositories;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Styles;
using ReactiveUI;
using SkiaSharp;

namespace Balise.ViewModels;

/// <summary>
/// ViewModel responsible for museum map display, floor navigation,
/// space selection and point of interest interaction.
/// </summary>
public class MapViewModel : ViewModelBase
{
    private Bitmap image;
    private Museum? museum;
    private List<Space> spaces;
    private Space? selectedSpace;
    private PointOfInterest? selectedPointOfInterest;
    private MemoryLayer spacesLayer = new MemoryLayer();
    private MemoryLayer pointOfInterestLayer = new MemoryLayer();

    private MuseumRepository repository;

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

    public Map Map { get; } = new Map();

    /// <summary>
    /// Command used to change the displayed floor.
    /// </summary>
    public ReactiveCommand<Floor, Unit> ChangeFloorCommand { get; }

    /// <summary>
    /// Command used to select a point of interest.
    /// </summary>
    public ReactiveCommand<PointOfInterest, Unit> SelectingPoiCommand { get; }

    /// <summary>
    /// Command used to close de side panel.
    /// </summary>
    public ICommand ClosePanelCommand { get; }

    /// <summary>
    /// Activator for ReactiveUI lifecycle management.
    /// </summary>
    public ViewModelActivator Activator => new ViewModelActivator();

    /// <summary>
    /// Initializes the ViewModel with a sample museum structure.
    /// (Currently hardcoded for testing purposes.)
    /// </summary>
    public MapViewModel(MuseumRepository museumRepository)
    {
        repository = museumRepository;

        // Temporarily load empty values until the museum is fetched asynchronously
        Floors = new List<Floor>();
        spaces = new List<Space>();
        CurrentFloor = new Floor(0, "", spaces, new Museum());
        image = new Bitmap(AssetLoader.Open(new Uri("avares://Balise/Assets/1_RDC 2.png"))); // TODO: placeholder

        _ = InitializeAsync();

        // Commands
        ChangeFloorCommand = ReactiveCommand.Create<Floor>(ChangeFloor);
        SelectingPoiCommand = ReactiveCommand.Create<PointOfInterest>(SelectPointOfInterest);
        ClosePanelCommand = ReactiveCommand.Create(ClosePanel);

        BuildLayers();
        Map.Tapped += OnMapTapped;
    }

    private void OnMapTapped(object? sender, MapEventArgs e)
    {
        var mapInfo = e.GetMapInfo(new[] { pointOfInterestLayer, spacesLayer });

        if (mapInfo.Feature is null)
        {
            return;
        }

        if (mapInfo.Feature["PointOfInterest"] is PointOfInterest poi)
        {
            SelectedPointOfInterest = poi;
        }
        else if (mapInfo.Feature["Space"] is Space space)
        {
            SelectedSpace = space;
        }
    }

    private void BuildLayers()
    {
        var floorPlanLyer = BuildFloorPlanLayer();
        spacesLayer = BuildSpacesLayer();
        pointOfInterestLayer = BuildPointOfInterestLayer();

        Map.Layers.Add(floorPlanLyer, -1);
        Map.Layers.Add(spacesLayer, 0);
        Map.Layers.Add(pointOfInterestLayer, 0);
    }

    private MemoryLayer BuildPointOfInterestLayer()
    {
        var features = CurrentFloor.Spaces.SelectMany(space => space.PointOfInterests).Select(poi =>
        {
            var feature = new GeometryFeature(poi.Position);
            feature["PointOfInterest"] = poi;
            return (IFeature)feature;
        }).ToList();

        return new MemoryLayer
        {
            Name = "PointOfInterest",
            Features = features,
            Style = new SymbolStyle
            {
                Fill = new Brush(Color.Blue),
                SymbolScale = 0.5
            }
        };
    }

    private MemoryLayer BuildSpacesLayer()
    {
        var features = CurrentFloor.Spaces.Select(space =>
        {
            var feature = new GeometryFeature(space.Boundary);
            feature["Space"] = space;
            return (IFeature)feature;
        }).ToList();

        return new MemoryLayer
        {
            Name = "Spaces",
            Features = features,
            Style = new VectorStyle
            {
                Fill = new Brush(Color.Red),
                Outline = new Pen(Color.DarkRed, 2)
            }
        };
    }

    private MemoryLayer BuildFloorPlanLayer()
    {
        using var stream = AssetLoader.Open(new Uri(CurrentFloor.MapImageUrl));
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);

        var imageBytes = memoryStream.ToArray();
        var image = SKBitmap.Decode(imageBytes);

        var extent = new MRect(0, 0, image.Width, image.Height);

        var raster = new MRaster(imageBytes, extent);
        var feature = new RasterFeature(raster);
        feature.Styles.Add(new RasterStyle());

        return new MemoryLayer
        {
            Name = "FloorPlan",
            Features = new List<IFeature> { feature }
        };
    }

    public async Task InitializeAsync()
    {
        museum = await repository.GetAsync(1);

        if (museum != null)
        {
            Floors = museum.Floors;
            CurrentFloor = museum.Floors.ElementAt(0);
            spaces = CurrentFloor.Spaces;
            var uri = new Uri(CurrentFloor.MapImageUrl);
            using var stream = AssetLoader.Open(uri);
            image = new Bitmap(stream);
        }
    }

    /// <summary>
    /// Changes the current floor and resets UI state (image, spaces, selections).
    /// </summary>
    /// <param name="floor">Floor to display.</param>
    public void ChangeFloor(Floor floor)
    {
        CurrentFloor = floor;
        Map.Layers.ClearAllGroups();
        BuildLayers();

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

    /// <summary>
    /// Closes the information panel by clearing the currently selected space.
    /// </summary>
    public void ClosePanel()
    {
        SelectedSpace = null;
    }
}
