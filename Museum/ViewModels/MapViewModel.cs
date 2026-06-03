using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Balise.Models;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.Operation.Buffer;
using ReactiveUI;

namespace Balise.ViewModels;

public class MapViewModel : ViewModelBase
{
    private Bitmap image;
    private Museum museum;
    private static readonly GeometryFactory Factory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 999001);

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

    public ReactiveCommand<int, Unit> ChangeFloorCommand { get; }

    public ViewModelActivator Activator => new ViewModelActivator();

    public MapViewModel()
    {
        // Hardcoding a museum for testing purposes
        // Points of intest
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

        // Spaces
        var space1 = new Space(
            "Salle Romaine",
            new List<PointOfInterest> { poi1, poi2 },
            Factory.CreatePolygon(new[]
            {
                new Coordinate(0, 0),
                new Coordinate(0, 100),
                new Coordinate(100, 100),
                new Coordinate(100, 0),
                new Coordinate(0,0)
            })
        );

        var space2 = new Space(
            "Salle Médiévale",
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

        //Floors
        var floor0 = new Floor(
            0,
            "avares://Balise/Assets/REZ_GC.png",
            new List<Space> { space1 }
        );

        var floor1 = new Floor(
            1,
            "avares://Balise/Assets/GC_1.png",
            new List<Space> { space2 }
        );

        var floor2 = new Floor(
            2,
            "avares://Balise/Assets/GC_2.png",
            new List<Space>()
        );

        // Museum
        museum = new Museum(
            "Musée Test",
            new List<Floor> { floor0, floor1, floor2 }
        );

        var uri = new Uri(museum.Floors.ElementAt(0).MapImageUri);
        image = new Bitmap(AssetLoader.Open(uri));

        ChangeFloorCommand = ReactiveCommand.Create<int>(ChangeFloor);
    }

    public void ChangeFloor(int floorNumber)
    {
        var uri = new Uri(museum.Floors.ElementAt(floorNumber).MapImageUri);
        Image = new Bitmap(AssetLoader.Open(uri));
    }
}
