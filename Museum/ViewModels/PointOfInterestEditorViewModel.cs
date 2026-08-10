using System.Threading.Tasks;
using System.Windows.Input;
using Balise.Models;
using Balise.Repositories;
using ReactiveUI;

namespace Balise.ViewModels;

/// <summary>
/// ViewModel responsible for editing point of interest information.
/// </summary>
public class PointOfInterestEditorViewModel : ViewModelBase
{
    private readonly PointOfInterestRepository repository;
    private readonly PointOfInterest selectedPointOfInterest;
    private string name = string.Empty;
    private string description = string.Empty;

    /// <summary>
    /// Command used to save the current point of interest changes.
    /// </summary>
    public ICommand SaveCommand { get; }

    /// <summary>
    /// Name of the point of interest being edited.
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
    /// Description of the point of interest being edited.
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
    /// Initializes a new instance of the <see cref="PointOfInterestEditorViewModel"/> class.
    /// </summary>
    /// <param name="pointOfInterest">Point of interest to edit.</param>
    /// <param name="pointOfInterestRepository">Repository used to persist point of interest changes.</param>
    public PointOfInterestEditorViewModel(PointOfInterest pointOfInterest, PointOfInterestRepository pointOfInterestRepository)
    {
        selectedPointOfInterest = pointOfInterest;

        Name = selectedPointOfInterest.Name;
        Description = selectedPointOfInterest.Description;

        repository = pointOfInterestRepository;

        SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync);
    }

    /// <summary>
    /// Saves the current point of interest information to the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation.</returns>
    public async Task SaveAsync()
    {
        selectedPointOfInterest.Name = Name;
        selectedPointOfInterest.Description = Description;

        await repository.UpdateAsync(selectedPointOfInterest);
    }
}
