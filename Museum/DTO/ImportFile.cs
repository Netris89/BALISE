using System.Collections.Generic;

namespace Balise.DTO;

public class ImportFile
{
    public List<SpaceImportDto> Spaces { get; set; }

    public ImportFile(List<SpaceImportDto> spaces)
    {
        Spaces = spaces;
    }
}
