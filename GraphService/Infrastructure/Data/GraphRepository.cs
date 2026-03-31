using GraphService.Application.Interfaces;
using GraphService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GraphService.Infrastructure.Data
{
    public class GraphRepository : IGraphRepository
    {
        private readonly AppDbContext _context;

        public GraphRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreateNode(Node node)
        {
            ArgumentNullException.ThrowIfNull(node);
            _context.Nodes.Add(node);
        }

        public void CreateEdge(Edge edge)
        {
            ArgumentNullException.ThrowIfNull(edge);
            _context.Edges.Add(edge);
        }

        public Task<bool> EdgeExists(int sourceNodeId, int targetNodeId)
        {
            return _context.Edges
                .AnyAsync(e => e.SourceNodeId == sourceNodeId && e.TargetNodeId == targetNodeId);
        }

        public Task<List<Node>> GetAllNodes()
        {
            return _context.Nodes.ToListAsync();
        }

        public Task<Node?> GetNodeById(int id)
        {
            return _context.Nodes.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<Dictionary<int, List<int>>> GetAdjacencyList()
        {
            var edges = await _context.Edges
                .Select(e => new { e.SourceNodeId, e.TargetNodeId })
                .ToListAsync();

            var adjacency = new Dictionary<int, List<int>>();

            foreach (var edge in edges)
            {
                if (!adjacency.ContainsKey(edge.SourceNodeId))
                    adjacency[edge.SourceNodeId] = [];

                adjacency[edge.SourceNodeId].Add(edge.TargetNodeId);
            }

            return adjacency;
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
