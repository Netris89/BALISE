using System.Collections.Generic;
using System.Threading.Tasks;
using Balise.Data;
using Balise.Models;
using Microsoft.EntityFrameworkCore;

namespace Balise.Repositories;

public class SpaceRepository
{
    private readonly AppDbContext context;

    public SpaceRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task AddAsync(Space space)
    {
        await context.Spaces.AddAsync(space);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Space space)
    {
        var existingPoi = await context.Spaces.FindAsync(space.Id);

        if (existingPoi != null)
        {
            context.Entry(existingPoi).CurrentValues.SetValues(space);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var poi = await context.Spaces.FindAsync(id);

        if (poi != null)
        {
            context.Spaces.Remove(poi);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Space?> GetAsync(int id)
    {
        return await context.Spaces.FindAsync(id);
    }

    public async Task<List<Space>> GetAllAsync()
    {
        return await context.Spaces.ToListAsync();
    }
}
