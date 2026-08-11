using System.Diagnostics.CodeAnalysis;
using NetTopologySuite.Geometries;

namespace Balise.Models;

/// <summary>
/// Represents a geographical point of interest.
/// </summary>
public class PointOfInterest : ILocalizedContent
{
    /// <summary>
    /// Unique identifier of the point of interest.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the point of interest.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    public string NameNl { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string NameDe { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the point of interest.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    public string DescriptionNl { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string DescriptionDe { get; set; } = string.Empty;

    /// <summary>
    /// Geographical position of the point of interest.
    /// </summary>
    public Point Position { get; set; } = null!;
    public required Space Space { get; set; }

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
    [SetsRequiredMembers]
    public PointOfInterest(string name, string description, Point position, Space space)
    {
        Name = name;
        Description = description;
        Position = position;
        Space = space;
    }
}
