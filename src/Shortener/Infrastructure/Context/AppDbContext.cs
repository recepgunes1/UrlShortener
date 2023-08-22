using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Url> Urls { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
