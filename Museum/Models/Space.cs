using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Balise.Models;

public class Space
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<PointOfInterest> PointOfInterests { get; set; } = [];
    public Polygon Boundary { get; set; } = null!;

    public Space()
    {
    }

    public Space(string name, List<PointOfInterest> poi, Polygon boundary)
    {
        Name = name;
        PointOfInterests = poi;
        Boundary = boundary;
    }
}
