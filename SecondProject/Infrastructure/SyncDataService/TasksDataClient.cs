using AutoMapper;
using Grpc.Net.Client;
using NotificationService.Dtos;
using TaskService;

namespace NotificationService.SyncDataService
{
    public class TasksDataClient : ITasksDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public TasksDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<List<ReadTaskDto>> GetAllTasks()
        {
            Console.WriteLine($"Calling GRPC Service from {_configuration["GrpcTasks"]}");
            var channel = GrpcChannel.ForAddress(_configuration["GrpcTasks"]!);
            var client = new GrpcTasks.GrpcTasksClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = await client.GetAllTasksAsync(request);
                return _mapper.Map<List<ReadTaskDto>>(reply.Tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not call the grpc server {ex.Message}");
                return new List<ReadTaskDto>();
            }
        }
    }
}
