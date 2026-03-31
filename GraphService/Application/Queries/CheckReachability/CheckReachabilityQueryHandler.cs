using GraphService.Application.Dto;
using GraphService.Application.Interfaces;
using GraphService.Application.Messaging;
using GraphService.Domain.Exceptions;
using GraphService.Domain.Shared;

namespace GraphService.Application.Queries.CheckReachability
{
    public class CheckReachabilityQueryHandler
        : IQueryHandler<CheckReachabilityQuery, Result<ReachabilityResultDto>>
    {
        private readonly IGraphRepository _repository;

        public CheckReachabilityQueryHandler(IGraphRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<ReachabilityResultDto>> Handle(
            CheckReachabilityQuery request,
            CancellationToken cancellationToken)
        {
            var source = await _repository.GetNodeById(request.SourceNodeId)
                ?? throw new NotFoundException("Node", request.SourceNodeId);

            var target = await _repository.GetNodeById(request.TargetNodeId)
                ?? throw new NotFoundException("Node", request.TargetNodeId);

            var adjacency = await _repository.GetAdjacencyList();

            var (reachable, path) = Bfs(adjacency, request.SourceNodeId, request.TargetNodeId);

            var dto = new ReachabilityResultDto
            {
                SourceNodeId = request.SourceNodeId,
                TargetNodeId = request.TargetNodeId,
                IsReachable = reachable,
                Path = path
            };

            return Result<ReachabilityResultDto>.Success(dto);
        }

        private static (bool Reachable, List<int>? Path) Bfs(
            Dictionary<int, List<int>> adjacency,
            int source,
            int target)
        {
            if (source == target)
                return (true, [source]);

            var visited = new HashSet<int>();
            var queue = new Queue<int>();
            var parent = new Dictionary<int, int>();

            visited.Add(source);
            queue.Enqueue(source);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (!adjacency.TryGetValue(current, out var neighbors))
                    continue;

                foreach (var neighbor in neighbors)
                {
                    if (visited.Contains(neighbor))
                        continue;

                    visited.Add(neighbor);
                    parent[neighbor] = current;

                    if (neighbor == target)
                        return (true, ReconstructPath(parent, source, target));

                    queue.Enqueue(neighbor);
                }
            }

            return (false, null);
        }

        private static List<int> ReconstructPath(Dictionary<int, int> parent, int source, int target)
        {
            var path = new List<int>();
            var current = target;

            while (current != source)
            {
                path.Add(current);
                current = parent[current];
            }

            path.Add(source);
            path.Reverse();
            return path;
        }
    }
}
