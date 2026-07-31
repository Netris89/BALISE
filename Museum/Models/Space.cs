using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Balise.DTO;
using NetTopologySuite.Geometries;

namespace Balise.Models;

/// <summary>
/// Represents a physical space defined by a boundary and containing points of interest.
/// </summary>
public class Space
{
    /// <summary>
    /// Unique identifier of the space.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the space.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of a space
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Collection of points of interest contained within the space.
    /// </summary>
    public List<PointOfInterest> PointOfInterests { get; set; } = [];

    /// <summary>
    /// Geographical boundary defining the limits of the space.
    /// </summary>
    public Polygon Boundary { get; set; } = null!;

    public List<Avalonia.Point> AvaloniaPoints
    {
        get => Boundary.ExteriorRing.Coordinates.Select(c => new Avalonia.Point(c.X, c.Y)).ToList();
    }

    public required Floor Floor { get; set; }

    /// <summary>
    /// Initializes a new empty instance of <see cref="Space"/>.
    /// </summary>
    public Space()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Space"/> with specified values.
    /// </summary>
    /// <param name="name">Name of the space.</param>
    /// <param name="poi">List of points of interest contained in the space.</param>
    /// <param name="boundary">Geographical boundary of the space.</param>
    [SetsRequiredMembers]
    public Space(string name, string description, List<PointOfInterest> poi, Polygon boundary, Floor floor)
    {
        Name = name;
        Description = description;
        PointOfInterests = poi;
        Boundary = boundary;
        Floor = floor;
    }
}
