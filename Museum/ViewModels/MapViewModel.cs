using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Balise.Models;
using Balise.Repositories;
using Balise.Services;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Styles;
using ReactiveUI;
using SkiaSharp;

namespace Balise.ViewModels;

/// <summary>
/// A point of interest paired with its name already resolved in the session's language,
/// for display in lists.
/// </summary>
public record PoiListItem(string Name, PointOfInterest PointOfInterest);

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
    private readonly LanguageService languageService;
    private bool isReinitializeActive;
    private List<Floor> floors = new List<Floor>();
    private Bitmap? selectedPointOfInterestImage;

    public bool IsReinitializeActive
    {
        get => isReinitializeActive;
        set
        {
            this.RaiseAndSetIfChanged(ref isReinitializeActive, value);
        }
    }

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
            this.RaisePropertyChanged(nameof(SelectedSpaceName));
            this.RaisePropertyChanged(nameof(SelectedSpaceDescription));
            this.RaisePropertyChanged(nameof(SelectedSpacePointOfInterests));
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
            this.RaisePropertyChanged(nameof(SelectedPointOfInterestName));
            this.RaisePropertyChanged(nameof(SelectedPointOfInterestDescription));
            this.RaisePropertyChanged(nameof(SelectedPointOfInterestImage));
            _ = LoadImageAsync(value?.ImageUrl);

            Map.Layers.Remove(pointOfInterestLayer);
            pointOfInterestLayer = BuildPointOfInterestLayer();
            Map.Layers.Add(pointOfInterestLayer, 0);
        }
    }

    /// <summary>
    /// Name of the currently selected space, in the session's chosen language.
    /// </summary>
    public string? SelectedSpaceName => SelectedSpace?.GetName(languageService.CurrentLanguage);

    /// <summary>
    /// Description of the currently selected space, in the session's chosen language.
    /// </summary>
    public string? SelectedSpaceDescription => SelectedSpace?.GetDescription(languageService.CurrentLanguage);

    /// <summary>
    /// Points of interest of the currently selected space, with names resolved
    /// in the session's chosen language.
    /// </summary>
    public List<PoiListItem> SelectedSpacePointOfInterests => SelectedSpace?.PointOfInterests.Select(poi => new PoiListItem(poi.GetName(languageService.CurrentLanguage), poi))
                                                                                             .ToList() ?? new List<PoiListItem>();

    /// <summary>
    /// Name of the currently selected point of interest, in the session's chosen language.
    /// </summary>
    public string? SelectedPointOfInterestName => SelectedPointOfInterest?.GetName(languageService.CurrentLanguage);

    public Bitmap? SelectedPointOfInterestImage
    {
        get => selectedPointOfInterestImage;
        set => this.RaiseAndSetIfChanged(ref selectedPointOfInterestImage, value);
    }

    /// <summary>
    /// Description of the currently selected point of interest, in the session's chosen language.
    /// </summary>
    public string? SelectedPointOfInterestDescription => SelectedPointOfInterest?.GetDescription(languageService.CurrentLanguage);

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
    public List<Floor> Floors
    {
        get => floors;
        set => this.RaiseAndSetIfChanged(ref floors, value);
    }

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
    /// Map instance displayed by the view.
    /// </summary>
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
    /// Command used to close the side panel.
    /// </summary>
    public ICommand ClosePanelCommand { get; }

    /// <summary>
    /// Command used to zoom in the map.
    /// <\summary>
    public ICommand ZoomInCommand { get; }

    /// <summary>
    /// Command used to zoom out the map.
    /// <\summary>
    public ICommand ZoomOutCommand { get; }

    /// <summary>
    /// Command used to restore the default map view.
    /// </summary>
    public ICommand ReinitializeCommand { get; }

    /// <summary>
    /// Activator for ReactiveUI lifecycle management.
    /// </summary>
    public ViewModelActivator Activator => new ViewModelActivator();

    /// <summary>
    /// Initializes the ViewModel and starts loading the museum data.
    /// </summary>
    /// <param name="museumRepository">
    /// Repository used to retrieve museum data.
    /// </param>
    public MapViewModel(MuseumRepository museumRepository, LanguageService langService)
    {
        repository = museumRepository;
        languageService = langService;

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
        ReinitializeCommand = ReactiveCommand.Create(ReinitializeMap);
        ZoomInCommand = ReactiveCommand.Create(ZoomIn);
        ZoomOutCommand = ReactiveCommand.Create(ZoomOut);

        Map.Tapped += OnMapTapped;
    }

    /// <summary>
    /// Handles map click events to select either a space or a point of interest.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Map event arguments.</param>
    private void OnMapTapped(object? sender, MapEventArgs e)
    {
        var poiInfo = e.GetMapInfo(new[] { pointOfInterestLayer });

        if (poiInfo.Feature?["PointOfInterest"] is PointOfInterest poi)
        {
            SelectedSpace = poi.Space;
            SelectedPointOfInterest = poi;
            return;
        }

        var spaceInfo = e.GetMapInfo(new[] { spacesLayer });

        if (spaceInfo.Feature?["Space"] is Space space)
        {
            SelectedPointOfInterest = null;
            SelectedSpace = space;
        }
    }

    /// <summary>
    /// Zoom the map in.
    /// <\summary>
    private void ZoomIn()
    {
        Map.Navigator.ZoomIn();
    }

    /// <summary>
    /// Zoom the map out.
    /// <\summary>
    private void ZoomOut()
    {
        Map.Navigator.ZoomOut();
    }

    /// <summary>
    /// Restores the map to its default extent.
    /// </summary>
    public void ReinitializeMap()
    {
        Map.Navigator.ZoomToBox(new MRect(0, 0, image.Size.Width, image.Size.Height));
        IsReinitializeActive = false;
    }

    /// <summary>
    /// Builds and adds all map layers for the current floor.
    /// </summary>
    private void BuildLayers()
    {
        var floorPlanLayer = BuildFloorPlanLayer();
        spacesLayer = BuildSpacesLayer();
        pointOfInterestLayer = BuildPointOfInterestLayer();

        Map.Navigator.OverrideZoomBounds = new MMinMax(0.25, 2.5);
        Map.Navigator.OverridePanBounds = floorPlanLayer.Extent;

        Map.Layers.Add(floorPlanLayer, -1);
        Map.Layers.Add(spacesLayer, 0);
        Map.Layers.Add(pointOfInterestLayer, 0);
    }

    /// <summary>
    /// Creates the layer containing all points of interest for the current floor.
    /// </summary>
    /// <returns>A memory layer containing point of interest features.</returns>
    private MemoryLayer BuildPointOfInterestLayer()
    {
        var features = CurrentFloor.Spaces.SelectMany(space => space.PointOfInterests).Select(poi =>
        {
            var feature = new GeometryFeature(poi.Position);
            feature["PointOfInterest"] = poi;

            bool isSelected = poi == SelectedPointOfInterest;

            if (isSelected)
            {
                feature.Styles.Add(new SymbolStyle
                {
                    Fill = new Brush(Color.FromString("#CA005D")),
                    SymbolScale = 0.5,
                    Outline = new Pen(Color.White, 2)
                });
            }
            else
            {
                feature.Styles.Add(new SymbolStyle
                {
                    Fill = new Brush(Color.White),
                    SymbolScale = 0.5,
                    Outline = null,
                });
            }

            return (IFeature)feature;
        }).ToList();

        return new MemoryLayer
        {
            Name = "PointOfInterest",
            Features = features, 
            Style = null
        };
    }

    /// <summary>
    /// Creates the layer containing all spaces for the current floor.
    /// </summary>
    /// <returns>A memory layer containing space features.</returns>
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
                Fill = new Brush(Color.FromString("#CA005D")),
                Opacity = 0.90f,
                Outline = new Pen(Color.White, 2)
            }
        };
    }

    /// <summary>
    /// Creates the raster layer displaying the current floor plan.
    /// </summary>
    /// <returns>A memory layer containing the floor plan image.</returns>
    private MemoryLayer BuildFloorPlanLayer()
    {
        // TODO: Replace png by svg

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

    /// <summary>
    /// Loads the museum data and initializes the first displayed floor.
    /// </summary>
    /// <returns>A task representing the asynchronous initialization operation.</returns>
    public async Task InitializeAsync()
    {
        var museums = await repository.GetAllAsync();
        museum = museums.FirstOrDefault();
        Map.Widgets.Clear();

        if (museum != null)
        {
            Floors = museum.Floors;
            CurrentFloor = museum.Floors.ElementAt(0);
            spaces = CurrentFloor.Spaces;

            using var stream = AssetLoader.Open(new Uri(CurrentFloor.MapImageUrl));
            image = new Bitmap(stream);

            BuildLayers();
            ReinitializeMap();
        }
    }

    /// <summary>
    /// Changes the current floor and rebuilds the displayed map layers.
    /// </summary>
    /// <param name="floor">Floor to display.</param>
    public void ChangeFloor(Floor floor)
    {
        CurrentFloor = floor;
        Map.Layers.ClearAllGroups();
        BuildLayers();
        ReinitializeMap();

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

    public void OnMapMoved()
    {
        IsReinitializeActive = true;
    }

    private async Task LoadImageAsync(string? url)
    {
        if (string.IsNullOrEmpty(url))
        {
            SelectedPointOfInterestImage = null;
            return;
        }

        try
        {
            using var httpClient = new HttpClient();
            var bytes = await httpClient.GetByteArrayAsync(url);
            using var stream = new MemoryStream(bytes);
            SelectedPointOfInterestImage = new Bitmap(stream);
        }
        catch
        {
            SelectedPointOfInterestImage = null;
        }
    }
}
