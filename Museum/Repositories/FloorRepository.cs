using System.Collections.Generic;
using System.Threading.Tasks;
using Balise.Data;
using Balise.Models;
using Microsoft.EntityFrameworkCore;

namespace Balise.Repositories;

public class FloorRepository
{
    private readonly AppDbContext context;

    public FloorRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task AddAsync(Floor floor)
    {
        await context.Floors.AddAsync(floor);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Floor floor)
    {
        var existingFloor = await context.Floors.FindAsync(floor.Id);

        if (existingFloor != null)
        {
            context.Entry(existingFloor).CurrentValues.SetValues(floor);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var floor = await context.Floors.FindAsync(id);

        if (floor != null)
        {
            context.Floors.Remove(floor);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Floor?> GetAsync(int id)
    {
        return await context.Floors.FindAsync(id);
    }

    public async Task<List<Floor>> GetAllAsync()
    {
        return await context.Floors.ToListAsync();
    }
}
