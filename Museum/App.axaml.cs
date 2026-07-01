using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Balise.Data;
using Balise.Repositories;
using Balise.Services;
using Balise.ViewModels;
using Balise.Views;
using NetTopologySuite.Geometries;
using NetTopologySuite;
using Balise.Models;
using System.Collections.Generic;
using Avalonia.Platform;
using Avalonia.Media.Imaging;
using System;

namespace Balise;

public partial class App : Application
{
    private static DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("BaliseDb").Options;
    private static AppDbContext context = new AppDbContext(options);
    private static UserRepository userRepository = new UserRepository(context);
    private MuseumRepository museumRepository = new MuseumRepository(context);
    private FormService formService = new FormService(userRepository); 
    private static readonly GeometryFactory Factory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 999001);

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(formService, museumRepository)
            };
        }

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

        // Museum root structure
        var museum = new Museum(
            "Musée Test",
            new List<Floor> { floor0, floor1, floor2 }
        );

        museumRepository.AddAsync(museum);

        base.OnFrameworkInitializationCompleted();
    }
}