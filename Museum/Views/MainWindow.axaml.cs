using ReactiveUI.Avalonia;
using Balise.ViewModels;
using Avalonia.Interactivity;

namespace Balise.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent(); 
        AddHandler(PointerMovedEvent, OnUserActivity, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
        AddHandler(PointerPressedEvent, OnUserActivity, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
        AddHandler(KeyDownEvent, OnUserActivity, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
    }

    private void OnUserActivity(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.RegisterActivity();
        }
    }
}