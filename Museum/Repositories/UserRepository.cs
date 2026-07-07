using System.Threading.Tasks;
using Balise.Data;
using Balise.DTO;
using Balise.Models;
using Microsoft.EntityFrameworkCore;

namespace Balise.Repositories;

public class UserRepository
{
    private readonly AppDbContext context;

    public UserRepository(AppDbContext context)
    {
        this.context = context;
    }

    public void AddUser(FormDto dto)
    {
        User user = new User(dto);

        context.Users.Add(user);
        context.SaveChanges();
    }

    public void AddUser(User user)
    {
        context.Users.Add(user);
        context.SaveChanges();
    }

    public async Task<User>? GetByEmailAsync(string email)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user != null)
        {
            return user;
        }

        return null!;
    }
}
