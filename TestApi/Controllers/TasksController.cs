using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskService.Models;
using TaskService.SyncDataService;
using TaskService.Dto;
using TaskService.Data.Interfaces;
using TaskService.AsyncDataService;

namespace TaskService.Controllers
{
    [ApiController]
    [Route("api/t/[controller]")]
    [Produces("application/json")]
    public class TasksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TasksController> _logger;
        private readonly INotificationDataClient _notificationDataClient;
        private readonly ITaskRepository _taskRepository;
        private readonly IMessageBusClient _messageBusClient;

        public TasksController(IMapper mapper, ILogger<TasksController> logger, INotificationDataClient notificationDataClient, ITaskRepository taskRepository, IMessageBusClient messageBusClient)
        {
            _mapper = mapper;
            _logger = logger;
            _notificationDataClient = notificationDataClient;
            _taskRepository = taskRepository;
            _messageBusClient = messageBusClient;
        }

        [HttpGet(Name = "GetTasks")]
        public async Task<ActionResult<List<Entities.Task>>> Get()
        {
            var taks = await _taskRepository.GetAll();
            return Ok(taks);
        }

        [HttpPost(Name = "SaveTask")]
        public async Task<ActionResult<ReadTaskDto>> Post([FromBody] CreateTaskModel model)
        {
            var entity = _mapper.Map<Entities.Task>(model);
            _taskRepository.Create(entity);
            await _taskRepository.SaveChangesAsync();

            return Ok(_mapper.Map<ReadTaskDto>(entity));
        }
    }
}
