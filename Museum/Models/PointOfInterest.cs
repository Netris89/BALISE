using System.Diagnostics.CodeAnalysis;
using Balise.DTO;
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

    public string? ImageUrl { get; set; }

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
    public PointOfInterest(string name, string nameDe, string nameEn, string nameNl, string description, string descriptionDe, string descriptionEn, string descriptionNl, Point position, Space space)
    {
        Name = name;
        NameDe = nameDe;
        NameEn = nameEn;
        NameNl = nameNl;
        Description = description;
        DescriptionDe = descriptionDe;
        DescriptionEn = descriptionEn;
        DescriptionNl = descriptionNl;
        Position = position;
        Space = space;
    }

    [SetsRequiredMembers]
    public PointOfInterest(PointOfInterestImportDto dto, Space space, GeometryFactory factory)
    {
        Id = dto.Id;
        Name = dto.Name;
        NameDe = dto.NameDe;
        NameEn = dto.NameEn;
        NameNl = dto.NameNl;
        Description = dto.Description;
        DescriptionDe = dto.DescriptionDe;
        DescriptionEn = dto.DescriptionEn;
        DescriptionNl = dto.DescriptionNl;
        Position = factory.CreatePoint(new Coordinate(dto.Location.X, dto.Location.Y));
        Space = space;
    }
}
