using AutoMapper;
using TaskService.Application.Messaging;
using TaskService.Data.Interfaces;
using TaskService.Domain.Shared;
using TaskService.Dto;

namespace TaskService.Application.Commands.CreateTask
{
    public class CreateTaskCommandHandler : ICommandHendler<CreateTaskCommand, ReadTaskDto>
    {
        private readonly IMapper _mapper;
        private readonly ITaskRepository _taskRepository;

        public CreateTaskCommandHandler(IMapper mapper, ITaskRepository taskRepository)
        {
            _mapper = mapper;
            _taskRepository = taskRepository;
        }

        public async Task<Result<ReadTaskDto>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Entities.Task>(request);
            _taskRepository.Create(entity);
            await _taskRepository.SaveChangesAsync();

            return Result<ReadTaskDto>.Success(_mapper.Map<ReadTaskDto>(entity));
        }
    }
}
