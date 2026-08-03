using System.Collections.Generic;

namespace Balise.DTO;

/// <summary>
/// Raw data for a space read from an import file, before being resolved
/// into a real <see cref="Space"/> entity.
/// </summary>
public class SpaceImportDto
{
    /// <summary>
    /// The id of the space to update, or <c>0</c>/absent when the space should be created.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The id of the floor this space belongs to.
    /// </summary>
    public int FloorId { get; set; }

    /// <summary>
    /// The name of the space.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The description of the space.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The points forming the boundary polygon of the space.
    /// </summary>
    public List<PointDto> Polygon { get; set; }

    /// <summary>
    /// The points of interest located within this space.
    /// </summary>
    public List<PointOfInterestImportDto> PointsOfInterest { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="SpaceImportDto"/>.
    /// </summary>
    /// <param name="id">The id of the space to update, or <c>0</c> when creating a new one.</param>
    /// <param name="name">The name of the space.</param>
    /// <param name="description">The description of the space.</param>
    /// <param name="polygon">The points forming the boundary polygon of the space.</param>
    /// <param name="pointsOfInterest">The points of interest located within this space.</param>
    public SpaceImportDto(int id, string name, string description, List<PointDto> polygon, List<PointOfInterestImportDto> pointsOfInterest)
    {
        Id = id;
        Name = name;
        Description = description;
        Polygon = polygon;
        PointsOfInterest = pointsOfInterest;
    }
}
