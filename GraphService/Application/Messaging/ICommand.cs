using MediatR;

namespace GraphService.Application.Messaging
{
    public interface ICommand<out TResponse> : IRequest<TResponse>;
}
