using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Balise.Models;
using Balise.Repositories;
using ReactiveUI;

namespace Balise.ViewModels;

public class SpaceEditorViewModel : ViewModelBase
{
    private string name = string.Empty;
    private string description = string.Empty;
    private readonly Space selectedSpace;
    private readonly SpaceRepository repository;
    public ICommand SaveCommand { get; }

    public string Name
    {
        get => name;
        set
        {
            this.RaiseAndSetIfChanged(ref name, value);
        }
    }

    public string Description
    {
        get => description;
        set
        {
            this.RaiseAndSetIfChanged(ref description, value);
        }
    }

    public SpaceEditorViewModel(Space space, SpaceRepository spaceRepository)
    {
        selectedSpace = space;

        Name = selectedSpace.Name;
        Description = selectedSpace.Description;

        repository = spaceRepository;

        SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync);
    }

    public async Task SaveAsync()
    {
        selectedSpace.Name = Name;
        selectedSpace.Description = Description;
        
        await repository.UpdateAsync(selectedSpace);
    }
}
