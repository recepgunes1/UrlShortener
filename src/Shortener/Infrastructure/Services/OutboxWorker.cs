using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Extensions;
using Shortener.Infrastructure.Context;
using Shortener.Infrastructure.Models;
using System.Text.Json;

namespace Shortener.Infrastructure.Services
{
    public class OutboxWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IBus _bus;
        private readonly ILogger<OutboxWorker> _logger;

        public OutboxWorker(IServiceScopeFactory scopeFactory, IBus bus, ILogger<OutboxWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _bus = bus;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await PublishOutboxMessages(stoppingToken);
            }
        }

        private async Task PublishOutboxMessages(CancellationToken stoppingToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                try
                {
                    List<OutboxMessage> messages = dbContext.OutboxMessages.Where(p => !p.IsSent).ToList();
                    for (int i = 0; i < messages.Count; i++)
                    {
                        try
                        {
                            var outboxMessage = messages[i];
                            var messageType = Type.GetType(outboxMessage.EventType)!;
                            var message = JsonSerializer.Deserialize(outboxMessage.EventPayload, messageType)!;
                            await _bus.Publish(message, messageType);
                            outboxMessage.IsSent = true;
                            outboxMessage.SentDate = DateTime.UtcNow;
                            await dbContext.SaveChangesAsync();
                            _logger.LogInformation($"Outbox ID: {outboxMessage.Id} was published to consumer");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex.ToJsonString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex.ToJsonString());
                }
            }
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
