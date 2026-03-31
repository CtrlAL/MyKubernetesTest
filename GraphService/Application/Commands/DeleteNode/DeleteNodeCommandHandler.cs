using GraphService.Application.Interfaces;
using GraphService.Application.Messaging;
using GraphService.Domain.Exceptions;
using GraphService.Domain.Shared;

namespace GraphService.Application.Commands.DeleteNode
{
    public class DeleteNodeCommandHandler : ICommandHandler<DeleteNodeCommand, Result>
    {
        private readonly IGraphRepository _repository;

        public DeleteNodeCommandHandler(IGraphRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(DeleteNodeCommand request, CancellationToken cancellationToken)
        {
            var node = await _repository.GetNodeById(request.Id)
                ?? throw new NotFoundException("Node", request.Id);

            _repository.DeleteNode(node);
            await _repository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
