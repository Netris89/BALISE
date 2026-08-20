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
using System.Linq;

namespace Balise;

public partial class App : Application
{
    private IServiceProvider? services;
    private static readonly GeometryFactory factory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 999001);
    // private static readonly InMemoryDatabaseRoot InMemoryRoot = new InMemoryDatabaseRoot();
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

        var connectionString = config["Postgres:ConnectionString"];
        // collection.AddDbContextFactory<AppDbContext>(options => options.UseInMemoryDatabase("BaliseDb", InMemoryRoot));
        collection.AddDbContextFactory<AppDbContext>(options => options.UseNpgsql(connectionString, npgsql => npgsql.UseNetTopologySuite()));

        collection.AddSingleton<UserRepository>();
        collection.AddSingleton<AgendaRepository>();
        collection.AddSingleton<MuseumRepository>();
        collection.AddSingleton<SpaceRepository>();
        collection.AddSingleton<FloorRepository>();
        collection.AddSingleton<PointOfInterestRepository>();
        collection.AddSingleton<FormService>();
        collection.AddSingleton<AuthenticationService>();
        collection.AddSingleton<MainWindowViewModel>();
        collection.AddSingleton<LanguageService>();
        collection.AddSingleton(auth0Client);
        collection.AddSingleton(browser);
        collection.AddSingleton(factory);

        services = collection.BuildServiceProvider();

        var userRepository = services.GetRequiredService<UserRepository>();
        var museumRepository = services.GetRequiredService<MuseumRepository>();
        var agendaRepository = services.GetRequiredService<AgendaRepository>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = services.GetRequiredService<MainWindowViewModel>(),
            };
        }

        try
        {
            using var context = await services.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContextAsync();
            await context.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Échec de l'initialisation de la base de données : {ex.Message}");
        }

        var existingMuseums = await museumRepository.GetAllAsync();

        Museum? museum = existingMuseums.FirstOrDefault();

        try
        {
            HttpClient client = new HttpClient();
            EventFetcherService service = new EventFetcherService(client, museum!);

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
}