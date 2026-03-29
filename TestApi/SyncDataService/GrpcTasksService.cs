using AutoMapper;
using Grpc.Core;
using TaskService.Data.Interfaces;

namespace TaskService.SyncDataService
{
    public class GrpcTasksService : GrpcTasks.GrpcTasksBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public GrpcTasksService(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public override async Task<TasksResponse> GetAllTasks(GetAllRequest getAll, ServerCallContext context)
        {
            var tasks = await _taskRepository.GetAll();
            var tasksModels = _mapper.Map<List<GrpsTaskModel>>(tasks);
            var response = new TasksResponse();
            response.Tasks.AddRange(tasksModels);

            return response;
        }
    }
}
