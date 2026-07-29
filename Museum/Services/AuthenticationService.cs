using System.Threading.Tasks;
using Balise.Models;
using Balise.Repositories;
using Duende.IdentityModel.Client;
using Duende.IdentityModel.OidcClient;
using ReactiveUI;

namespace Balise.Services;

/// <summary>
/// Manages the OIDC login flow and tracks the currently authenticated user.
/// </summary>
public class AuthenticationService : ReactiveObject
{
    private readonly OidcClient oidcClient;
    private readonly UserRepository userRepository;
    private User? currentUser;
    private bool isAuthenticated;

    /// <summary>
    /// The user currently logged in, or <c>null</c> if no one is authenticated.
    /// </summary>
    public User? CurrentUser
    {
        get => currentUser;
        set => this.RaiseAndSetIfChanged(ref currentUser, value);
    }

    /// <summary>
    /// Indicates whether a user is currently authenticated.
    /// </summary>
    public bool IsAuthenticated
    {
        get => isAuthenticated;
        set => this.RaiseAndSetIfChanged(ref isAuthenticated, value);
    }

    /// <summary>
    /// Initializes a new instance of <see cref="AuthenticationService"/>.
    /// </summary>
    /// <param name="client">The OIDC client used to perform the login.</param>
    /// <param name="repository">The repository used to look up the logged-in user.</param>
    public AuthenticationService(OidcClient client, UserRepository repository)
    {
        oidcClient = client;
        userRepository = repository;
    }

    /// <summary>
    /// Runs the OIDC login flow and, on success, loads the matching user by email
    /// and marks them as authenticated.
    /// </summary>
    public async Task LoginAsync()
    {
        var loginRequest = new LoginRequest
        {
            FrontChannelExtraParameters = new Parameters
            {
                { "prompt", "login" }
            }
        };

        var result = await oidcClient.LoginAsync(loginRequest);

        if (result.IsError == true)
        {
            return;
        }

        string? email = result.User.FindFirst("email")?.Value;

        if (email != null)
        {
            CurrentUser = await userRepository.GetByEmailAsync(email)!;
        }

        if (CurrentUser != null)
        {
            IsAuthenticated = true;
        }
    }

    /// <summary>
    /// Clears the current user and marks the session as unauthenticated.
    /// </summary>
    public void LogoutAsync()
    {
        CurrentUser = null;
        IsAuthenticated = false;
    }
}
