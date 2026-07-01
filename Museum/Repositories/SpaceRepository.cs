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
        var existingSpace = await context.Spaces.FindAsync(space.Id);

        if (existingSpace != null)
        {
            context.Entry(existingSpace).CurrentValues.SetValues(space);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var space = await context.Spaces.FindAsync(id);

        if (space != null)
        {
            context.Spaces.Remove(space);
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
