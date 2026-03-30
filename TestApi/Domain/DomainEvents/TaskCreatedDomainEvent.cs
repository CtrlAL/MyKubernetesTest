using TaskService.DomainEvents.Base;

namespace TaskService.DomainEvents
{
    public sealed record class TaskCreatedDomainEvent(Entities.Task Task) : IDomainEvent
    {
    }
}
