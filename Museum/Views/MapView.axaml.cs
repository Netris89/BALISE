using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Balise.Models;
using Balise.ViewModels;

namespace Balise.Views;

public partial class MapView : UserControl
{
    public MapView()
    {
        InitializeComponent();
    }

    private void PolygonPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (sender is Polygon polygon && DataContext is MapViewModel vm)
        {
            vm.SelectedSpace = polygon.DataContext as Space;
        }
    }
    
    /* private void OnTogglePopup(object sender, RoutedEventArgs e)
    {
        MyPopup.IsOpen = !MyPopup.IsOpen;
    } */
}