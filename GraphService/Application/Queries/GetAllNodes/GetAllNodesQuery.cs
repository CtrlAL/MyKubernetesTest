using GraphService.Application.Dto;
using GraphService.Application.Messaging;
using GraphService.Domain.Shared;

namespace GraphService.Application.Queries.GetAllNodes
{
    public record GetAllNodesQuery : IQuery<Result<List<ReadNodeDto>>>;
}
