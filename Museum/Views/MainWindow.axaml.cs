using Avalonia.ReactiveUI;
using Museum.ViewModels;

namespace Museum.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }
}