using System;
using Avalonia.Media.Imaging;

namespace Balise.Models;

public class Event
{
    public string? Title { get; set; }
    public string? Date { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? Link { get; set; }
    public Bitmap? Image { get; set; }

    public Event()
    {
        if (ImageUrl != null)
        {
            Image = new Bitmap(ImageUrl);
        }
    }
}
