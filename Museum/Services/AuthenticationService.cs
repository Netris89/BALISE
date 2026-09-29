using System.Threading.Tasks;
using Balise.Models;
using Balise.Repositories;
using Duende.IdentityModel;
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
    private string? identityToken;

    /// <summary>
    /// The access token from the last login. Not currently used anywhere in the
    /// app — kept for if/when calls to a secured API are added, so it doesn't need
    /// to be threaded through the login flow again at that point.
    /// </summary>
    private string? accessToken;

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
    /// <returns>The result of the login attempt, including whether it failed.</returns>
    public async Task<LoginResult> LoginAsync()
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
            return result;
        }

        identityToken = result.IdentityToken;
        
        string? email = result.User.FindFirst(JwtClaimTypes.Email)?.Value;
        string? emailVerifiedClaim = result.User.FindFirst(JwtClaimTypes.EmailVerified)?.Value;

        bool emailVerified = bool.TryParse(emailVerifiedClaim, out bool verified) && verified;

        if (email != null && emailVerified)
        {
            CurrentUser = await userRepository.GetByEmailAsync(email);
            accessToken = result.TokenResponse.AccessToken!;
        }

        if (CurrentUser != null)
        {
            IsAuthenticated = true;
        }

        return result;
    }

    /// <summary>
    /// Ends the current session with the identity provider, then clears the
    /// current user and marks the session as unauthenticated.
    /// </summary>
    public async Task LogoutAsync()
    {
        await oidcClient.LogoutAsync(new LogoutRequest { IdTokenHint = identityToken });
        CurrentUser = null;
        IsAuthenticated = false;
    }
}
