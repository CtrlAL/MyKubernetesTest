using GraphService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GraphService.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Node> Nodes { get; set; }
        public DbSet<Edge> Edges { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Edge>(e =>
            {
                e.HasOne(edge => edge.SourceNode)
                    .WithMany(node => node.OutgoingEdges)
                    .HasForeignKey(edge => edge.SourceNodeId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(edge => edge.TargetNode)
                    .WithMany(node => node.IncomingEdges)
                    .HasForeignKey(edge => edge.TargetNodeId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(edge => new { edge.SourceNodeId, edge.TargetNodeId })
                    .IsUnique();
            });
        }
    }
}
