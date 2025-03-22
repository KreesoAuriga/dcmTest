using Microsoft.EntityFrameworkCore;
using static JobApplicationTracker.Server.Data.User;

namespace JobApplicationTracker.Server.Data
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
