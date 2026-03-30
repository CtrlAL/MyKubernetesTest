using MediatR;
using TaskService.Domain.Shared;

namespace TaskService.Application.Messaging
{
    public interface ICommand : IRequest<Result>
    {
    }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
