using GraphService.Application.Messaging;
using GraphService.Domain.Shared;

namespace GraphService.Application.Commands.DeleteNode
{
    public record DeleteNodeCommand(int Id) : ICommand<Result>;
}
