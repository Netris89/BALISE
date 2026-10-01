using System.Collections.Generic;
using System.Linq;
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
    private readonly IDbContextFactory<AppDbContext> contextFactory;

    /// <summary>
    /// Initializes a new instance of <see cref="SpaceRepository"/>.
    /// </summary>
    /// <param name="factory">The database context to operate on.</param>
    public SpaceRepository(IDbContextFactory<AppDbContext> factory)
    {
        contextFactory = factory;
    }

    /// <summary>
    /// Adds a new space to the database.
    /// </summary>
    /// <param name="space">The space to add.</param>
    public async Task AddAsync(Space space)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        context.Attach(space.Floor);

        await context.Spaces.AddAsync(space);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing space with new values, if it exists.
    /// </summary>
    /// <param name="space">The space containing the updated values, matched by Id.</param>
    public async Task UpdateAsync(Space space)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        var existingSpace = await context.Spaces.Include(s => s.PointOfInterests).FirstOrDefaultAsync(s => s.Id == space.Id);

        if (existingSpace == null)
        {
            return;
        }

        context.Entry(existingSpace).CurrentValues.SetValues(space);

        foreach (var poi in space.PointOfInterests)
        {
            var existingPoi = existingSpace.PointOfInterests.FirstOrDefault(p => p.Id == poi.Id);

            if (existingPoi != null)
            {
                context.Entry(existingPoi).CurrentValues.SetValues(poi);
            }
            else
            {
                poi.Space = existingSpace;
                existingSpace.PointOfInterests.Add(poi);
            }
        }

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes the space with the given id, if it exists.
    /// </summary>
    /// <param name="id">The id of the space to delete.</param>
    public async Task DeleteAsync(int id)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        
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
        using var context = await contextFactory.CreateDbContextAsync();
        return await context.Spaces.Include(s => s.PointOfInterests).FirstOrDefaultAsync(s => s.Id == id);
    }

    /// <summary>
    /// Retrieves all spaces in the database.
    /// </summary>
    public async Task<List<Space>> GetAllAsync()
    {
        using var context = await contextFactory.CreateDbContextAsync();
        return await context.Spaces.ToListAsync();
    }
}
