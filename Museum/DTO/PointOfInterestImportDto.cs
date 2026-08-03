namespace Balise.DTO;

/// <summary>
/// Raw data for a point of interest read from an import file, before being
/// resolved into a real <see cref="PointOfInterest"/> entity.
/// </summary>
public class PointOfInterestImportDto
{
    /// <summary>
    /// The id of the point of interest to update, or <c>0</c>/absent when it should be created.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the point of interest.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The description of the point of interest.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The location of the point of interest within its space.
    /// </summary>
    public PointDto Location { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="PointOfInterestImportDto"/>.
    /// </summary>
    /// <param name="id">The id of the point of interest to update, or <c>0</c> when creating a new one.</param>
    /// <param name="name">The name of the point of interest.</param>
    /// <param name="description">The description of the point of interest.</param>
    /// <param name="location">The location of the point of interest within its space.</param>
    public PointOfInterestImportDto(int id, string name, string description, PointDto location)
    {
        Id = id;
        Name = name;
        Description = description;
        Location = location;
    }
}
