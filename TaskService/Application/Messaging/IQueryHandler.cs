using MediatR;
using TaskService.Domain.Shared;

namespace TaskService.Application.Messaging
{
    public interface IQueryHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
        where TCommand : IQuery<TResponse>
    {
    }
}
