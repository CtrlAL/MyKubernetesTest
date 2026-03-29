using Microsoft.EntityFrameworkCore;

namespace TaskService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected AppDbContext()
        {
        }

        public DbSet<Entities.Task> Tasks { get; set; }
    }
}
