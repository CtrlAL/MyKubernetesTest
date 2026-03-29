using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using TaskService.DomainEvents.Base;

namespace TaskService.Entities
{
    public class Entity
    {
        [NotMapped]
        public List<IDomainEvent> DomainEvents { get; } = [];

        public void RaiseEvent(IDomainEvent @event) => DomainEvents.Add(@event);
        public void ClearEvents() => DomainEvents.Clear();
    }
}
