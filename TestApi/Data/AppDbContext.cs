using Microsoft.EntityFrameworkCore;
using TaskService.DomainEvents.Base;
using TaskService.Interceptors;

namespace TaskService.Data
{
    public class AppDbContext : DbContext
    {
        private readonly DomainEventInterceptor _domainEventInterceptor;

        public AppDbContext(DbContextOptions<AppDbContext> options, DomainEventInterceptor domainEventInterceptor) : base(options)
        {
            _domainEventInterceptor = domainEventInterceptor;
        }

        protected AppDbContext()
        {
        }

        public DbSet<Entities.Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Entities.Entity>(e =>
            {
                e.Ignore(e => e.DomainEvents);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_domainEventInterceptor);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
