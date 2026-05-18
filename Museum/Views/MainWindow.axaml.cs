using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Museum.ViewModels;
using ReactiveUI;

namespace Museum.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            // Bind les commmades de navigation
            this.BindCommand(ViewModel, vm => vm.GoToMap, v => v.GoToMapButton).DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.GoToAgenda, v => v.GoToAgendaButton).DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.GoToConnection, v => v.GoToConnectionButton).DisposeWith(disposables);

            // Bind CurrentView au ContentControl afin d'afficher la page dans la MainWindow
            this.Bind(ViewModel, vm => vm.CurrentView, v => v.ContentControl.Content);
        });
    }
}