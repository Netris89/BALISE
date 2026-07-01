using System.Linq;
using System.Threading.Tasks;
using Balise.Data;
using Balise.Models;
using DynamicData.Kernel;

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

    public Museum Get()
    {
        var museums = context.Museums.AsList();

        // TODO : make it so it returns all museums
        return museums.ElementAt(0);
    }
}
