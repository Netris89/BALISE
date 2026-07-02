using System.Collections.Generic;
using System.Threading.Tasks;
using Balise.Data;
using Balise.Models;
using Microsoft.EntityFrameworkCore;

namespace Balise.Repositories;

public class AgendaRepository
{
    private readonly AppDbContext context;

    public AgendaRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task AddAsync(Event evt)
    {
        await context.Events.AddAsync(evt);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Event evt)
    {
        var existingEvent = await context.Events.FindAsync(evt.Id);

        if (existingEvent != null)
        {
            context.Entry(existingEvent).CurrentValues.SetValues(evt);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var evt = await context.Events.FindAsync(id);

        if (evt != null)
        {
            context.Events.Remove(evt);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Event?> GetAsync(int id)
    {
        return await context.Events.FindAsync(id);
    }

    public async Task<List<Event>> GetAllAsync()
    {
        return await context.Events.ToListAsync();
    }
}
