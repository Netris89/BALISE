using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using Balise.Repositories;
using Balise.Services;
using NetTopologySuite.Geometries;
using ReactiveUI;

namespace Balise.ViewModels;

/// <summary>
/// Root view model that holds the currently displayed view and exposes the
/// navigation commands between the app's screens.
/// </summary>
public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase currentView;
    private FormService formServ;
    private AuthenticationService authServ;
    private readonly ObservableAsPropertyHelper<bool> isAdmin;
    private readonly ObservableAsPropertyHelper<bool> isContentManager;
    private readonly ObservableAsPropertyHelper<bool> isConnected;
    private readonly PointOfInterestRepository pointOfInterestRepository;
    private readonly SpaceRepository spaceRepository;
    private readonly UserRepository userRepository;
    private readonly DispatcherTimer activityTimeOut = new DispatcherTimer() { Interval = TimeSpan.FromMinutes(5) };

    /// <summary>
    /// The view model currently displayed in the main window.
    /// </summary>
    public ViewModelBase CurrentView
    {
        get => currentView;
        set
        {
            this.RaiseAndSetIfChanged(ref currentView, value);
            this.RaisePropertyChanged(nameof(IsLangChoiceActive));

            if (value is LangChoiceViewModel)
            {
                activityTimeOut.Stop();
            }
            else
            {
                activityTimeOut.Stop();
                activityTimeOut.Start();
            }
        }
    }

    /// <summary>
    /// Indicates whether the language selection screen is currently active.
    /// </summary>
    public bool IsLangChoiceActive
    {
        get => currentView is LangChoiceViewModel;
    }

    public bool IsAdmin => isAdmin.Value;

    public bool IsContentManager => isContentManager.Value;

    public bool IsConnected => isConnected.Value;

    /// <summary>
    /// Command that navigates to the map view.
    /// </summary>
    public ReactiveCommand<Unit, ViewModelBase> GoToMap { get; }

    /// <summary>
    /// Command that navigates to the agenda view.
    /// </summary>
    public ReactiveCommand<Unit, ViewModelBase> GoToAgenda { get; }

    /// <summary>
    /// Command that navigates to the connection (login) view.
    /// </summary>
    public ReactiveCommand<Unit, ViewModelBase> GoToConnection { get; }

    /// <summary>
    /// Command that navigates to the registration form view.
    /// </summary>
    public ReactiveCommand<Unit, ViewModelBase> GoToForm { get; }

    /// <summary>
    /// Command that navigates to the content management view.
    /// </summary>
    public ReactiveCommand<Unit, ViewModelBase> GoToContent { get; }

    /// <summary>
    /// Command that navigates to the administration view.
    /// </summary>
    public ReactiveCommand<Unit, ViewModelBase> GoToAdmin { get; }

    public ICommand LogOutCommand { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="MainWindowViewModel"/>, starting
    /// on the language selection screen and wiring up the navigation commands.
    /// </summary>
    /// <param name="formService">The service used by the form view model.</param>
    /// <param name="museumRepository">The repository used by the map and language selection view models.</param>
    /// <param name="agendaRepository">The repository used by the agenda view model.</param>
    /// <param name="auth">The authentication service used by the connection view model.</param>
    /// <param name="userRepository">The repository used to look up users during login.</param>
    public MainWindowViewModel(FormService formService, MuseumRepository museumRepository, AgendaRepository agendaRepository, AuthenticationService auth, UserRepository uRepository, PointOfInterestRepository poiRepository, SpaceRepository sRepository, Browser brwser, GeometryFactory fctory, FloorRepository fRepository)
    {
        formServ = formService;
        authServ = auth;
        pointOfInterestRepository = poiRepository;
        spaceRepository = sRepository;
        userRepository = uRepository;

        isAdmin = authServ.WhenAnyValue(
                x => x.IsAuthenticated,
                x => x.CurrentUser,
                (auth, user) => auth && user?.Role == Enum.UserRole.Admin)
            .ToProperty(this, x => x.IsAdmin);

        isContentManager = authServ.WhenAnyValue(
                x => x.IsAuthenticated,
                x => x.CurrentUser,
                (auth, user) => auth && user?.Role == Enum.UserRole.ContentManager || user?.Role == Enum.UserRole.Admin)
            .ToProperty(this, x => x.IsContentManager);

        isConnected = authServ.WhenAnyValue(x => x.IsAuthenticated).ToProperty(this, x => x.IsConnected);

        // At application startup, the language selection screen is displayed by default
        currentView = new LangChoiceViewModel(SetCurrentView, museumRepository);

        activityTimeOut.Tick += (_,_) => CurrentView = new LangChoiceViewModel(SetCurrentView, museumRepository);

        // Command to change the current view
        GoToMap = ReactiveCommand.Create(() => CurrentView = new MapViewModel(museumRepository));
        GoToAgenda = ReactiveCommand.Create(() => CurrentView = new AgendaViewModel(agendaRepository));
        GoToConnection = ReactiveCommand.Create(() => CurrentView = new ConnectionViewModel(authServ, brwser, SetCurrentView));
        GoToForm = ReactiveCommand.Create(() => CurrentView = new FormViewModel(formServ));
        GoToContent = ReactiveCommand.Create(() => CurrentView = new ContentManagerViewModel(museumRepository, fRepository, pointOfInterestRepository, spaceRepository, fctory));
        GoToAdmin = ReactiveCommand.Create(() => CurrentView = new AdminViewModel(userRepository));
        LogOutCommand = ReactiveCommand.Create(LogOutAsync);
    }

    /// <summary>
    /// Sets the currently displayed view model.
    /// </summary>
    /// <param name="vm">The view model to display.</param>
    public void SetCurrentView(ViewModelBase vm)
    {
        CurrentView = vm;
    }

    public async Task LogOutAsync()
    {
        await authServ.LogoutAsync();
    }

    public void RegisterActivity()
    {
        if (activityTimeOut.IsEnabled)
        {
            activityTimeOut.Stop();
            activityTimeOut.Start();
        }
    }
}
