using System;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Museum.Models;
using NetTopologySuite.Geometries;
using ReactiveUI;

namespace Museum.ViewModels;

public class MapViewModel : ViewModelBase
{
    private Bitmap image;
    private Space space;

    public Space? SelectedSpace
    {
        get;
        set;
    }

    public bool IsSpaceDetailsVisible
    {
        get;
        set;
    }

    public Bitmap Image
    {
        get => image;
        set => this.RaiseAndSetIfChanged(ref image, value);
    }

    public ViewModelActivator Activator => new ViewModelActivator();

    public MapViewModel()
    {
        var uri = new Uri("avares://Museum/Assets/REZ_GC.png");

        var polygon = new Polygon(
            new LinearRing(
            [
                new Coordinate(0, 0),
                new Coordinate(10, 0),
                new Coordinate(10, 10),
                new Coordinate(0, 10),
                new Coordinate(0, 0)
            ]));
        image = new(AssetLoader.Open(uri));
    }
}
