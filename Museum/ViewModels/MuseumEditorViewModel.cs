using System.Threading.Tasks;
using System.Windows.Input;
using Balise.Models;
using Balise.Repositories;
using ReactiveUI;

namespace Balise.ViewModels;

/// <summary>
/// ViewModel responsible for editing museum information.
/// </summary>
public class MuseumEditorViewModel : ViewModelBase
{
    private string name = string.Empty;
    private readonly Museum selectedMuseum;
    private readonly MuseumRepository repository;

    /// <summary>
    /// Command used to save the current museum changes.
    /// </summary>
    public ICommand SaveCommand { get; }

    /// <summary>
    /// Name of the museum being edited.
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
    /// Initializes a new instance of the <see cref="MuseumEditorViewModel"/> class.
    /// </summary>
    /// <param name="museum">Museum to edit.</param>
    /// <param name="museumRepository">Repository used to persist museum changes.</param>
    public MuseumEditorViewModel(Museum museum, MuseumRepository museumRepository)
    {
        selectedMuseum = museum;

        Name = selectedMuseum.Name;

        repository = museumRepository;

        SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync);
    }

    public async Task SaveAsync()
    {
        selectedMuseum.Name = Name;

        await repository.UpdateAsync(selectedMuseum);
    }
}
