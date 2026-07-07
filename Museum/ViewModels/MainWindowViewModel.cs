using System.Reactive;
using Balise.Repositories;
using Balise.Services;
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
        }
    }

    /// <summary>
    /// Indicates whether the language selection screen is currently active.
    /// </summary>
    public bool IsLangChoiceActive
    {
        get => currentView is LangChoiceViewModel;
    }

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
    /// Initializes a new instance of <see cref="MainWindowViewModel"/>, starting
    /// on the language selection screen and wiring up the navigation commands.
    /// </summary>
    /// <param name="formService">The service used by the form view model.</param>
    /// <param name="museumRepository">The repository used by the map and language selection view models.</param>
    /// <param name="agendaRepository">The repository used by the agenda view model.</param>
    /// <param name="auth">The authentication service used by the connection view model.</param>
    /// <param name="userRepository">The repository used to look up users during login.</param>
    public MainWindowViewModel(FormService formService, MuseumRepository museumRepository, AgendaRepository agendaRepository, AuthenticationService auth, UserRepository userRepository)
    {
        formServ = formService;
        authServ = auth;

        // At application startup, the language selection screen is displayed by default
        currentView = new LangChoiceViewModel(SetCurrentView, museumRepository);

        // Command to change the current view
        GoToMap = ReactiveCommand.Create(() => CurrentView = new MapViewModel(museumRepository));
        GoToAgenda = ReactiveCommand.Create(() => CurrentView = new AgendaViewModel(agendaRepository));
        GoToConnection = ReactiveCommand.Create(() => CurrentView = new ConnectionViewModel(authServ));
        GoToForm = ReactiveCommand.Create(()=> CurrentView = new FormViewModel(formServ));
    }

    /// <summary>
    /// Sets the currently displayed view model.
    /// </summary>
    /// <param name="vm">The view model to display.</param>
    public void SetCurrentView(ViewModelBase vm)
    {
        CurrentView = vm;
    }
}
