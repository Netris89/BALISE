using Avalonia.Controls;
using Avalonia.Interactivity;
using Balise.ViewModels;

namespace Balise.Views;

public partial class ConnectionView : UserControl
{
    public ConnectionView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ConnectionViewModel vm)
        {
            vm.LoginCommand.Execute(null);
        }
    }

    private void OnNavigating(object? sender, WebViewNavigationStartingEventArgs e)
    {
        if (DataContext is ConnectionViewModel vm)
        {
            vm.Browser.OnNavigating(e);
        }
    }
}