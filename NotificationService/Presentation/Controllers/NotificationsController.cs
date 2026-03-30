using Microsoft.AspNetCore.Mvc;
using NotificationService.Dtos;
using NotificationService.Infrastructure.DataServices.SyncDataService;
using NotificationService.Models;

namespace SecondProject.Controllers
{
    [ApiController]
    [Route("api/n/[controller]")]
    [Produces("application/json")]
    public class NotificationsController : ControllerBase
    {
        private readonly ILogger<NotificationsController> _logger;
        private readonly ITasksDataClient _tasksDataClient;

        public NotificationsController(ILogger<NotificationsController> logger, ITasksDataClient tasksDataClient)
        {
            _logger = logger;
            _tasksDataClient = tasksDataClient;
        }

        [HttpPost(Name = "SendNotification")]
        [Route("get-tasks")]
        public ActionResult Post([FromBody] SendNotificationModel model)
        {
            Console.WriteLine("Apply from Test Api");

            return Ok();
        }

        [HttpGet(Name = "GetTasksFromNotificationService")]
        public async Task<ActionResult<List<ReadTaskDto>>> GetTasks()
        {
            var response = await _tasksDataClient.GetAllTasks();

            return Ok(response);
        }
    }
}