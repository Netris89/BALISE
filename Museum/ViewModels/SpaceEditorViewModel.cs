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
    private string nameDe = string.Empty;
    private string nameEn = string.Empty;
    private string nameNl = string.Empty;
    private string description = string.Empty;
    private string descriptionDe = string.Empty;
    private string descriptionEn = string.Empty;
    private string descriptionNl = string.Empty;
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

    public string NameDe
    {
        get => nameDe;
        set
        {
            this.RaiseAndSetIfChanged(ref nameDe, value);
        }
    }

    public string NameEn
    {
        get => nameEn;
        set
        {
            this.RaiseAndSetIfChanged(ref nameEn, value);
        }
    }

    public string NameNl
    {
        get => nameNl;
        set
        {
            this.RaiseAndSetIfChanged(ref nameNl, value);
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

    public string DescriptionDe
    {
        get => descriptionDe;
        set
        {
            this.RaiseAndSetIfChanged(ref descriptionDe, value);
        }
    }

    public string DescriptionEn
    {
        get => descriptionEn;
        set
        {
            this.RaiseAndSetIfChanged(ref descriptionEn, value);
        }
    }

    public string DescriptionNl
    {
        get => descriptionNl;
        set
        {
            this.RaiseAndSetIfChanged(ref descriptionNl, value);
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
        NameDe = selectedSpace.NameDe;
        NameEn = selectedSpace.NameEn;
        NameNl = selectedSpace.NameNl;
        Description = selectedSpace.Description;
        DescriptionDe = selectedSpace.DescriptionDe;
        DescriptionEn = selectedSpace.DescriptionEn;
        DescriptionNl = selectedSpace.DescriptionNl;

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
        selectedSpace.NameDe = NameDe;
        selectedSpace.NameEn = NameEn;
        selectedSpace.NameNl = NameNl;
        selectedSpace.Description = Description;
        selectedSpace.DescriptionDe = DescriptionDe;
        selectedSpace.DescriptionEn = DescriptionEn;
        selectedSpace.DescriptionNl = DescriptionNl;
        
        await repository.UpdateAsync(selectedSpace);
    }
}
