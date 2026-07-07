using System;
using System.ComponentModel.DataAnnotations.Schema;
using Avalonia.Media.Imaging;

namespace Balise.Models;

/// <summary>
/// Represents an event scraped or created for a museum's agenda.
/// </summary>
public class Event
{
    /// <summary>
    /// The primary key of the event.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The title of the event.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The date on which the event takes place.
    /// </summary>
    public DateOnly? Date { get; set; }

    /// <summary>
    /// A textual description of the event.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The URL of the event's image, used to load or cache the <see cref="Image"/>.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// A link to the source page.
    /// </summary>
    public string? Link { get; set; }

    /// <summary>
    /// The loaded bitmap for the event's image. Not persisted to the database.
    /// </summary>
    [NotMapped]
    public Bitmap? Image { get; set; }

    /// <summary>
    /// The museum this event belongs to.
    /// </summary>
    public required Museum Museum { get; set; }

    /// <summary>
    /// Initializes a new, empty instance of <see cref="Event"/>.
    /// </summary>
    public Event()
    {
    }
}
