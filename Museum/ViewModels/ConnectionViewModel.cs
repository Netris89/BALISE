using System.Threading.Tasks;
using System.Windows.Input;
using Balise.Services;
using ReactiveUI;

namespace Balise.ViewModels;

/// <summary>
/// Backs the connection view, exposing the login command.
/// </summary>
public class ConnectionViewModel : ViewModelBase
{
    private readonly AuthenticationService authenticationService;

    /// <summary>
    /// Command that triggers the login flow.
    /// </summary>
    public ICommand LoginCommand { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="ConnectionViewModel"/>.
    /// </summary>
    /// <param name="auth">The authentication service used to perform login.</param>
    public ConnectionViewModel(AuthenticationService auth)
    {
        authenticationService = auth;
        LoginCommand = ReactiveCommand.CreateFromTask(LoginAsync);
    }

    /// <summary>
    /// Runs the login flow through the authentication service.
    /// </summary>
    public async Task LoginAsync()
    {
        await authenticationService.LoginAsync();
    }
}
