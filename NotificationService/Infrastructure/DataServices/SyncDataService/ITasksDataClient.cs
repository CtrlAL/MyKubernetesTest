using NotificationService.Dtos;

namespace NotificationService.Infrastructure.DataServices.SyncDataService
{
    public interface ITasksDataClient
    {
        public Task<List<ReadTaskDto>> GetAllTasks();
    }
}
