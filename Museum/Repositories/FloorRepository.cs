using System.Collections.Generic;
using System.Threading.Tasks;
using Balise.Data;
using Balise.Models;
using Microsoft.EntityFrameworkCore;

namespace Balise.Repositories;

/// <summary>
/// Provides CRUD access to <see cref="Floor"/> entities.
/// </summary>
public class FloorRepository
{
    private readonly IDbContextFactory<AppDbContext> contextFactory;

    /// <summary>
    /// Initializes a new instance of <see cref="FloorRepository"/>.
    /// </summary>
    /// <param name="context">The database context to operate on.</param>
    public FloorRepository(IDbContextFactory<AppDbContext> factory)
    {
        contextFactory = factory;
    }

    /// <summary>
    /// Adds a new floor to the database.
    /// </summary>
    /// <param name="floor">The floor to add.</param>
    public async Task AddAsync(Floor floor)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        await context.Floors.AddAsync(floor);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing floor with new values, if it exists.
    /// </summary>
    /// <param name="floor">The floor containing the updated values, matched by Id.</param>
    public async Task UpdateAsync(Floor floor)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var existingFloor = await context.Floors.FindAsync(floor.Id);

        if (existingFloor != null)
        {
            context.Entry(existingFloor).CurrentValues.SetValues(floor);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Deletes the floor with the given id, if it exists.
    /// </summary>
    /// <param name="id">The id of the floor to delete.</param>
    public async Task DeleteAsync(int id)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var floor = await context.Floors.FindAsync(id);

        if (floor != null)
        {
            context.Floors.Remove(floor);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Retrieves a single floor by its id.
    /// </summary>
    /// <param name="id">The id of the floor to retrieve.</param>
    /// <returns>The matching floor, or <c>null</c> if none is found.</returns>
    public async Task<Floor?> GetByIdAsync(int id)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        return await context.Floors.FindAsync(id);
    }

    /// <summary>
    /// Retrieves all floors in the database.
    /// </summary>
    public async Task<List<Floor>> GetAllAsync()
    {
        using var context = await contextFactory.CreateDbContextAsync();
        return await context.Floors.ToListAsync();
    }
}
