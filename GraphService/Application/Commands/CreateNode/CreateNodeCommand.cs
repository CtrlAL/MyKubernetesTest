using GraphService.Application.Dto;
using GraphService.Application.Messaging;
using GraphService.Domain.Shared;

namespace GraphService.Application.Commands.CreateNode
{
    public record CreateNodeCommand(string Name) : ICommand<Result<NodeCreatedDto>>;
}
