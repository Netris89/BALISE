using System.Collections.Generic;
using System.Threading.Tasks;
using Balise.Data;
using Balise.Models;
using Microsoft.EntityFrameworkCore;

namespace Balise.Repositories;

/// <summary>
/// Provides CRUD access to <see cref="PointOfInterest"/> entities.
/// </summary>
public class PointOfInterestRepository
{
    private readonly IDbContextFactory<AppDbContext> contextFactory;

    /// <summary>
    /// Initializes a new instance of <see cref="PointOfInterestRepository"/>.
    /// </summary>
    /// <param name="context">The database context to operate on.</param>
    public PointOfInterestRepository(IDbContextFactory<AppDbContext> factory)
    {
        contextFactory = factory;
    }

    /// <summary>
    /// Adds a new point of interest to the database.
    /// </summary>
    /// <param name="pointOfInterest">The point of interest to add.</param>
    public async Task AddAsync(PointOfInterest pointOfInterest)
    {
        using var context = await contextFactory.CreateDbContextAsync(); 

        context.Attach(pointOfInterest.Space);
        
        await context.PointOfInterests.AddAsync(pointOfInterest);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing point of interest with new values, if it exists.
    /// </summary>
    /// <param name="pointOfInterest">The point of interest containing the updated values, matched by Id.</param>
    public async Task UpdateAsync(PointOfInterest pointOfInterest)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var existingPoi = await context.PointOfInterests.FindAsync(pointOfInterest.Id);

        if (existingPoi != null)
        {
            pointOfInterest.ImageUrl ??= existingPoi.ImageUrl;
            context.Entry(existingPoi).CurrentValues.SetValues(pointOfInterest);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Deletes the point of interest with the given id, if it exists.
    /// </summary>
    /// <param name="id">The id of the point of interest to delete.</param>
    public async Task DeleteAsync(int id)
    {
        using var context = await contextFactory.CreateDbContextAsync(); 
        var poi = await context.PointOfInterests.FindAsync(id);

        if (poi != null)
        {
            context.PointOfInterests.Remove(poi);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Retrieves a single point of interest by its id.
    /// </summary>
    /// <param name="id">The id of the point of interest to retrieve.</param>
    /// <returns>The matching point of interest, or <c>null</c> if none is found.</returns>
    public async Task<PointOfInterest?> GetAsync(int id)
    {
        using var context = await contextFactory.CreateDbContextAsync(); 
        return await context.PointOfInterests.FindAsync(id);
    }

    /// <summary>
    /// Retrieves all points of interest in the database.
    /// </summary>
    public async Task<List<PointOfInterest>> GetAllAsync()
    {
        using var context = await contextFactory.CreateDbContextAsync(); 
        return await context.PointOfInterests.ToListAsync();
    }
}
