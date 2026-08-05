using System;
using Avalonia.Controls;
using Balise.ViewModels;
using Mapsui;
using Avalonia.Threading;

namespace Balise.Views;

public partial class MapView : UserControl
{
    private bool isMapInitialized;

    public MapView()
    {
        InitializeComponent();

        AttachedToVisualTree += (_, _) =>
        {
            MapControl.Map.Navigator.ViewportChanged += OnViewportChanged;
        };
    }

    private void OnViewportChanged(object sender, ViewportChangedEventArgs e)
    {
        // Ignores the change to the viewport while the map is not initialized
        if (!isMapInitialized)
        {
            isMapInitialized = true;
            return;
        }

        // Mapsui may trigger viewport changes from a background thread.
        // Dispatch to the UI thread before accessing Avalonia properties.
        Dispatcher.UIThread.Post(() =>
        {
            if (DataContext is MapViewModel vm)
            {
                vm.OnMapMoved();
            }
        });
    }
}
