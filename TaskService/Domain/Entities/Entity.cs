using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TaskService.DomainEvents.Base;

namespace TaskService.Entities
{
    public class Entity
    {
        [NotMapped]
        [JsonIgnore]
        public List<IDomainEvent> DomainEvents { get; } = [];

        public void RaiseEvent(IDomainEvent @event) => DomainEvents.Add(@event);
        public void ClearEvents() => DomainEvents.Clear();
    }
}
