using Balise.Models;
using NetTopologySuite.Geometries;

/// <summary>
/// Represents a geographical point of interest.
/// </summary>
public class PointOfInterest
{
    /// <summary>
    /// Unique identifier of the point of interest.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the point of interest.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the point of interest.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Geographical position of the point of interest.
    /// </summary>
    public Point Position { get; set; } = null!;
    public Space Space { get; set; }

    /// <summary>
    /// Initializes a new empty instance of <see cref="PointOfInterest"/>.
    /// </summary>
    public PointOfInterest()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="PointOfInterest"/> with specified values.
    /// </summary>
    /// <param name="name">Name of the point of interest.</param>
    /// <param name="description">Description of the point of interest.</param>
    /// <param name="position">Geographical position of the point of interest.</param>
    public PointOfInterest(string name, string description, Point position)
    {
        Name = name;
        Description = description;
        Position = position;
    }
}
