using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Balise.Data;
using Balise.Models;
using Microsoft.EntityFrameworkCore;

namespace Balise.Repositories;

/// <summary>
/// Provides CRUD access to <see cref="Museum"/> entities.
/// </summary>
public class MuseumRepository
{
    private readonly IDbContextFactory<AppDbContext> contextFactory;

    /// <summary>
    /// Initializes a new instance of <see cref="MuseumRepository"/>.
    /// </summary>
    /// <param name="context">The database context to operate on.</param>
    public MuseumRepository(IDbContextFactory<AppDbContext> factory)
    {
        contextFactory = factory;
    }

    /// <summary>
    /// Adds a new museum to the database.
    /// </summary>
    /// <param name="museum">The museum to add.</param>
    public async Task AddAsync(Museum museum)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        await context.Museums.AddAsync(museum);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing museum with new values, if it exists.
    /// </summary>
    /// <param name="museum">The museum containing the updated values, matched by Id.</param>
    public async Task UpdateAsync(Museum museum)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var existingMuseum = await context.Museums.FindAsync(museum.Id);

        if (existingMuseum != null)
        {
            context.Entry(existingMuseum).CurrentValues.SetValues(museum);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Deletes the museum with the given id, if it exists.
    /// </summary>
    /// <param name="id">The id of the museum to delete.</param>
    public async Task DeleteAsync(int id)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var museum = await context.Museums.FindAsync(id);

        if (museum != null)
        {
            context.Museums.Remove(museum);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Retrieves a single museum by its id.
    /// </summary>
    /// <param name="id">The id of the museum to retrieve.</param>
    /// <returns>The matching museum, or <c>null</c> if none is found.</returns>
    public async Task<Museum?> GetAsync(int id)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        return await context.Museums.Include(m => m.Floors)
                                    .ThenInclude(f => f.Spaces)
                                    .ThenInclude(s => s.PointOfInterests)
                                    .FirstOrDefaultAsync(m => m.Id == id);
    }

    /// <summary>
    /// Retrieves all museums in the database.
    /// </summary>
    public async Task<List<Museum>> GetAllAsync()
    {
        using var context = await contextFactory.CreateDbContextAsync();
        return await context.Museums.Include(m => m.Floors)
                                    .ThenInclude(f => f.Spaces)
                                    .ThenInclude(s => s.PointOfInterests)
                                    .ToListAsync();
    }
}
