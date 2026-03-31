using MediatR;

namespace GraphService.Application.Messaging
{
    public interface IQuery<out TResponse> : IRequest<TResponse>;
}
