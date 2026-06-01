using System.Collections.Generic;
using Avalonia;

namespace Museum.Models;

public class Space
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<PointOfInterest> PointOfInterests { get; set; } = [];
    public List<Point> Boundary { get; set; } = [];

    public Space()
    {
    }

    public Space(string name, List<PointOfInterest> poi, List<Point> boundary)
    {
        Name = name;
        PointOfInterests = poi;
        Boundary = boundary;
    }
}
