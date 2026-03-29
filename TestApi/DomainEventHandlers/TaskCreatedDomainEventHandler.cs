using AutoMapper;
using MediatR;
using TaskService.AsyncDataService;
using TaskService.DomainEvents;
using TaskService.Dto;
using TaskService.SyncDataService;

namespace TaskService.DomainEventHandlers
{
    public class TaskCreatedDomainEventHandler : INotificationHandler<TaskCreatedDomainEvent>
    {
        private readonly IMapper _mapper;
        private readonly INotificationDataClient _notificationDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public TaskCreatedDomainEventHandler(IMapper mapper, INotificationDataClient notificationDataClient, IMessageBusClient messageBusClient)
        {
            _mapper = mapper;
            _notificationDataClient = notificationDataClient;
            _messageBusClient = messageBusClient;
        }

        public async Task Handle(TaskCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var entity = notification.Task;

                var syncEvent = _mapper.Map<SendNotificatioDto>(entity);
                var asyncEvent = _mapper.Map<TaskCreatedDto>(entity);

                await _notificationDataClient.SendToReportService(syncEvent);
                await _messageBusClient.PublishNewTask(asyncEvent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not send synchronously: {ex.Message}");
            }
        }
    }
}
