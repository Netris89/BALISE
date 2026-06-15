using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Balise.Models;
using Balise.Services;

namespace Balise.ViewModels;

public class AgendaViewModel : ViewModelBase
{
    public ObservableCollection<Event> Events { get; } = new();

    public AgendaViewModel()
    {
        _ = TestAsync();
    }

    public async Task TestAsync()
    {
        HttpClient client = new HttpClient();
        EventFetcherService service = new EventFetcherService(client);

        string html = await service.GetAgendaHtmlAsync();
        var events = service.ParseEvents(html);

        Events.Clear();

        foreach (var ev in events)
        {
            Events.Add(ev);
        }
    }
}
