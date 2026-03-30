using MediatR;
using Microsoft.EntityFrameworkCore;
using Quartz;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using TaskService.Data;
using TaskService.DomainEvents.Base;
using TaskService.Outbox;

namespace TaskService.BackgroundJob
{
    [DisallowConcurrentExecution]
    public class ProcessOutboxMessageJob : IJob
    {
        private readonly AppDbContext _appDbContext;
        private readonly IPublisher _publisher;

        public ProcessOutboxMessageJob(AppDbContext appDbContext, IPublisher publisher)
        {
            _appDbContext = appDbContext;
            _publisher = publisher;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var messages = await _appDbContext.Set<OutboxMessage>()
                .Where(m => m.ProcededOnUtc == null)
                .Take(20)
                .ToListAsync();

            foreach (var outboxMessage in messages)
            {
                var options = new JsonSerializerOptions
                {
                    TypeInfoResolver = new DefaultJsonTypeInfoResolver()
                };

                var type = Type.GetType(outboxMessage.Type);
                IDomainEvent? domainEvent = JsonSerializer.Deserialize(outboxMessage.Content, type!) as IDomainEvent;

                if (domainEvent is null)
                {
                    continue;
                }

                await _publisher.Publish(domainEvent);
                outboxMessage.ProcededOnUtc = DateTime.UtcNow;
            }

            await _appDbContext.SaveChangesAsync();
        }
    }
}
