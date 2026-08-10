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
using Microsoft.Extensions.Configuration;
using Duende.IdentityModel.OidcClient;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Balise;

public partial class App : Application
{
    private IServiceProvider? services;
    private static readonly GeometryFactory Factory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 999001);
    private static readonly InMemoryDatabaseRoot InMemoryRoot = new InMemoryDatabaseRoot();
    private Browser browser = new Browser();

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var auth0Client = new OidcClient(new OidcClientOptions
        {
            Authority = config["Auth0:Domain"],
            ClientId = config["Auth0:ClientId"],
            Browser = browser,
            RedirectUri = "http://127.0.0.1:7890/callback",
            Scope = "openid profile email",
        });

        var collection = new ServiceCollection();

        collection.AddDbContextFactory<AppDbContext>(options => options.UseInMemoryDatabase("BaliseDb", InMemoryRoot));

        collection.AddSingleton<UserRepository>();
        collection.AddSingleton<AgendaRepository>();
        collection.AddSingleton<MuseumRepository>();
        collection.AddSingleton<SpaceRepository>();
        collection.AddSingleton<FloorRepository>();
        collection.AddSingleton<PointOfInterestRepository>();
        collection.AddSingleton<FormService>();
        collection.AddSingleton<AuthenticationService>();
        collection.AddSingleton<MainWindowViewModel>();
        collection.AddSingleton(auth0Client);
        collection.AddSingleton<AuthenticationService>();
        collection.AddSingleton(browser);
        collection.AddSingleton(Factory);

        services = collection.BuildServiceProvider();

        var authService = services.GetRequiredService<AuthenticationService>();
        var userRepository = services.GetRequiredService<UserRepository>();
        var museumRepository = services.GetRequiredService<MuseumRepository>();
        var agendaRepository = services.GetRequiredService<AgendaRepository>();

        authService = new AuthenticationService(auth0Client, userRepository);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = services.GetRequiredService<MainWindowViewModel>(),
            };
        }

        Museum museum = await Hardcoding(museumRepository, userRepository);

        try
        {
            HttpClient client = new HttpClient();
            EventFetcherService service = new EventFetcherService(client, museum);
    
            string html = await service.GetAgendaHtmlAsync();
            var events = service.ParseEvents(html);
    
            await agendaRepository.AddRangeAsync(await events);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Échec du chargement de l'agenda : {ex.Message}");
        }

        base.OnFrameworkInitializationCompleted();
    }

    // Temporary. Just hardcodes a few things. I put it out the OnFrameworkInitializationCompleted() because it was becoming hard to read.
    private async Task<Museum> Hardcoding(MuseumRepository museumRepository, UserRepository userRepository)
    {
        // Museum root structure
        var museum = new Museum(
            "Musée Test",
            new List<Floor>()
        );

        var floor0 = new Floor(0, "avares://Balise/Assets/REZ_GC.png", new List<Space>(), museum);
        var floor1 = new Floor(1, "avares://Balise/Assets/GC_1.png", new List<Space>(), museum);
        var floor2 = new Floor(2, "avares://Balise/Assets/GC_2.png", new List<Space>(), museum);

        museum.Floors.Add(floor0);
        museum.Floors.Add(floor1);
        museum.Floors.Add(floor2);

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

        var poi1 = new PointOfInterest("Statue A", "Statue en marbre", Factory.CreatePoint(new Coordinate(120, 120)), space1);
        var poi2 = new PointOfInterest("Statue B", "Statue en bronze", Factory.CreatePoint(new Coordinate(180, 160)), space1);
        var poi3 = new PointOfInterest("Vitrine C", "Objets anciens", Factory.CreatePoint(new Coordinate(350, 300)), space2);

        space1.PointOfInterests.Add(poi1);
        space2.PointOfInterests.Add(poi2);
        space2.PointOfInterests.Add(poi3);

        await museumRepository.AddAsync(museum);

        var user = new User
        {
            Email = "ludovic.hansen@isl-edu.be",
            FirstName = "Ludovic",
            LastName = "Hansen",
            Role = Enum.UserRole.Admin
        };

        await userRepository.AddUserAsync(user);

        return museum;
    }
}