using Avalonia.ReactiveUI;
using Balise.ViewModels;

namespace Balise.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }
}