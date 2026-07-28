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

    /// <summary>
    /// Starts the OIDC login flow once the view has finished loading.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">The event arguments.</param>
    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ConnectionViewModel vm)
        {
            vm.LoginCommand.Execute(null);
        }
    }

    /// <summary>
    /// Forwards each WebView navigation to the browser instance so it can
    /// detect and intercept the redirect URI.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">The navigation event arguments.</param>
    private void OnNavigating(object? sender, WebViewNavigationStartingEventArgs e)
    {
        if (DataContext is ConnectionViewModel vm)
        {
            vm.Browser.OnNavigating(e);
        }
    }
}