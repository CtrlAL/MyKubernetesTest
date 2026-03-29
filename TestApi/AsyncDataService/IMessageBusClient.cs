using TaskService.Dto;

namespace TaskService.AsyncDataService
{
    public interface IMessageBusClient
    {
        Task PublishNewTask(TaskCreatedDto taskCreatedDto);
    }
}
