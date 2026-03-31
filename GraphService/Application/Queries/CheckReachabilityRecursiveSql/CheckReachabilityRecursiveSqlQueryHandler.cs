using GraphService.Application.Dto;
using GraphService.Application.Interfaces;
using GraphService.Application.Messaging;
using GraphService.Domain.Exceptions;
using GraphService.Domain.Shared;

namespace GraphService.Application.Queries.CheckReachabilityRecursiveSql
{
    public class CheckReachabilityRecursiveSqlQueryHandler
        : IQueryHandler<CheckReachabilityRecursiveSqlQuery, Result<ReachabilityResultDto>>
    {
        private readonly IGraphRepository _repository;

        public CheckReachabilityRecursiveSqlQueryHandler(IGraphRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<ReachabilityResultDto>> Handle(
            CheckReachabilityRecursiveSqlQuery request,
            CancellationToken cancellationToken)
        {
            _ = await _repository.GetNodeById(request.SourceNodeId)
                ?? throw new NotFoundException("Node", request.SourceNodeId);

            _ = await _repository.GetNodeById(request.TargetNodeId)
                ?? throw new NotFoundException("Node", request.TargetNodeId);

            var (isReachable, path) = await _repository
                .CheckReachabilityRecursiveSql(request.SourceNodeId, request.TargetNodeId);

            var dto = new ReachabilityResultDto
            {
                SourceNodeId = request.SourceNodeId,
                TargetNodeId = request.TargetNodeId,
                IsReachable = isReachable,
                Path = path
            };

            return Result<ReachabilityResultDto>.Success(dto);
        }
    }
}
