using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using TaskService.DomainEvents.Base;
using TaskService.Outbox;

namespace TaskService.Interceptors
{
    public class DomainEventInterceptor : SaveChangesInterceptor
    {
        private readonly IPublisher _mediator;

        public DomainEventInterceptor(IPublisher mediatr)
        {
            _mediator = mediatr;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            PublishDomainEvent(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }

        public async override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await PublishDomainEvent(eventData.Context!);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private async Task PublishDomainEvent(DbContext? context)
        {
            if (context is null)
            {
                return;
            }

            var entitesWithEvents = context.ChangeTracker.Entries<Entities.Entity>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity);

            var domainEvents = entitesWithEvents
                .SelectMany(e => e.DomainEvents)
                .ToList();

            foreach (var item in entitesWithEvents)
            {
                item.ClearEvents();
            }

            var outboxMessages = ConvertToOutbox(domainEvents);

            context.Set<OutboxMessage>().AddRange(outboxMessages);
        }

        private IEnumerable<OutboxMessage> ConvertToOutbox(IEnumerable<IDomainEvent> domainEvents) 
        {
            var options = new JsonSerializerOptions
            {
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            };

            var outboxMessages = domainEvents.Select(e => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccuredOnUtc = DateTime.UtcNow,
                Type = e.GetType().AssemblyQualifiedName!,
                Content = JsonSerializer.Serialize((object)e, options),
            });

            return outboxMessages;
        }
    }
}
