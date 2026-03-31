using GraphService.Application.Dto;
using GraphService.Application.Messaging;
using GraphService.Domain.Shared;

namespace GraphService.Application.Queries.CheckReachabilityRecursiveSql
{
    public record CheckReachabilityRecursiveSqlQuery(int SourceNodeId, int TargetNodeId)
        : IQuery<Result<ReachabilityResultDto>>;
}
