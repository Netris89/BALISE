namespace Balise.DTO;

public class PointOfInterestImportDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public PointDto Location { get; set; }

    public PointOfInterestImportDto(string name, string description, PointDto location)
    {
        Name = name;
        Description = description;
        Location = location;
    }
}
