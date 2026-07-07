using System.Reactive;
using Balise.Repositories;
using Balise.Services;
using ReactiveUI;

namespace Balise.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase currentView;
    private FormService formServ;
    private AuthenticationService authServ;
    public ViewModelBase CurrentView
    {
        get => currentView;
        set
        {
            this.RaiseAndSetIfChanged(ref currentView, value);
            this.RaisePropertyChanged(nameof(IsLangChoiceActive));
        }
    }

    public bool IsLangChoiceActive
    {
        get => currentView is LangChoiceViewModel;
    }

    public ReactiveCommand<Unit, ViewModelBase> GoToMap { get; }
    public ReactiveCommand<Unit, ViewModelBase> GoToAgenda { get; }
    public ReactiveCommand<Unit, ViewModelBase> GoToConnection { get; }
    public ReactiveCommand<Unit, ViewModelBase> GoToForm { get; }

    public MainWindowViewModel(FormService formService, MuseumRepository museumRepository, AgendaRepository agendaRepository, AuthenticationService auth, UserRepository userRepository)
    {
        formServ = formService;
        authServ = auth;

        // At application startup, the language selection screen is displayed by default
        currentView = new LangChoiceViewModel(SetCurrentView, museumRepository);

        // Command to change the current view
        GoToMap = ReactiveCommand.Create(() => CurrentView = new MapViewModel(museumRepository));
        GoToAgenda = ReactiveCommand.Create(() => CurrentView = new AgendaViewModel(agendaRepository));
        GoToConnection = ReactiveCommand.Create(() => CurrentView = new ConnectionViewModel(authServ/* , userRepository */));
        GoToForm = ReactiveCommand.Create(()=> CurrentView = new FormViewModel(formServ));
    }

    public void SetCurrentView(ViewModelBase vm)
    {
        CurrentView = vm;
    }
}
