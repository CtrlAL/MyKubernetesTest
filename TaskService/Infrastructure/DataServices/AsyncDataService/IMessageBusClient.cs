using TaskService.Dto;

namespace TaskService.Infrastructure.DataServices.AsyncDataService
{
    public interface IMessageBusClient
    {
        Task PublishNewTask(TaskCreatedDto taskCreatedDto);
    }
}
