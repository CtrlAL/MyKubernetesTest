using AutoMapper;
using GraphService.Application.Dto;
using GraphService.Application.Interfaces;
using GraphService.Application.Messaging;
using GraphService.Domain.Entities;
using GraphService.Domain.Shared;

namespace GraphService.Application.Commands.CreateNode
{
    public class CreateNodeCommandHandler : ICommandHandler<CreateNodeCommand, Result<NodeCreatedDto>>
    {
        private readonly IGraphRepository _repository;
        private readonly IMapper _mapper;

        public CreateNodeCommandHandler(IGraphRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<NodeCreatedDto>> Handle(CreateNodeCommand request, CancellationToken cancellationToken)
        {
            var node = new Node { Name = request.Name };

            _repository.CreateNode(node);
            await _repository.SaveChangesAsync();

            var dto = _mapper.Map<NodeCreatedDto>(node);
            return Result<NodeCreatedDto>.Success(dto);
        }
    }
}
