using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Balise.Data;
using DynamicData.Kernel;
using Microsoft.EntityFrameworkCore;

namespace Balise.Repositories;

public class PointOfInterestRepository
{
    private readonly AppDbContext context;

    public PointOfInterestRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task AddAsync(PointOfInterest pointOfInterest)
    {
        await context.PointOfInterests.AddAsync(pointOfInterest);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PointOfInterest pointOfInterest)
    {
        var existingPoi = await context.PointOfInterests.FindAsync(pointOfInterest.Id);

        if (existingPoi != null)
        {
            context.Entry(existingPoi).CurrentValues.SetValues(pointOfInterest);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var poi = await context.PointOfInterests.FindAsync(id);

        if (poi != null)
        {
            context.PointOfInterests.Remove(poi);
            await context.SaveChangesAsync();
        }
    }

    public async Task<PointOfInterest?> GetAsync(int id)
    {
        return await context.PointOfInterests.FindAsync(id);
    }

    public async Task<List<PointOfInterest>> GetAllAsync()
    {
        return await context.PointOfInterests.ToListAsync();
    }
}
