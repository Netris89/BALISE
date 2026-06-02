using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Balise.Data;
using Balise.Repositories;
using Balise.Services;
using Balise.ViewModels;
using Balise.Views;

namespace Balise;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("BaliseDb")
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