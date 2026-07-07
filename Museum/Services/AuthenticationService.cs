using System.Threading.Tasks;
using Balise.Models;
using Balise.Repositories;
using Duende.IdentityModel.OidcClient;

namespace Balise.Services;

public class AuthenticationService
{
    private readonly OidcClient oidcClient;
    private readonly UserRepository userRepository;
    public User? CurrentUser { get; set; }
    public bool IsAuthenticated { get; set; }

    public AuthenticationService(OidcClient client, UserRepository repository)
    {
        oidcClient = client;
        userRepository = repository;
    }

    public async Task LoginAsync()
    {
        var result = await oidcClient.LoginAsync();

        if (result.IsError == true)
        {
            return;
        }

        string? id = result.User.FindFirst("sub")?.Value;
        string? email = result.User.FindFirst("email")?.Value;

        if (email != null)
        {
            CurrentUser = await userRepository.GetByAuth0IdAsync(email)!;
        }

        if (CurrentUser != null)
        {
            IsAuthenticated = true;
        }
    }

    public void LogoutAsync()
    {
        CurrentUser = null;
        IsAuthenticated = false;
    }
}
