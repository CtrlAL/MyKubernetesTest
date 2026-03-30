using AutoMapper;
using TaskService.Application.Messaging;
using TaskService.Data.Interfaces;
using TaskService.Domain.Shared;
using TaskService.Dto;

namespace TaskService.Application.Queries.GetAllTask
{
    public class GetAllQueryHandler : IQueryHandler<GetAllQuery, List<ReadTaskDto>>
    {
        private readonly IMapper _mapper;
        private readonly ITaskRepository _taskRepository;

        public GetAllQueryHandler(IMapper mapper, ITaskRepository taskRepository)
        {
            _mapper = mapper;
            _taskRepository = taskRepository;
        }

        public async Task<Result<List<ReadTaskDto>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var taks = await _taskRepository.GetAll();
            var response = _mapper.Map<List<ReadTaskDto>>(taks);

            return Result<List<ReadTaskDto>>.Success(response);
        }
    }
}
