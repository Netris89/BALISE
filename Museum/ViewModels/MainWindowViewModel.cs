using System.Reactive;
using Balise.Repositories;
using Balise.Services;
using ReactiveUI;

namespace Balise.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase currentView;
    private FormService formService;
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

    public MainWindowViewModel(FormService formService, MuseumRepository museumRepository)
    {
        this.formService = formService;

        // Au lancement de l'application, l'écran de sélection des langues est affiché par défaut
        currentView = new LangChoiceViewModel(SetCurrentView, museumRepository);

        // Commande pour changer de vue
        GoToMap = ReactiveCommand.Create(() => CurrentView = new MapViewModel(museumRepository));
        GoToAgenda = ReactiveCommand.Create(() => CurrentView = new AgendaViewModel());
        GoToConnection = ReactiveCommand.Create(() => CurrentView = new ConnectionViewModel());
        GoToForm = ReactiveCommand.Create(()=> CurrentView = new FormViewModel(formService));
    }

    public void SetCurrentView(ViewModelBase vm)
    {
        CurrentView = vm;
    }
}
