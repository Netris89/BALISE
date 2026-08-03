using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Balise.DTO;
using Balise.Models;
using Balise.Repositories;
using NetTopologySuite.Geometries;

namespace Balise.Services;

public class ContentImportService
{
    private readonly SpaceRepository spaceRepository;
    private readonly FloorRepository floorRepository;
    private readonly GeometryFactory factory;

    public ContentImportService(SpaceRepository sRepository, FloorRepository fRepository, GeometryFactory fctory)
    {
        spaceRepository = sRepository;
        floorRepository = fRepository;
        factory = fctory;
    }

    public async Task ImportAsync(string jsonContent)
    {
        if (jsonContent == null)
        {
            return; // TODO: show error message in the UI
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var data = JsonSerializer.Deserialize<ImportFile>(jsonContent, options);

        if (data != null)
        {
            var spaces = data.Spaces;

            foreach (var dto in data.Spaces)
            {
                var floor = await floorRepository.GetByIdAsync(dto.FloorId);

                if (floor == null)
                {
                    continue;
                }

                var coordinates = dto.Polygon.Select(p => new Coordinate(p.X, p.Y)).ToArray();
                var boundary = factory.CreatePolygon(coordinates);

                var space = new Space(dto.Name, dto.Description, new List<PointOfInterest>(), boundary, floor);
                var existingSpace = await spaceRepository.GetByIdAsync(dto.Id);

                if (existingSpace != null)
                {
                    existingSpace.Name = dto.Name;
                    existingSpace.Description = dto.Description;
                    existingSpace.Boundary = boundary;
                    existingSpace.Floor = floor;

                    await spaceRepository.UpdateAsync(existingSpace);
                }
                else
                {
                    var newSpace = new Space(dto.Name, dto.Description, new List<PointOfInterest>(), boundary, floor);
                    newSpace.PointOfInterests.AddRange(dto.PointsOfInterest.Select(poi => new PointOfInterest(poi.Name, poi.Description, factory.CreatePoint(new Coordinate(poi.Location.X, poi.Location.Y)), newSpace)));
                    await spaceRepository.AddAsync(newSpace);
                }
            }
        }
    }
}
