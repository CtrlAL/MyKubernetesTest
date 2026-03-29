using AutoMapper;
using NotificationService.Dtos;
using System.Text.Json;

namespace NotificationService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }

        public Task ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            if (eventType == EventType.TaskCreated)
            {
                EventHandle(message);
            }
            
            return Task.CompletedTask;
        }

        private EventType DetermineEvent(string message)
        {
            Console.WriteLine(message);
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(message) 
                ?? throw new InvalidCastException();

            return eventType.Event switch
            {
                "Task_Created" => EventType.TaskCreated,
                _ => EventType.Undefined,
            };
        }

        private void EventHandle(string message)
        {
            var @event = JsonSerializer.Deserialize<TaskCreatedDto>(message)
                ?? throw new InvalidCastException();

            Console.WriteLine($"We accept {@event.Name}");
        }

        enum EventType
        {
            Undefined = 0,
            TaskCreated
        }
    }
}