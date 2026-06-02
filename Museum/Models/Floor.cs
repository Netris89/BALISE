using System.Collections.Generic;

namespace Balise.Models;

/// <summary>
/// Represents a building floor containing spaces and a map representation.
/// </summary>
public class Floor
{
    /// <summary>
    /// Unique identifier of the floor.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Floor number within the building.
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// URI of the map image representing the floor layout.
    /// </summary>
    public string MapImageUri { get; set; } = string.Empty;

    /// <summary>
    /// Collection of spaces located on the floor.
    /// </summary>
    public List<Space> Spaces { get; set; } = [];

    /// <summary>
    /// Initializes a new empty instance of <see cref="Floor"/>.
    /// </summary>
    public Floor()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Floor"/> with specified values.
    /// </summary>
    /// <param name="number">Floor number within the building.</param>
    /// <param name="mapImage">URI of the map image representing the floor layout.</param>
    /// <param name="spaces">List of spaces located on the floor.</param>
    public Floor(int number, string mapImage, List<Space> spaces)
    {
        Number = number;
        MapImageUri = mapImage;
        Spaces = spaces;
    }
}
