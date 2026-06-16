using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Balise.Models;
using HtmlAgilityPack;

namespace Balise.Services;

public class EventFetcherService
{
    private readonly HttpClient httpClient;
    
    public EventFetcherService(HttpClient client)
    {
        httpClient = client;
    }

    public async Task<string> GetAgendaHtmlAsync()
    {
        string url = "https://www.grandcurtius.be/index.php/fr/actualites/agenda";
        string html = await httpClient.GetStringAsync(url);

        return html;
    }

    public List<Event> ParseEvents(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var nodes = doc.DocumentNode.SelectNodes("//article[contains(@class,'node--type-event')]");

        var events = new List<Event>();

        if (nodes == null)
        {
            return events;
        }

        foreach (var node in nodes)
        {
            var baseUrl = "https://www.grandcurtius.be";
            var titleNode = node.SelectSingleNode(".//h3[contains(@class,'node__title')]//span");
            var dateNode = node.SelectSingleNode(".//time");
            var linkNode = node.SelectSingleNode(".//h3[contains(@class,'node__title')]//a");
            var imgNode = node.SelectSingleNode(".//div[contains(@class,'field--name-field-image')]//img");
            var descNode = node.SelectSingleNode(".//div[contains(@class,'field--name-body')]");

            var dateString = dateNode?.GetAttributeValue("datetime", "");
            DateOnly date = default;
            
            // Convert the ISO date string to a DateOnly value.
            // The source contains a full timestamp, but only the calendar date is relevant.
            if (DateTime.TryParse(dateString, out var parsedDate))
            {
                date = DateOnly.FromDateTime(parsedDate);
            }

            var ev = new Event
            {
                Title = WebUtility.HtmlDecode(titleNode?.InnerText)?.Trim(),
                Date = date,

                Link = linkNode != null ? baseUrl + linkNode.GetAttributeValue("href", "") : null,
                ImageUrl = imgNode != null ? baseUrl + imgNode.GetAttributeValue("src", "") : null,

                Description = WebUtility.HtmlDecode(descNode?.InnerText)?.Trim()
            };

            events.Add(ev);
        }

        return events;
    }
}
