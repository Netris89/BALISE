using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Balise.Models;
using Balise.Repositories;

namespace Balise.ViewModels;

/// <summary>
/// Backs the agenda view, exposing the list of events loaded from the repository.
/// </summary>
public class AgendaViewModel : ViewModelBase
{
    private AgendaRepository repository;

    /// <summary>
    /// The events currently loaded and displayed in the agenda.
    /// </summary>
    public ObservableCollection<Event> Events { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of <see cref="AgendaViewModel"/> and starts
    /// loading events asynchronously.
    /// </summary>
    /// <param name="agendaRepository">The repository used to fetch events.</param>
    public AgendaViewModel(AgendaRepository agendaRepository)
    {
        repository = agendaRepository;

        _ = InitializeAsync();
    }

    /// <summary>
    /// Loads all events from the repository into <see cref="Events"/>.
    /// </summary>
    private async Task InitializeAsync()
    {
        var scrapedList = await repository.GetAllAsync();

        foreach (var evt in scrapedList)
        {
            Events.Add(evt);
        }
    }
}
