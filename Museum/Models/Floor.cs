using System;
using System.Collections.Generic;

namespace Museum.Models;

public class Floor
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string MapImageUri { get; set; } = string.Empty;
    public List<Space> Spaces { get; set; } = [];

    public Floor()
    {
    }

    public Floor(int number, string mapImage, List<Space> spaces)
    {
        Number = number;
        MapImageUri = mapImage;
        Spaces = spaces;
    }
}
