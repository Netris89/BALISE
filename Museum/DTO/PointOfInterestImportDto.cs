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
    public string NameDe { get; set; }
    public string NameEn { get; set; }
    public string NameNl { get; set; }

    /// <summary>
    /// The description of the point of interest.
    /// </summary>
    public string Description { get; set; }
    public string DescriptionDe { get; set; }
    public string DescriptionEn { get; set; }
    public string DescriptionNl { get; set; }

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
    public PointOfInterestImportDto(int id, string name, string nameDe, string nameEn, string nameNl, string description, string descriptionDe, string descriptionEn, string descriptionNl, PointDto location)
    {
        Id = id;
        Name = name;
        NameDe = nameDe;
        NameEn = nameEn;
        NameNl = nameNl;
        Description = description;
        DescriptionDe = descriptionDe;
        DescriptionEn = descriptionEn;
        DescriptionNl = descriptionNl;
        Location = location;
    }
}
