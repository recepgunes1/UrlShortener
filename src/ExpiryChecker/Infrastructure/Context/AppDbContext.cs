using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace ExpiryChecker.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Url> Urls { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
