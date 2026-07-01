using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Balise.Models;
using Balise.Repositories;
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
        museum = repository.Get();
        Floors = museum.Floors;
        CurrentFloor = museum.Floors.ElementAt(0);

        spaces = CurrentFloor.Spaces;
        var uri = new Uri(CurrentFloor.MapImageUrl);
        using var stream = AssetLoader.Open(uri);
        image = new Bitmap(stream);

        // Commands
        ChangeFloorCommand = ReactiveCommand.Create<Floor>(ChangeFloor);
        SelectingPoiCommand = ReactiveCommand.Create<PointOfInterest>(SelectPointOfInterest);
        ClosePanelCommand = ReactiveCommand.Create(ClosePanel);
    }

    /// <summary>
    /// Changes the current floor and resets UI state (image, spaces, selections).
    /// </summary>
    /// <param name="floor">Floor to display.</param>
    public void ChangeFloor(Floor floor)
    {
        CurrentFloor = floor;
        var uri = new Uri(CurrentFloor.MapImageUrl);
        using var stream = AssetLoader.Open(uri);
        Image = new Bitmap(stream);
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

    /// <summary>
    /// Closes the information panel by clearing the currently selected space.
    /// </summary>
    public void ClosePanel()
    {
        SelectedSpace = null;
    }
}
