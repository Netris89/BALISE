using System.Threading.Tasks;
using System.Windows.Input;
using Balise.Models;
using Balise.Repositories;
using ReactiveUI;

namespace Balise.ViewModels;

public class PointOfInterestEditorViewModel : ViewModelBase
{
    private readonly PointOfInterestRepository repository;
    private readonly PointOfInterest selectedPointOfInterest;
    private string name = string.Empty;
    private string description = string.Empty;
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

    public PointOfInterestEditorViewModel(PointOfInterest pointOfInterest, PointOfInterestRepository pointOfInterestRepository)
    {
        selectedPointOfInterest = pointOfInterest;

        Name = selectedPointOfInterest.Name;
        Description = selectedPointOfInterest.Description;

        repository = pointOfInterestRepository;

        SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync);
    }

    public async Task SaveAsync()
    {
        selectedPointOfInterest.Name = Name;
        selectedPointOfInterest.Description = Description;

        await repository.UpdateAsync(selectedPointOfInterest);
    }
}
