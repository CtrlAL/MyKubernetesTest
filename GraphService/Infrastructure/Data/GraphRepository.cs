using System.Data;
using GraphService.Application.Interfaces;
using GraphService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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

        public async Task<(bool IsReachable, List<int>? Path)> CheckReachabilityRecursiveSql(
            int sourceNodeId, int targetNodeId)
        {
            if (sourceNodeId == targetNodeId)
                return (true, [sourceNodeId]);

            const string sql = """
                WITH RECURSIVE reachable AS (
                    -- anchor: direct neighbours of the source node
                    SELECT
                        e."TargetNodeId"  AS node_id,
                        ARRAY[e."SourceNodeId", e."TargetNodeId"] AS path
                    FROM "Edges" e
                    WHERE e."SourceNodeId" = @source

                    UNION

                    -- recursive step: follow outgoing edges, preventing cycles
                    SELECT
                        e."TargetNodeId",
                        r.path || e."TargetNodeId"
                    FROM "Edges" e
                    INNER JOIN reachable r ON e."SourceNodeId" = r.node_id
                    WHERE e."TargetNodeId" != ALL(r.path)
                )
                SELECT path
                FROM reachable
                WHERE node_id = @target
                LIMIT 1;
                """;

            var connection = _context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = sql;
            command.Parameters.Add(new NpgsqlParameter("@source", sourceNodeId));
            command.Parameters.Add(new NpgsqlParameter("@target", targetNodeId));

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var pathArray = (int[])reader["path"];
                return (true, pathArray.ToList());
            }

            return (false, null);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
