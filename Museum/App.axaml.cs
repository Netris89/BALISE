using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Museum.Data;
using Museum.Repositories;
using Museum.Services;
using Museum.ViewModels;
using Museum.Views;

namespace Museum;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("MuseumDb")
            .Options;

        var dbContext = new AppDbContext(options);

        var userRepository = new UserRepository(dbContext);
        var formService = new FormService(userRepository);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(formService)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}