using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Balise.Data;
using Balise.DTO;
using Balise.Enum;
using Balise.Models;
using Microsoft.EntityFrameworkCore;

namespace Balise.Repositories;

/// <summary>
/// Provides access to <see cref="User"/> entities.
/// </summary>
public class UserRepository
{
    private readonly AppDbContext context;

    /// <summary>
    /// Initializes a new instance of <see cref="UserRepository"/>.
    /// </summary>
    /// <param name="context">The database context to operate on.</param>
    public UserRepository(AppDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Creates a new user from form data and saves it to the database.
    /// </summary>
    /// <param name="dto">The form data to build the user from.</param>
    public void AddUser(FormDto dto)
    {
        User user = new User(dto);

        context.Users.Add(user);
        context.SaveChanges();
    }

    /// <summary>
    /// Saves an existing user instance to the database.
    /// </summary>
    /// <param name="user">The user to add.</param>
    public void AddUser(User user)
    {
        context.Users.Add(user);
        context.SaveChanges();
    }

    /// <summary>
    /// Retrieves all users who do not have the <see cref="UserRole.Visitor"/> role.
    /// </summary>
    /// <returns>The list of staff users.</returns>
    public async Task<List<User>> GetStaff()
    {
        return await context.Users.Where(r => r.Role != UserRole.Visitor).ToListAsync();
    }

    /// <summary>
    /// Retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email address to search for.</param>
    /// <returns>The matching user, or <c>null</c> if none is found.</returns>
    public async Task<User>? GetByEmailAsync(string email)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user != null)
        {
            return user;
        }

        return null!;
    }

    /// <summary>
    /// Deletes the user with the specified id, if it exists.
    /// </summary>
    /// <param name="id">The id of the user to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var user = await context.Users.FindAsync(id);

        if (user != null)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Saves changes made to an existing user to the database.
    /// </summary>
    /// <param name="user">The user to update.</param>
    public async Task UpdateAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }
}
