using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using Museum.ViewModels;
using ReactiveUI;

namespace Museum.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }
}