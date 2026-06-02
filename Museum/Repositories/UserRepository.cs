using Balise.Data;
using Balise.DTO;
using Balise.Models;

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
}
