using System.Collections.Generic;

namespace Balise.Models;

public class Museum
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Floor> Floors { get; set; } = [];

    public Museum()
    {
    }

    public Museum(string name, List<Floor> floors)
    {
        Name = name;
        Floors = floors;
    }
}
