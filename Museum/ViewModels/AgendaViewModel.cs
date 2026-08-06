using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
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
        var client = new HttpClient();

        foreach (var evt in scrapedList)
        {
            if (!string.IsNullOrEmpty(evt.ImageUrl))
            {
                var bytes = await client.GetByteArrayAsync(evt.ImageUrl);
                using var stream = new MemoryStream(bytes);
                evt.Image = new Bitmap(stream);
            }

            Events.Add(evt);
        }
    }
}
