using System.Threading.Tasks;
using System.Windows.Input;
using Balise.Services;

// using Auth0.OidcClient;
using ReactiveUI;

namespace Balise.ViewModels;

public class ConnectionViewModel : ViewModelBase
{
    private readonly AuthenticationService authenticationService;

    public ICommand LoginCommand { get; }

    public ConnectionViewModel(AuthenticationService auth)
    {
        authenticationService = auth;
        LoginCommand = ReactiveCommand.CreateFromTask(LoginAsync);
    }

    public async Task LoginAsync()
    {
        await authenticationService.LoginAsync();
    }
}
