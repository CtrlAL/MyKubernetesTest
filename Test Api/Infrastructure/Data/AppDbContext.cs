using Microsoft.EntityFrameworkCore;
using TaskService.DomainEvents.Base;
using TaskService.Interceptors;
using TaskService.Outbox;

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
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Ignore<List<IDomainEvent>>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_domainEventInterceptor);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
