using GraphService.Domain.Entities;

namespace GraphService.Application.Interfaces
{
    public interface IGraphRepository
    {
        Task SaveChangesAsync();
        Task<List<Node>> GetAllNodes();
        Task<Node?> GetNodeById(int id);
        void CreateNode(Node node);
        void CreateEdge(Edge edge);
        Task<bool> EdgeExists(int sourceNodeId, int targetNodeId);
        Task<Dictionary<int, List<int>>> GetAdjacencyList();
        Task<(bool IsReachable, List<int>? Path)> CheckReachabilityRecursiveSql(int sourceNodeId, int targetNodeId);
    }
}
