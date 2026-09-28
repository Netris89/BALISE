using Balise.Services;

namespace Balise.ViewModels;

/// <summary>
/// Backs the diconnect view, exposing the browser
/// used to drive the hosted WebView.
/// </summary>
public class DisconnectViewModel : ViewModelBase
{
    /// <summary>
    /// The browser instance that the hosted WebView binds to and reports navigations to.
    /// </summary>
    public Browser Browser { get; }

    public DisconnectViewModel(Browser browser)
    {
        Browser = browser;
    }
}
