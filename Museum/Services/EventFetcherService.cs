using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Balise.Models;
using HtmlAgilityPack;

namespace Balise.Services;

/// <summary>
/// Retrieves and parses museum events from the web.
/// </summary>
public class EventFetcherService
{
    private readonly HttpClient httpClient;
    
    // Temporary, probably will need a method to determine the museum to which an even belongs
    private Museum museum;

    /// <summary>
    /// Initializes a new instance of <see cref="EventFetcherService"/>.
    /// </summary>
    /// <param name="client">HTTP client used to retrieve remote content.</param>
    public EventFetcherService(HttpClient client, Museum museum)
    {
        httpClient = client;
        this.museum = museum;
    }

    /// <summary>
    /// Downloads the HTML content of the web page.
    /// </summary>
    /// <returns>The raw HTML content of the agenda page.</returns>
    public async Task<string> GetAgendaHtmlAsync()
    {
        string url = "https://www.grandcurtius.be/index.php/fr/actualites/agenda";
        string html = await httpClient.GetStringAsync(url);

        return html;
    }

    /// <summary>
    /// Extracts event information from the agenda page HTML.
    /// </summary>
    /// <param name="html">Raw HTML content of the agenda page.</param>
    /// <returns>A list of parsed events.</returns>
    public async Task<List<Event>> ParseEvents(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Select all event entries from the agenda page.
        var nodes = doc.DocumentNode.SelectNodes("//article[contains(@class,'node--type-event')]");

        var events = new List<Event>();

        if (nodes == null)
        {
            return events;
        }

        foreach (var node in nodes)
        {
            // Base URL used to convert relative links and image paths
            // into absolute URLs.
            var baseUrl = "https://www.grandcurtius.be";

            // Extract the relevant HTML elements for the event.
            var titleNode = node.SelectSingleNode(".//h3[contains(@class,'node__title')]//span");
            var dateNode = node.SelectSingleNode(".//time");
            var linkNode = node.SelectSingleNode(".//h3[contains(@class,'node__title')]//a");
            var imgNode = node.SelectSingleNode(".//div[contains(@class,'field--name-field-image')]//img");
            var descNode = node.SelectSingleNode(".//div[contains(@class,'field--name-body')]");

            // Extract the ISO date from the HTML datetime attribute.
            var dateString = dateNode?.GetAttributeValue("datetime", "");

            DateOnly date = default;

            // Convert the ISO date string to a DateOnly value.
            // The source contains a full timestamp, but only the calendar date is relevant.
            if (DateTime.TryParse(dateString, out var parsedDate))
            {
                date = DateOnly.FromDateTime(parsedDate);
            }

            // Convert the relative image path into an absolute URL.
            string? imageUrl;

            if (imgNode != null)
            {
                imageUrl = (string?)(baseUrl + imgNode.GetAttributeValue("src", ""));
            }
            else
            {
                imageUrl = null;
            }

            Event? ev;
            if (linkNode != null)
            {
                ev = new Event
                {
                    // Decode HTML entities such as &quot; and &amp; before displaying text.
                    Title = WebUtility.HtmlDecode(titleNode?.InnerText)?.Trim(),

                    Date = date,

                    // Convert the relative event link into an absolute URL.
                    Link = baseUrl + linkNode.GetAttributeValue("href", ""),

                    ImageUrl = imageUrl,

                    // Decode HTML entities and remove leading/trailing whitespace.
                    Description = WebUtility.HtmlDecode(descNode?.InnerText)?.Trim(),

                    Museum = museum
                };
            }
            else
            {
                ev = new Event
                {
                    // Decode HTML entities such as &quot; and &amp; before displaying text.
                    Title = WebUtility.HtmlDecode(titleNode?.InnerText)?.Trim(),

                    Date = date,

                    // Convert the relative event link into an absolute URL.
                    Link = null,

                    ImageUrl = imageUrl,

                    // Decode HTML entities and remove leading/trailing whitespace.
                    Description = WebUtility.HtmlDecode(descNode?.InnerText)?.Trim(),

                    Museum = museum
                };
            }

            if (!string.IsNullOrEmpty(imageUrl))
            {
                // Download the image and create an Avalonia bitmap
                // that can be displayed directly by the UI.
                var bytes = await httpClient.GetByteArrayAsync(imageUrl);

                using var stream = new MemoryStream(bytes);

                ev.Image = new Bitmap(stream);
            }

            events.Add(ev);
        }

        return events;
    }
}
