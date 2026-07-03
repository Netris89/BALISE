using System.Reactive;
using Auth0.OidcClient;
using Balise.Repositories;
using Balise.Services;
using ReactiveUI;

namespace Balise.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase currentView;
    private FormService formServ;
    private Auth0Client auth0Client;
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

    public MainWindowViewModel(FormService formService, MuseumRepository museumRepository, AgendaRepository agendaRepository, Auth0Client auth)
    {
        formServ = formService;
        auth0Client = auth;

        // Au lancement de l'application, l'écran de sélection des langues est affiché par défaut
        currentView = new LangChoiceViewModel(SetCurrentView, museumRepository);

        // Commande pour changer de vue
        GoToMap = ReactiveCommand.Create(() => CurrentView = new MapViewModel(museumRepository));
        GoToAgenda = ReactiveCommand.Create(() => CurrentView = new AgendaViewModel(agendaRepository));
        GoToConnection = ReactiveCommand.Create(() => CurrentView = new ConnectionViewModel(auth0Client));
        GoToForm = ReactiveCommand.Create(()=> CurrentView = new FormViewModel(formServ));
    }

    public void SetCurrentView(ViewModelBase vm)
    {
        CurrentView = vm;
    }
}
