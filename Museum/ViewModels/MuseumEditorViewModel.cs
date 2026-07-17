using System.Threading.Tasks;
using System.Windows.Input;
using Balise.Models;
using Balise.Repositories;
using ReactiveUI;

namespace Balise.ViewModels;

public class MuseumEditorViewModel : ViewModelBase
{
    private string name = string.Empty;
    private readonly Museum selectedMuseum;
    private readonly MuseumRepository repository;
    public ICommand SaveCommand { get; }

    public string Name
    {
        get => name;
        set
        {
            this.RaiseAndSetIfChanged(ref name, value);
        }
    }

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
