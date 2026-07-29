using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Balise.Services;
using ReactiveUI;

namespace Balise.ViewModels;

/// <summary>
/// Backs the connection view, exposing the login command and the browser
/// used to drive the hosted WebView.
/// </summary>
public class ConnectionViewModel : ViewModelBase
{
    private readonly AuthenticationService authenticationService;
    private readonly Action<ViewModelBase> navigate;

    /// <summary>
    /// The browser instance that the hosted WebView binds to and reports navigations to.
    /// </summary>
    public Browser Browser { get; }

    /// <summary>
    /// Command that triggers the login flow.
    /// </summary>
    public ICommand LoginCommand { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="ConnectionViewModel"/>.
    /// </summary>
    /// <param name="auth">The authentication service used to perform login.</param>
    /// <param name="browser">The shared browser instance used by the OIDC client.</param>
    /// <param name="setCurrentView">Callback invoked with the next view once login succeeds.</param>
    public ConnectionViewModel(AuthenticationService auth, Browser browser, Action<ViewModelBase> navigate)
    {
        authenticationService = auth;
        Browser = browser;
        this.navigate = navigate;
        LoginCommand = ReactiveCommand.CreateFromTask(LoginAsync);
    }

    /// <summary>
    /// Runs the login flow through the authentication service, then navigates
    /// away from this view once authentication succeeds.
    /// </summary>
    public async Task LoginAsync()
    {
        await authenticationService.LoginAsync();

        if (authenticationService.IsAuthenticated)
        {
            navigate(new ConnectionSuccessViewModel());
        }
        else
        {
            navigate(new ConnectionFailureViewModel());
        }
    }
}
