using MediatR;
using TaskService.Domain.Shared;

namespace TaskService.Application.Messaging
{
    public interface ICommandHendler<TCommand> : IRequestHandler<TCommand, Result>
        where TCommand : ICommand
    {
    }

    public interface ICommandHendler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
        where TCommand : ICommand<TResponse>
    {
    }
}
