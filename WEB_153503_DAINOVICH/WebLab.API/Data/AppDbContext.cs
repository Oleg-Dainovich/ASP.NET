using Microsoft.EntityFrameworkCore;
using WebLab.Domain.Entities;

namespace WebLab.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Movie> Movie { get; set; } = default!;
        public DbSet<MovieType> MovieType { get; set; } = default!;
    }    
}
