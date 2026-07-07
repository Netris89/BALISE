namespace Balise.Enum;

/// <summary>
/// The set of roles a user can have within the application.
/// </summary>
public enum UserRole
{
    /// <summary>
    /// A standard user with read-only access to the app's content.
    /// </summary>
    Visitor,

    /// <summary>
    /// A user allowed to create and manage content.
    /// </summary>
    ContentManager,

    /// <summary>
    /// A user with full administrative privileges.
    /// </summary>
    Admin
}
