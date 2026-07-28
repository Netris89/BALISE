using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Duende.IdentityModel.OidcClient.Browser;
using ReactiveUI;

namespace Balise.Services;

/// <summary>
/// Implements the OIDC browser step by exposing the authorization URL to a
/// hosted NativeWebView and completing once the redirect URI is reached.
/// </summary>
public class Browser : ReactiveObject, IBrowser
{
    private string? currentUrl;
    private string? endUrl;
    private TaskCompletionSource<BrowserResult>? completionSource;

    /// <summary>
    /// The URL the hosting NativeWebView should currently display.
    /// </summary>
    public string? CurrentUrl
    {
        get => currentUrl;
        private set => this.RaiseAndSetIfChanged(ref currentUrl, value);
    }

    /// <summary>
    /// Sets the authorization URL for the hosting NativeWebView and returns a task
    /// that completes once <see cref="OnNavigating"/> detects the redirect URI,
    /// or the cancellation token is triggered.
    /// </summary>
    /// <param name="options">The browser options, including the start and end URLs.</param>
    /// <param name="cancellationToken">A token used to cancel the pending login.</param>
    /// <returns>A task that resolves to the resulting <see cref="BrowserResult"/>.</returns>
    public Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
    {
        endUrl = options.EndUrl;
        completionSource = new TaskCompletionSource<BrowserResult>();

        cancellationToken.Register(() => completionSource.TrySetResult(new BrowserResult
        {
            ResultType = BrowserResultType.Timeout
        }));

        CurrentUrl = options.StartUrl;

        return completionSource.Task;
    }

    /// <summary>
    /// Called by the hosting view on every NativeWebView navigation.
    /// Completes the pending login once the redirect URI is reached, and
    /// cancels that navigation so the WebView never tries to load it.
    /// </summary>
    /// <param name="args">The navigation event arguments from NativeWebView.</param>
    public void OnNavigating(WebViewNavigationStartingEventArgs args)
    {
        if (args.Request is null)
        {
            return;
        }

        string url = args.Request.ToString();

        if (endUrl is not null && url.StartsWith(endUrl))
        {
            args.Cancel = true;

            completionSource?.TrySetResult(new BrowserResult
            {
                ResultType = BrowserResultType.Success,
                Response = url
            });
        }
    }
}
