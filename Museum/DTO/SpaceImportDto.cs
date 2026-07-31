using System.Collections.Generic;

namespace Balise.DTO;

public class SpaceImportDto
{
    public int FloorId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<PointDto> Polygon { get; set; }
    public List<PointOfInterestImportDto> PointsOfInterest { get; set; }
    
    public SpaceImportDto(string name, string description, List<PointDto> polygon, List<PointOfInterestImportDto> pointsOfInterest)
    {
        Name = name;
        Description = description;
        Polygon = polygon;
        PointsOfInterest = pointsOfInterest;
    }
}
