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
using System.Net.Http;
using Auth0.OidcClient;
using Microsoft.Extensions.Configuration;

namespace Balise;

public partial class App : Application
{
    private static DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("BaliseDb").Options;
    private static AppDbContext context = new AppDbContext(options);
    private static UserRepository userRepository = new UserRepository(context);
    private static AgendaRepository agendaRepository = new AgendaRepository(context);
    private MuseumRepository museumRepository = new MuseumRepository(context);
    private FormService formService = new FormService(userRepository);
    private static readonly GeometryFactory Factory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 999001);

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var auth0Client = new Auth0Client(new Auth0ClientOptions
        {
            Domain = config["Auth0:Domain"],
            ClientId = config["Auth0:ClientId"]
        });

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(formService, museumRepository, agendaRepository, auth0Client)
            };
        }


        // Museum root structure
        var museum = new Museum(
            "Musée Test",
            new List<Floor>()
        );

        // Floors (each floor has its own map and spaces)
        var floor0 = new Floor(
            0,
            "avares://Balise/Assets/REZ_GC.png",
            new List<Space>(),
            museum
        );

        var floor1 = new Floor(
            1,
            "avares://Balise/Assets/GC_1.png",
            new List<Space>(),
            museum
        );

        var floor2 = new Floor(
            2,
            "avares://Balise/Assets/GC_2.png",
            new List<Space>(),
            museum
        );

        museum.Floors.Add(floor0);
        museum.Floors.Add(floor1);
        museum.Floors.Add(floor2);

        // Spaces (rooms containing POIs and defined by polygons)
        var space1 = new Space(
            "Salle Romaine",
            "Une salle contenant des artéfacts romains",
            new List<PointOfInterest>(),
            Factory.CreatePolygon(new[]
            {
                new Coordinate(400, 300),
                new Coordinate(550, 300),
                new Coordinate(550, 500),
                new Coordinate(400, 500),
                new Coordinate(400, 300)
            }),
            floor0
        );

        var space2 = new Space(
            "Salle Médiévale",
            "Une salle contenant des artéfacts médiévaux",
            new List<PointOfInterest>(),
            Factory.CreatePolygon(new[]
            {
                new Coordinate(300, 200),
                new Coordinate(450, 200),
                new Coordinate(450, 400),
                new Coordinate(300, 400),
                new Coordinate(300, 200)
            }),
            floor1
        );

        floor0.Spaces.Add(space1);
        floor1.Spaces.Add(space2);

        // Points of interest (test data)
        var poi1 = new PointOfInterest(
            "Statue A",
            "Statue en marbre",
            Factory.CreatePoint(new Coordinate(120, 120)),
            space1
        );

        var poi2 = new PointOfInterest(
            "Statue B",
            "Statue en bronze",
            Factory.CreatePoint(new Coordinate(180, 160)),
            space1
        );

        var poi3 = new PointOfInterest(
            "Vitrine C",
            "Objets anciens",
            Factory.CreatePoint(new Coordinate(350, 300)),
            space2
        );

        space1.PointOfInterests.Add(poi1);
        space2.PointOfInterests.Add(poi2);
        space2.PointOfInterests.Add(poi3);

        _ = museumRepository.AddAsync(museum);

        HttpClient client = new HttpClient();
        EventFetcherService service = new EventFetcherService(client, museum);

        string html = await service.GetAgendaHtmlAsync();
        var events = service.ParseEvents(html);

        foreach (var ev in await events)
        {
            await agendaRepository.AddAsync(ev);
        }

        base.OnFrameworkInitializationCompleted();
    }
}