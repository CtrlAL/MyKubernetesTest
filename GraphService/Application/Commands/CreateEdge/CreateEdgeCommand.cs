using GraphService.Application.Dto;
using GraphService.Application.Messaging;
using GraphService.Domain.Shared;

namespace GraphService.Application.Commands.CreateEdge
{
    public record CreateEdgeCommand(int SourceNodeId, int TargetNodeId) : ICommand<Result<EdgeCreatedDto>>;
}
