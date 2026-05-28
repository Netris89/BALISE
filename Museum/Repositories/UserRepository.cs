using Museum.Data;
using Museum.DTO;
using Museum.Models;

namespace Museum.Repositories;

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
