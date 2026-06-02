using System;
using Avalonia.Controls;

namespace Balise.Views;

public partial class MapView : UserControl
{
    public MapView()
    {
        InitializeComponent();
    }

    private void PolygonPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        Console.WriteLine("clic");
    }
}