using System.Collections.ObjectModel;
using System.Net.Http;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Balise.Models;
using Balise.Repositories;
using Balise.Services;

namespace Balise.ViewModels;

public class AgendaViewModel : ViewModelBase
{
    private AgendaRepository repository;
    public ObservableCollection<Event> Events { get; set; } = new();

    public AgendaViewModel(AgendaRepository agendaRepository)
    {
        repository = agendaRepository;

        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        var scrapedList = await repository.GetAllAsync();

        foreach (var evt in scrapedList)
        {
            Events.Add(evt);
        }
    }
}
