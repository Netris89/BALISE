using System.Threading.Tasks;
using System.Windows.Input;
using Balise.Services;

// using Auth0.OidcClient;
using ReactiveUI;

namespace Balise.ViewModels;

public class ConnectionViewModel : ViewModelBase
{
    private readonly AuthenticationService authenticationService;
    // private readonly UserRepository repository;
    /* private bool isError;
    private ClaimsPrincipal? user;
    private string? token; */

    public ICommand LoginCommand { get; }

    public ConnectionViewModel(AuthenticationService auth/* , UserRepository userRepository */)
    {
        authenticationService = auth;
        // repository = userRepository;
        LoginCommand = ReactiveCommand.CreateFromTask(LoginAsync);
    }

    public async Task LoginAsync()
    {
        await authenticationService.LoginAsync();
    }
}
