using Avalonia;

namespace Museum.Models;

public class PointOfInterest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Point Position { get; set; }

    public PointOfInterest()
    {
    }

    public PointOfInterest(string name, string description, Point position)
    {
        Name = name;
        Description = description;
        Position = position;
    }
}
