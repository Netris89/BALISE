using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Balise.Data;
using Balise.Models;
using DynamicData.Kernel;
using Microsoft.EntityFrameworkCore;

namespace Balise.Repositories;

public class MuseumRepository
{
    private readonly AppDbContext context;

    public MuseumRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task AddAsync(Museum museum)
    {
        await context.Museums.AddAsync(museum);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Museum museum)
    {
        var existingMuseum = await context.Museums.FindAsync(museum.Id);

        if (existingMuseum != null)
        {
            context.Entry(existingMuseum).CurrentValues.SetValues(museum);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var museum = await context.Museums.FindAsync(id);

        if (museum != null)
        {
            context.Museums.Remove(museum);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Museum?> GetAsync(int id)
    {
        return await context.Museums.FindAsync(id);
    }

    public async Task<List<Museum>> GetAllAsync()
    {
        return await context.Museums.ToListAsync();
    }
}
