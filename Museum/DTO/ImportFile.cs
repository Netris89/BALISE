using System.Collections.Generic;

namespace Balise.DTO;

/// <summary>
/// Root structure of a content import file, containing the spaces to create or update.
/// </summary>
public class ImportFile
{
    /// <summary>
    /// The spaces described in the import file.
    /// </summary>
    public List<SpaceImportDto> Spaces { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="ImportFile"/>.
    /// </summary>
    /// <param name="spaces">The spaces described in the import file.</param>
    public ImportFile(List<SpaceImportDto> spaces)
    {
        Spaces = spaces;
    }
}
