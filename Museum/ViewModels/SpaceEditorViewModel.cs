using System.Threading.Tasks;
using System.Windows.Input;
using Balise.Models;
using Balise.Repositories;
using ReactiveUI;

namespace Balise.ViewModels;

/// <summary>
/// ViewModel responsible for editing space information.
/// </summary>
public class SpaceEditorViewModel : ViewModelBase
{
    private string name = string.Empty;
    private string description = string.Empty;
    private readonly Space selectedSpace;
    private readonly SpaceRepository repository;

    /// <summary>
    /// Command used to save the current space changes.
    /// </summary>
    public ICommand SaveCommand { get; }

    /// <summary>
    /// Name of the space being edited.
    /// </summary>
    public string Name
    {
        get => name;
        set
        {
            this.RaiseAndSetIfChanged(ref name, value);
        }
    }

    /// <summary>
    /// Description of the space being edited.
    /// </summary>
    public string Description
    {
        get => description;
        set
        {
            this.RaiseAndSetIfChanged(ref description, value);
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpaceEditorViewModel"/> class.
    /// </summary>
    /// <param name="space">Space to edit.</param>
    /// <param name="spaceRepository">Repository used to persist space changes.</param>
    public SpaceEditorViewModel(Space space, SpaceRepository spaceRepository)
    {
        selectedSpace = space;

        Name = selectedSpace.Name;
        Description = selectedSpace.Description;

        repository = spaceRepository;

        SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync);
    }

    /// <summary>
    /// Saves the current space information to the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation.</returns>
    public async Task SaveAsync()
    {
        selectedSpace.Name = Name;
        selectedSpace.Description = Description;
        
        await repository.UpdateAsync(selectedSpace);
    }
}
