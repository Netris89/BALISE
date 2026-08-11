namespace Balise.Models;

/// <summary>
/// Extension methods for resolving the correct translation of localized content.
/// </summary>
public static class LocalizedContentExtensions
{
    /// <summary>
    /// Returns the name in the given language, falling back to French if the
    /// translation is missing.
    /// </summary>
    public static string GetName(this ILocalizedContent content, string language) => language switch
    {
        "nl" => string.IsNullOrEmpty(content.NameNl) ? content.Name : content.NameNl,
        "en" => string.IsNullOrEmpty(content.NameEn) ? content.Name : content.NameEn,
        "de" => string.IsNullOrEmpty(content.NameDe) ? content.Name : content.NameDe,
        _ => content.Name
    };

    /// <summary>
    /// Returns the description in the given language, falling back to French if
    /// the translation is missing.
    /// </summary>
    public static string GetDescription(this ILocalizedContent content, string language) => language switch
    {
        "nl" => string.IsNullOrEmpty(content.DescriptionNl) ? content.Description : content.DescriptionNl,
        "en" => string.IsNullOrEmpty(content.DescriptionEn) ? content.Description : content.DescriptionEn,
        "de" => string.IsNullOrEmpty(content.DescriptionDe) ? content.Description : content.DescriptionDe,
        _ => content.Description
    };
}
