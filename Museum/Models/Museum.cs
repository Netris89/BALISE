using System.Collections.Generic;

namespace Balise.Models;

/// <summary>
/// Represents a museum containing multiple floors.
/// </summary>
public class Museum
{
    /// <summary>
    /// Unique identifier of the museum.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the museum.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Collection of floors belonging to the museum.
    /// </summary>
    public List<Floor> Floors { get; set; } = [];

    /// <summary>
    /// Initializes a new empty instance of <see cref="Museum"/>.
    /// </summary>
    public Museum()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Museum"/> with specified values.
    /// </summary>
    /// <param name="name">Name of the museum.</param>
    /// <param name="floors">List of floors in the museum.</param>
    public Museum(string name, List<Floor> floors)
    {
        Name = name;
        Floors = floors;
    }
}
