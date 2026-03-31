using AutoMapper;
using GraphService.Application.Dto;
using GraphService.Application.Interfaces;
using GraphService.Application.Messaging;
using GraphService.Domain.Entities;
using GraphService.Domain.Exceptions;
using GraphService.Domain.Shared;

namespace GraphService.Application.Commands.CreateEdge
{
    public class CreateEdgeCommandHandler : ICommandHandler<CreateEdgeCommand, Result<EdgeCreatedDto>>
    {
        private readonly IGraphRepository _repository;
        private readonly IMapper _mapper;

        public CreateEdgeCommandHandler(IGraphRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<EdgeCreatedDto>> Handle(CreateEdgeCommand request, CancellationToken cancellationToken)
        {
            var source = await _repository.GetNodeById(request.SourceNodeId)
                ?? throw new NotFoundException(nameof(Node), request.SourceNodeId);

            var target = await _repository.GetNodeById(request.TargetNodeId)
                ?? throw new NotFoundException(nameof(Node), request.TargetNodeId);

            if (await _repository.EdgeExists(request.SourceNodeId, request.TargetNodeId))
                throw new BadRequestException(
                    $"Edge from node {request.SourceNodeId} to node {request.TargetNodeId} already exists.");

            var edge = new Edge
            {
                SourceNodeId = request.SourceNodeId,
                TargetNodeId = request.TargetNodeId
            };

            _repository.CreateEdge(edge);
            await _repository.SaveChangesAsync();

            var dto = _mapper.Map<EdgeCreatedDto>(edge);
            return Result<EdgeCreatedDto>.Success(dto);
        }
    }
}
