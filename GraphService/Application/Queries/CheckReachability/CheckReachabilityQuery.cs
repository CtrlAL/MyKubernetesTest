using GraphService.Application.Dto;
using GraphService.Application.Messaging;
using GraphService.Domain.Shared;

namespace GraphService.Application.Queries.CheckReachability
{
    public record CheckReachabilityQuery(int SourceNodeId, int TargetNodeId)
        : IQuery<Result<ReachabilityResultDto>>;
}
