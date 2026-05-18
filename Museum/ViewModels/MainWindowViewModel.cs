using System.Reactive;
using ReactiveUI;

namespace Museum.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase currentView;

    public ViewModelBase CurrentView
    {
        get => currentView;
        set => this.RaiseAndSetIfChanged(ref currentView, value);
    }

    public ReactiveCommand<Unit, ViewModelBase> GoToMap { get; }
    public ReactiveCommand<Unit, ViewModelBase> GoToAgenda { get; }
    public ReactiveCommand<Unit, ViewModelBase> GoToConnection { get; }


    public MainWindowViewModel()
    {
        currentView = new LangChoiceViewModel();

        GoToMap = ReactiveCommand.Create(() => CurrentView = new MapViewModel());
        GoToAgenda = ReactiveCommand.Create(() => CurrentView = new AgendaViewModel());
        GoToConnection = ReactiveCommand.Create(() => CurrentView = new ConnectionViewModel());
    }
}
