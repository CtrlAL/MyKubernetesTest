using NotificationService.Dtos;

namespace NotificationService.SyncDataService
{
    public interface ITasksDataClient
    {
        public Task<List<ReadTaskDto>> GetAllTasks();
    }
}
