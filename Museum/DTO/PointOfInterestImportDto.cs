namespace Balise.DTO;

public class PointOfInterestImportDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public PointDto Location { get; set; }

    public PointOfInterestImportDto(int id, string name, string description, PointDto location)
    {
        Id = id;
        Name = name;
        Description = description;
        Location = location;
    }
}
