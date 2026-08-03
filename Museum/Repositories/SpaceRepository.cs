using System.Collections.Generic;
using System.Threading.Tasks;
using Balise.Data;
using Balise.Models;
using Microsoft.EntityFrameworkCore;

namespace Balise.Repositories;

/// <summary>
/// Provides CRUD access to <see cref="Space"/> entities.
/// </summary>
public class SpaceRepository
{
    private readonly AppDbContext context;

    /// <summary>
    /// Initializes a new instance of <see cref="SpaceRepository"/>.
    /// </summary>
    /// <param name="context">The database context to operate on.</param>
    public SpaceRepository(AppDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Adds a new space to the database.
    /// </summary>
    /// <param name="space">The space to add.</param>
    public async Task AddAsync(Space space)
    {
        await context.Spaces.AddAsync(space);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing space with new values, if it exists.
    /// </summary>
    /// <param name="space">The space containing the updated values, matched by Id.</param>
    public async Task UpdateAsync(Space space)
    {
        var existingSpace = await context.Spaces.FindAsync(space.Id);

        if (existingSpace != null)
        {
            context.Entry(existingSpace).CurrentValues.SetValues(space);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Deletes the space with the given id, if it exists.
    /// </summary>
    /// <param name="id">The id of the space to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var space = await context.Spaces.FindAsync(id);

        if (space != null)
        {
            context.Spaces.Remove(space);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Retrieves a single space by its id.
    /// </summary>
    /// <param name="id">The id of the space to retrieve.</param>
    /// <returns>The matching space, or <c>null</c> if none is found.</returns>
    public async Task<Space?> GetByIdAsync(int id)
    {
        return await context.Spaces.FindAsync(id);
    }

    /// <summary>
    /// Retrieves all spaces in the database.
    /// </summary>
    public async Task<List<Space>> GetAllAsync()
    {
        return await context.Spaces.ToListAsync();
    }
}
