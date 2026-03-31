using AutoMapper;
using GraphService.Application.Dto;
using GraphService.Application.Interfaces;
using GraphService.Application.Messaging;
using GraphService.Domain.Shared;

namespace GraphService.Application.Queries.GetAllNodes
{
    public class GetAllNodesQueryHandler : IQueryHandler<GetAllNodesQuery, Result<List<ReadNodeDto>>>
    {
        private readonly IGraphRepository _repository;
        private readonly IMapper _mapper;

        public GetAllNodesQueryHandler(IGraphRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<List<ReadNodeDto>>> Handle(GetAllNodesQuery request, CancellationToken cancellationToken)
        {
            var nodes = await _repository.GetAllNodes();
            var dtos = _mapper.Map<List<ReadNodeDto>>(nodes);
            return Result<List<ReadNodeDto>>.Success(dtos);
        }
    }
}
