namespace Balise.Models;

/// <summary>
/// Implemented by entities exposing a name and description translated into
/// French, Dutch, English, and German.
/// </summary>
public interface ILocalizedContent
{
    string Name { get; }
    string NameNl { get; }
    string NameEn { get; }
    string NameDe { get; }
    string Description { get; }
    string DescriptionNl { get; }
    string DescriptionEn { get; }
    string DescriptionDe { get; }
}
