using Avalonia.Controls;
using Balise.ViewModels;

namespace Balise.Views;

public partial class DisconnectView : UserControl
{
    public DisconnectView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Forwards each WebView navigation to the browser instance so it can
    /// detect and intercept the redirect URI.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">The navigation event arguments.</param>
    private void OnNavigating(object? sender, WebViewNavigationStartingEventArgs e)
    {
        if (DataContext is DisconnectViewModel vm)
        {
            vm.Browser.OnNavigating(e);
        }
    }
}