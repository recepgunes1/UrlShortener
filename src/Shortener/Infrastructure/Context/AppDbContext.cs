using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Shortener.Infrastructure.Models;

namespace Shortener.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Url> Urls { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
