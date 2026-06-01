using System;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ReactiveUI;

namespace Museum.ViewModels;

public class MapViewModel : ViewModelBase
{
    private Bitmap image;

    public Bitmap Image
    {
        get => image;
        set => this.RaiseAndSetIfChanged(ref image, value);
    }

    public ViewModelActivator Activator => new ViewModelActivator();

    public MapViewModel()
    {
        var uri = new Uri("avares://Museum/Assets/REZ_GC.png");

        image = new(AssetLoader.Open(uri));
    }
}
