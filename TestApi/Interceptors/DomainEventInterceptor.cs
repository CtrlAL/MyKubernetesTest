using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
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

        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            PublishDomainEvent(eventData.Context).GetAwaiter().GetResult();
            return base.SavedChanges(eventData, result);
        }

        public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            await PublishDomainEvent(eventData.Context!);
            return await base.SavedChangesAsync(eventData, result, cancellationToken);
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

            var options = new JsonSerializerOptions
            {
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            };

            var outboxMessages = domainEvents.Select(e => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccuredOnUtc = DateTime.UtcNow,
                Type = e.GetType().Name,
                Content = JsonSerializer.Serialize((object)e, options),
            });

            await context.Set<OutboxMessage>().AddRangeAsync(outboxMessages);
        }

        private async Task ConvertToOutboxAsync() 
        { 
        }
    }
}
