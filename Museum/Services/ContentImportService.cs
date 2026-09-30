using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Balise.DTO;
using Balise.Models;
using Balise.Repositories;
using NetTopologySuite.Geometries;

namespace Balise.Services;

/// <summary>
/// Imports spaces and their points of interest from a JSON file, creating
/// new spaces or updating existing ones depending on whether a matching
/// space is found in the database.
/// </summary>
public class ContentImportService
{
    private readonly SpaceRepository spaceRepository;
    private readonly FloorRepository floorRepository;
    private readonly GeometryFactory factory;

    /// <summary>
    /// Initializes a new instance of <see cref="ContentImportService"/>.
    /// </summary>
    /// <param name="sRepository">The repository used to create or update spaces.</param>
    /// <param name="fRepository">The repository used to resolve the target floor of each space.</param>
    /// <param name="fctory">The geometry factory used to build polygons and points.</param>
    public ContentImportService(SpaceRepository sRepository, FloorRepository fRepository, GeometryFactory fctory)
    {
        spaceRepository = sRepository;
        floorRepository = fRepository;
        factory = fctory;
    }

    /// <summary>
    /// Parses the given JSON content and imports each described space: existing
    /// spaces are updated in place, spaces with no match in the database are
    /// created along with their points of interest. Spaces referencing an
    /// unknown floor are skipped.
    /// </summary>
    /// <param name="jsonContent">The raw JSON content of the import file.</param>
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

                var existingSpace = await spaceRepository.GetByIdAsync(dto.Id);

                if (existingSpace != null)
                {
                    existingSpace.Name = dto.Name;
                    existingSpace.NameDe = dto.NameDe;
                    existingSpace.NameEn = dto.NameEn;
                    existingSpace.NameNl = dto.NameNl;
                    existingSpace.Description = dto.Description;
                    existingSpace.DescriptionDe = dto.DescriptionDe;
                    existingSpace.DescriptionEn = dto.DescriptionEn;
                    existingSpace.DescriptionNl = dto.DescriptionNl;
                    existingSpace.Boundary = boundary;
                    existingSpace.Floor = floor;

                    foreach (var poi in dto.PointsOfInterest)
                    {
                        var existingPoi = existingSpace.PointOfInterests.FirstOrDefault(p => p.Id == poi.Id);

                        if (existingPoi != null)
                        {
                            existingPoi.Name = poi.Name;
                            existingPoi.NameDe = poi.NameDe;
                            existingPoi.NameEn = poi.NameEn;
                            existingPoi.NameNl = poi.NameNl;
                            existingPoi.Description = poi.Description;
                            existingPoi.DescriptionDe = poi.DescriptionDe;
                            existingPoi.DescriptionEn = poi.DescriptionEn;
                            existingPoi.DescriptionNl = poi.DescriptionNl;
                            existingPoi.Position = factory.CreatePoint(new Coordinate(poi.Location.X, poi.Location.Y));
                            existingPoi.Space = existingSpace;
                        }
                        else
                        {
                            existingSpace.PointOfInterests.Add(new PointOfInterest(poi, existingSpace, factory));
                        }
                    }

                    await spaceRepository.UpdateAsync(existingSpace);
                }
                else
                {
                    var newSpace = new Space(dto.Name, dto.NameDe, dto.NameEn, dto.NameNl, dto.Description, dto.DescriptionDe, dto.DescriptionEn, dto.DescriptionNl, new List<PointOfInterest>(), boundary, floor);
                    newSpace.PointOfInterests.AddRange(dto.PointsOfInterest.Select(poi => new PointOfInterest(poi.Name, poi.NameDe, poi.NameEn, poi.NameNl, poi.Description, poi.DescriptionDe, poi.DescriptionEn, poi.DescriptionNl, factory.CreatePoint(new Coordinate(poi.Location.X, poi.Location.Y)), newSpace)));
                    await spaceRepository.AddAsync(newSpace);
                }
            }
        }
    }
}
