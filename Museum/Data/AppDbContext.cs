using Microsoft.EntityFrameworkCore;
using Balise.Models;

namespace Balise.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Museum> Museums { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Space> Spaces { get; set; }
        public DbSet<PointOfInterest> PointOfInterests { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Museum>()
                        .HasMany(m => m.Floors)
                        .WithOne(f => f.Museum)
                        .HasForeignKey("MuseumId")
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Floor>()
                        .HasMany(f => f.Spaces)
                        .WithOne(s => s.Floor)
                        .HasForeignKey("FloorId")
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Space>()
                        .HasMany(s => s.PointOfInterests)
                        .WithOne(p => p.Space)
                        .HasForeignKey("SpaceId")
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
