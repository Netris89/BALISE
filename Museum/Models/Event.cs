using System;
using System.ComponentModel.DataAnnotations.Schema;
using Avalonia.Media.Imaging;

namespace Balise.Models;

public class Event
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public DateOnly? Date { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? Link { get; set; }
    [NotMapped]
    public Bitmap? Image { get; set; }
    public /* required */ Museum Museum { get; set; }

    public Event()
    {
    }
}
