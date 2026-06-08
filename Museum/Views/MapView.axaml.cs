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

    /// <summary>
    /// Handles the pointer press event on a map polygon and updates the selected space in the ViewModel.
    /// </summary>
    /// <param name="sender">The polygon that was clicked.</param>
    /// <param name="e">Pointer event data.</param>
    private void PolygonPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (sender is Polygon polygon && DataContext is MapViewModel vm)
        {
            vm.SelectedSpace = polygon.DataContext as Space;
        }
    }
}
