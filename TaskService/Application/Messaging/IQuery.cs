using MediatR;
using TaskService.Domain.Shared;

namespace TaskService.Application.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
