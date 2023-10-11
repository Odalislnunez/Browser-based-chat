using Browser_based_chat.Areas.Identity.Data.Configurations;
using Browser_based_chat.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Browser_based_chat.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new RoomConfiguration());

        SeedRooms(builder);
    }

    public DbSet<Room> rooms { get; set; }

    public void SeedRooms(ModelBuilder builder)
    {
        var rooms = new List<Room> {
            new Room ("Food"),
            new Room ("Video Games"),
            new Room ("Movies")
        };

        builder.Entity<Room>().HasData(rooms);
    }
}

