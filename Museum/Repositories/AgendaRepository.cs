using System.Collections.Generic;
using System.Threading.Tasks;
using Balise.Data;
using Balise.Models;
using Microsoft.EntityFrameworkCore;

namespace Balise.Repositories;

/// <summary>
/// Provides CRUD access to <see cref="Event"/> entities.
/// </summary>
public class AgendaRepository
{
    private readonly AppDbContext context;

    /// <summary>
    /// Initializes a new instance of <see cref="AgendaRepository"/>.
    /// </summary>
    /// <param name="context">The database context to operate on.</param>
    public AgendaRepository(AppDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Adds a new event to the database.
    /// </summary>
    /// <param name="evt">The event to add.</param>
    public async Task AddAsync(Event evt)
    {
        await context.Events.AddAsync(evt);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing event with new values, if it exists.
    /// </summary>
    /// <param name="evt">The event containing the updated values, matched by Id.</param>
    public async Task UpdateAsync(Event evt)
    {
        var existingEvent = await context.Events.FindAsync(evt.Id);

        if (existingEvent != null)
        {
            context.Entry(existingEvent).CurrentValues.SetValues(evt);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Deletes the event with the given id, if it exists.
    /// </summary>
    /// <param name="id">The id of the event to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var evt = await context.Events.FindAsync(id);

        if (evt != null)
        {
            context.Events.Remove(evt);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Retrieves a single event by its id.
    /// </summary>
    /// <param name="id">The id of the event to retrieve.</param>
    /// <returns>The matching event, or <c>null</c> if none is found.</returns>
    public async Task<Event?> GetAsync(int id)
    {
        return await context.Events.FindAsync(id);
    }

    /// <summary>
    /// Retrieves all events in the database.
    /// </summary>
    public async Task<List<Event>> GetAllAsync()
    {
        return await context.Events.ToListAsync();
    }
}
