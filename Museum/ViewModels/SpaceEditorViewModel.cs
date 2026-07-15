using System;
using System.Threading.Tasks;
using ReactiveUI;

namespace Balise.ViewModels;

public class SpaceEditorViewModel : ViewModelBase
{
    private string? name;
    private string? description;

    public string? Name
    {
        get => name;
        set
        {
            this.RaiseAndSetIfChanged(ref name, value);
        }
    }

    public string? Description
    {
        get => description;
        set
        {
            this.RaiseAndSetIfChanged(ref description, value);
        }
    }

    public async Task SaveAsync()
    {
        // Copy values into the model and save it.

    }
}
